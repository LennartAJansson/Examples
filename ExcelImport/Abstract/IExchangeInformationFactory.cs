namespace ExcelImport.Abstract
{
    public interface IExchangeInformationFactory
    {
        IServiceProvider Provider { get; }

        //TODO! Rethink a bit further?
        IExchangeInformationFactory AddBuilder<TIn>(Func<Type> factory) where TIn : IExchangeInformationHandler;

        TIn GetBuilder<TIn>() where TIn : IExchangeInformationHandler;
    }
}
