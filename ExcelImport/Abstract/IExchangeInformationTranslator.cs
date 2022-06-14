namespace ExcelImport.Abstract
{
    public interface IExchangeInformationTranslator<TIn, TOut>
        where TIn : class
        where TOut : class
    {
        Task<TOut> Translate(TIn data);
    }
}
