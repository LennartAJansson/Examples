namespace ExcelImport.Extensions
{
    using ExcelImport.Abstract;
    using ExcelImport.Core;

    using Microsoft.Extensions.DependencyInjection;

    public static class ExchangeInformationExtensions
    {
        public static IServiceCollection AddExchangeInformation(this IServiceCollection services, Action<IExchangeInformationFactory> configure)
        {
            services.AddSingleton<IExchangeInformationFactory, ExchangeInformationFactory>((s) =>
            {
                IServiceProvider svc = s.GetRequiredService<IServiceProvider>();
                IExchangeInformationFactory factory = new ExchangeInformationFactory(svc);
                configure.Invoke(factory);
                return factory as ExchangeInformationFactory ?? throw new ArgumentNullException();
            });

            return services;
        }
    }
}
