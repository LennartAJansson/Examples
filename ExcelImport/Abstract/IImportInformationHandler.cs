namespace ExcelImport.Abstract
{
    public interface IImportInformationHandler<TIn, TOut> : IExchangeInformationHandler
        where TIn : class
        where TOut : class
    {
        Task<TIn> GetData();
        Task<TOut> Translate(TIn data);
    }
}
