namespace ExcelImport.Exporter
{
    using CsvHelper;
    using CsvHelper.Configuration;

    using ExcelImport.Abstract;
    using ExcelImport.Contracts;
    using ExcelImport.Models;

    using Microsoft.Extensions.Logging;

    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;

    public class ExcelExportHandler : IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>
    {
        private readonly ILogger<ExcelExportHandler> logger;
        private readonly IExchangeInformationTranslator<IEnumerable<Model>, IEnumerable<Contract>> translator;
        private readonly string filename;

        public ExcelExportHandler(ILogger<ExcelExportHandler> logger, IExchangeInformationTranslator<IEnumerable<Model>, IEnumerable<Contract>> translator)
        {
            this.logger = logger;
            this.translator = translator;
            filename = ".\\Export.csv";
        }

        public Task<IEnumerable<Contract>> Translate(IEnumerable<Model> data)
        {
            logger.LogInformation("Translating Model to Contract");
            return translator.Translate(data);
        }

        public async Task SendData(IEnumerable<Contract> data)
        {
            logger.LogInformation("Sending Contract");
            await ExportData(data);
        }

        private Task ExportData(IEnumerable<Contract> data)
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ";",
                Encoding = Encoding.GetEncoding(1252),
                HasHeaderRecord = true
            };

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                using (CsvWriter csv = new CsvWriter(writer, config))
                {
                    csv.WriteHeader<Contract>();
                    csv.NextRecord();
                    csv.WriteRecords(data);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Something went wrong");
            }

            return Task.CompletedTask;
        }
    }
}
