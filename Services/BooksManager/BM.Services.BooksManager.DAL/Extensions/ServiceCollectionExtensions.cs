using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BM.Common.Extensions;
using BM.Services.BooksManager.DAL.Context;
using BM.Services.BooksManager.DAL.Repositories.Books;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.BookAuthors;

namespace BM.Services.BooksManager.DAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Extension method to add Data Access Layer (DAL) services to the IServiceCollection.
        public static void AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            // Add the main database context (OMDbContext) to the service collection.
            services.AddBMDbContext<BMDbContext>(configuration);

            // Add repositories to the service collection using a separate method.
            services.AddRepositories();
        }

        // Private method to add repository services to the IServiceCollection.
        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBooksRepository, BooksRepository>();

            services.AddTransient<IAuthorsRepository, AuthorsRepository>();

            services.AddTransient<IBookAuthorsRepository, BookAuthorsRepository>();
        }

    }
}
