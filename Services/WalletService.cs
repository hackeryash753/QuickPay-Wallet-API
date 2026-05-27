using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using QuickPay.Data;
using QuickPay.Exceptions;
using QuickPay.Models.Domain;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;
using System.Security.Claims;

namespace QuickPay.Services
{
    public class WalletService : IWalletService
    {
        public readonly QuickPayDbContext dbContext;
        public readonly RabbitMQService rabbitMQService;
        public readonly IDistributedCache cache;
        public WalletService(QuickPayDbContext dbContext, RabbitMQService rabbitMQService, IDistributedCache cache)
        {
            this.dbContext = dbContext;
            this.rabbitMQService = rabbitMQService;
            this.cache = cache;   
        }

        public async Task<SendMoneyResponseDto> SendMoneyAsync(SendMoneyDto dto)
        {
            using var transaction =
                await dbContext.Database.BeginTransactionAsync();

            try
            {
                if (dto.Amount <= 0)
                    throw new ValidationException("Invalid amount");

                var senderWallet = await dbContext.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == dto.SenderId);

                var receiverUser = await dbContext.Users
                    .FirstOrDefaultAsync(u =>
                        u.Email.ToLower() == dto.ReceiverEmail.ToLower());

                if (receiverUser == null)
                    throw new NotFoundException("Receiver not found");

                var receiverWallet = await dbContext.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == receiverUser.Id);

                if (senderWallet.Balance < dto.Amount)
                    throw new ValidationException("Insufficient balance");

                senderWallet.Balance -= dto.Amount;
                receiverWallet.Balance += dto.Amount;

                dbContext.Transactions.Add(new Transactions
                {
                    SenderWalletId = senderWallet.Id,
                    ReceiverWalletId = receiverWallet.Id,
                    Amount = dto.Amount,
                    Type = "TRANSFER",
                    Status = "SUCCESS",
                    CreatedAt = DateTime.UtcNow
                });

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                await rabbitMQService.PublishMessage(
                      $"Transaction Success: {dto.Amount} sent");

                await cache.RemoveAsync(
                    $"wallet:balance:{senderWallet.Id}");

                await cache.RemoveAsync(
                    $"wallet:balance:{receiverWallet.Id}");

                return new SendMoneyResponseDto
                {
                    SenderWalletId = senderWallet.Id,
                    ReceiverWalletId = receiverWallet.Id,
                    Amount = dto.Amount,
                    Type = "TRANSFER",
                    Status = "SUCCESS",
                    CreatedAt = DateTime.UtcNow
                };

            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();

                throw new ValidationException(
                    "Wallet was modified by another request. Please retry.");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
           



        }

        public async Task<AddMoneyResponeDto> AddMoneyAsync(AddMoneyDto addMoneyDto)
        {
            await using var transaction =
                await dbContext.Database.BeginTransactionAsync();

            if (addMoneyDto.amount <= 0)
            {
                throw new ValidationException("Invalid Amount");
            }

            var wallet = await dbContext.Wallets
                .FirstOrDefaultAsync(
                    u => u.UserId == addMoneyDto.userid);

            if (wallet == null)
            {
                throw new NotFoundException("Could not find Wallet");
            }

            wallet.Balance += addMoneyDto.amount;
            wallet.LastUpdatedAt = DateTime.UtcNow;

            dbContext.Transactions.Add(new Transactions
            {
                SenderWalletId = addMoneyDto.userid,
                ReceiverWalletId = addMoneyDto.userid,
                Amount = addMoneyDto.amount,
                Type = "Credit",
                Status = "Success",
                CreatedAt = DateTime.UtcNow
            });

            try
            {
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();

                throw new ValidationException(
                    "Wallet was modified by another request. Please retry.");
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }

            await cache.RemoveAsync(
                $"wallet:balance:{addMoneyDto.userid}");

            return new AddMoneyResponeDto
            {
                Message = "Money Added Successfully",
                UpdatedBalance = wallet.Balance
            };
        }

        public async Task<WalletResponeDto> GetBalanceAsync(int userId)
        {
            var cacheKey = $"wallet:balance:{userId}";

            // CHECK CACHE FIRST
            var cachedBalance = await cache.GetStringAsync(cacheKey);

            if (cachedBalance != null)
            {
                Console.WriteLine("DATA FROM REDIS CACHE");

                return new WalletResponeDto
                {
                    UserId = userId,
                    Balance = decimal.Parse(cachedBalance),
                    LastUpdatedAt = DateTime.UtcNow
                };
            }

            Console.WriteLine("DATA FROM DATABASE");

            // FETCH FROM DATABASE
            var wallet = await dbContext.Wallets
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (wallet == null)
            {
                throw new NotFoundException("Wallet not found");
            }

            // STORE IN REDIS CACHE
            await cache.SetStringAsync(
                cacheKey,
                wallet.Balance.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                });

            // RETURN RESPONSE
            return new WalletResponeDto
            {
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                LastUpdatedAt = wallet.LastUpdatedAt
            };
        }




    }

}
