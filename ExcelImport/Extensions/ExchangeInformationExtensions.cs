namespace ExcelImport.Extensions
{
    using ExcelImport.Abstract;
    using ExcelImport.Core;

    using Microsoft.Extensions.DependencyInjection;

    public static class ExchangeInformationExtensions
    {
        public static IServiceCollection AddExchangeInformation(this IServiceCollection services, Action<IServiceCollection> svc, Action<IExchangeInformationFactory> configure)
        {
            svc.Invoke(services);
            ServiceProvider provider = services.BuildServiceProvider();
            IExchangeInformationFactory factory = new ExchangeInformationFactory(provider);

            services.AddSingleton<IExchangeInformationFactory>(factory);
            configure.Invoke(factory);

            return services;
        }
    }
}
