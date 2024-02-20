using BM.Common.Extensions;
using BM.Services.BooksManager.DAL.Context;

namespace BM.Services.BooksManager.DAL.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void RunMigrations(this IServiceProvider serviceProvider)
        {
            serviceProvider.RunMigrations<BMDbContext>();
        }
    }
}
