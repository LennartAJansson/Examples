namespace ExcelImport.Abstract
{
    public interface IExchangeInformationFactory
    {
        IServiceProvider Provider { get; }

        //TODO! Rethink a bit further?
        IExchangeInformationFactory AddHandler<TIn>(Func<Type> factory) where TIn : IExchangeInformationHandler;

        TIn GetHandler<TIn>() where TIn : IExchangeInformationHandler;
    }
}
