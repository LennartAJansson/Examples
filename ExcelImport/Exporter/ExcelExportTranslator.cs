namespace ExcelImport.Exporter
{
    using ExcelImport.Abstract;
    using ExcelImport.Contracts;
    using ExcelImport.Models;

    using Microsoft.Extensions.Logging;

    using System.Threading.Tasks;

    public class ExcelExportTranslator : IExchangeInformationTranslator<IEnumerable<Model>, IEnumerable<Contract>>
    {
        private readonly ILogger<ExcelExportTranslator> logger;

        public ExcelExportTranslator(ILogger<ExcelExportTranslator> logger)
        {
            this.logger = logger;
        }

        public Task<IEnumerable<Contract>> Translate(IEnumerable<Model> data)
        {
            logger.LogInformation("Translating {count} Model to Contract", data.Count());
            return Task.FromResult(data.Select(m => new Contract
            {
                Id = m.Id,
                Phonenumber = m.Phonenumber,
                PhoneType = m.PhoneType,
                NumberType = m.NumberType,
                Name = m.Name,
                Comments = m.Comments
            }));
        }
    }
}
