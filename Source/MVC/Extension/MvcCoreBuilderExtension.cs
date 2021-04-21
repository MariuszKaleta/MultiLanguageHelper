using System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MultiLanguage.Core.Middleware;
using MultiLanguage.Core.Validation.Message;

namespace MultiLanguage.MVC.AspNetCore.Extension
{
    public static class MvcCoreBuilderExtension
    {
        public static IMvcCoreBuilder AddMultiLanguageFluentValidation(this IMvcCoreBuilder mvcBuilder,
            Action<FluentValidationMvcConfiguration> configurationExpression = null)
        {
            mvcBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ctx => new ValidationFilter();
            });

            return mvcBuilder.AddFluentValidation(configuration =>
            {
                configurationExpression?.Invoke(configuration);
                configuration.ValidatorOptions.MessageFormatterFactory = () => new MultiLanguageMessageFormatter();
            });
        }
    }
}