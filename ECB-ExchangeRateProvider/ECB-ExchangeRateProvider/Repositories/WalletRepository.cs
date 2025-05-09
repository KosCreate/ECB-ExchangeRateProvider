﻿using ECB_ExchangeRateProvider.Data;
using ECB_ExchangeRateProvider.DBContexts;
using ECB_ExchangeRateProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace ECB_ExchangeRateProvider.Repositories {
    public class WalletRepository(
        ExchangeDbContext exchangeRateDBContext,
        IExchangeRateDbRepository exchangeRateDbRepository) : IWalletRepository {

        private readonly ExchangeDbContext _exchangeRateDBContext = exchangeRateDBContext;
        private readonly IExchangeRateDbRepository _exchangeRateDbRepository = exchangeRateDbRepository;

        public async Task<WalletModel> CreateWalletAsync(WalletModel wallet) {
            await _exchangeRateDBContext.AddAsync(wallet);
            await _exchangeRateDBContext.SaveChangesAsync();
            return wallet;
        }

        /// <summary>
        /// Adjust wallet balance by the specified amount
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="strategy">Used an enum to be able to validate the request attribute and for ease-of use in the application</param>
        /// <returns></returns>
        /// <exception cref="Exception">Throws an exception if the strategy is invalid</exception>
        public async Task<WalletModel> AdjustWalletBalanceAsync(long walletId, decimal amount, string currency, Strategy strategy) {
            var wallet = await GetWalletAsync(walletId) ?? throw new Exception("Could not find specified wallet");

            if (!wallet.Currency!.Equals(currency)) {
                var exchangeRate = await _exchangeRateDbRepository.GetLatestExchangeRateAsync(currency) ?? throw new Exception("Could not fetch latest exchange rates");
                amount *= exchangeRate;
            }

            switch (strategy) {
                    case Strategy.AddFundsStrategy:
                        wallet.Balance += amount;
                        break;
                    case Strategy.SubtractFundsStrategy:
                        if(wallet.Balance < amount) {
                            throw new Exception("Not enough funds");
                        }
                        wallet.Balance -= amount;
                        break;
                    case Strategy.ForceSubtractFundsStrategy:
                        wallet.Balance -= amount;
                        break;
                    default:
                        throw new Exception("Invalid strategy");
                }

            await _exchangeRateDBContext.SaveChangesAsync();

            return wallet;
        }

        public async Task<decimal?> RetrieveWalletBalanceAsync(long walletId, string? currency) {
            var wallet = await GetWalletAsync(walletId);

            if (wallet == null) {
                return null;
            }

            decimal balance = wallet.Balance;

            if (!string.IsNullOrEmpty(currency) && !wallet.Currency!.Equals(currency)) {
                var exchangeRate = await _exchangeRateDbRepository.GetLatestExchangeRateAsync(currency) ?? throw new Exception("Could not fetch latest exchange rates");

                balance *= exchangeRate;
            }

            return balance;
        }

        public async Task<WalletModel?> GetWalletAsync(long walletId) {
            return await _exchangeRateDBContext.Wallets.FirstOrDefaultAsync(item => item.Id.Equals(walletId));
        }
    }
}
