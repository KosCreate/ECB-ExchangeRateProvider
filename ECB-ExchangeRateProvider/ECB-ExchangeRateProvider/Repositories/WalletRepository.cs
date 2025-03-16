using ECB_ExchangeRateProvider.Data;
using ECB_ExchangeRateProvider.DTO;

namespace ECB_ExchangeRateProvider.Repositories {
    public class WalletRepository : IWalletRepository {
        public Task<int> CreateWalletAsync(WalletDto wallet) {
            throw new NotImplementedException();
        }

        public Task<int> AdjustWalletBalanceAsync(long walletId, int balance, string currency, Strategy strategy) {
            throw new NotImplementedException();
        }

        public Task<WalletDto?> GetWalletAsync(long walletId) {
            throw new NotImplementedException();
        }

        public Task<decimal> RetrieveWalletBalanceAsync(long walletId) {
            throw new NotImplementedException();
        }
    }
}
