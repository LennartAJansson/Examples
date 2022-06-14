using ExcelImport.Abstract;

using Microsoft.Extensions.DependencyInjection;

namespace ExcelImport.Core;

public class ExchangeInformationFactory : IExchangeInformationFactory
{
    private readonly List<Type> Instances = new List<Type>();
    public IServiceProvider Provider { get; private set; }

    public ExchangeInformationFactory(IServiceProvider provider)
    {
        Provider = provider;
    }

    //TODO! Rethink a bit further
    public IExchangeInformationFactory AddBuilder<TIn>(Func<Type> factory) where TIn : IExchangeInformationHandler
    {
        Instances.Add(typeof(TIn));

        return this;
    }

    public TIn GetBuilder<TIn>() where TIn : IExchangeInformationHandler
    {
        if (Instances.Contains(typeof(TIn)))
        {

            return Provider.GetRequiredService<TIn>();
        }

        return default;
    }
}
