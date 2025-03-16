using ECB_ExchangeRateProvider.Data;
using ECB_ExchangeRateProvider.Models;

namespace ECB_ExchangeRateProvider.Repositories {
    public interface IWalletRepository {
        Task<WalletModel?> GetWalletAsync(long walletId);
        Task<WalletModel> CreateWalletAsync(WalletModel wallet);
        Task<decimal?> RetrieveWalletBalanceAsync(long walletId, string? currency);
        Task<WalletModel> AdjustWalletBalanceAsync(long walletId, decimal amount, string currency, Strategy strategy);
    }
}
