using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MultiLanguage.Core.Middleware;
using MultiLanguage.Core.Service;

namespace MultiLanguage.Core.Extension
{
    public static class ServiceContainerExtension
    {
        public static void AddMultiLanguageTextRepository(this IServiceCollection services,
            Action<LanguageRepositoryBuilder> configure)
        {
            services.AddMultiLanguageTextRepository(configure, builder => new LanguageRepository(builder));
        }
        public static void AddMultiLanguageTextRepository(this IServiceCollection services,
            Action<LanguageRepositoryBuilder> configure, Func<LanguageRepositoryBuilder, LanguageRepository> build)
        {
            var languageRepositoryBuilder = new LanguageRepositoryBuilder();
            configure.Invoke(languageRepositoryBuilder);

            var languageRepository = build.Invoke(languageRepositoryBuilder);

            services.AddSingleton<ILanguageRepository>(languageRepository);
        }

        public static void UseMultiLanguage(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}