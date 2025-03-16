using ECB_ExchangeRateProvider.Data;
using ECB_ExchangeRateProvider.DTO;

namespace ECB_ExchangeRateProvider.Repositories {
    public interface IWalletRepository {
        Task<WalletDto?> GetWalletAsync(long walletId);
        Task<int> CreateWalletAsync(WalletDto wallet);
        Task<decimal> RetrieveWalletBalanceAsync(long walletId);
        Task<int> AdjustWalletBalanceAsync(long walletId, int balance, string currency, Strategy strategy);
    }
}
