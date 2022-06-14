namespace ExcelImport.Abstract
{
    public interface IExportInformationHandler<TIn, TOut> : IExchangeInformationHandler
        where TIn : class
        where TOut : class
    {
        Task<TOut> Translate(TIn data);
        Task SendData(TOut data);
    }
}
