using CsvHelper;
using CsvHelper.Configuration;

using ExcelImport.Abstract;
using ExcelImport.Contracts;
using ExcelImport.Models;

using Microsoft.Extensions.Logging;

using System.Globalization;
using System.Text;

public class ExcelImportHandler : IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>
{
    private readonly ILogger<ExcelImportHandler> logger;
    private readonly IExchangeInformationTranslator<IEnumerable<Contract>, IEnumerable<Model>> translator;
    private readonly string filename;

    public ExcelImportHandler(ILogger<ExcelImportHandler> logger, IExchangeInformationTranslator<IEnumerable<Contract>, IEnumerable<Model>> translator)
    {
        this.logger = logger;
        this.translator = translator;
        filename = ".\\Import.csv";
    }

    public async Task<IEnumerable<Contract>> GetData()
    {
        logger.LogInformation("Getting Contract");
        return await ImportData(filename);
    }

    public Task<IEnumerable<Model>> Translate(IEnumerable<Contract> data)
    {
        logger.LogInformation("Translating Contract to Model");
        return translator.Translate(data);
    }

    private Task<IEnumerable<Contract>> ImportData(string filename)
    {
        IEnumerable<Contract> records = Enumerable.Empty<Contract>();

        EncodingProvider provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);

        CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            Delimiter = ";",
            Encoding = Encoding.GetEncoding(1252),
            HasHeaderRecord = true
        };

        try
        {
            using (StreamReader reader = new StreamReader(filename, Encoding.GetEncoding(1252)))
            using (CsvReader csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                records = csv.GetRecords<Contract>().ToList();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
        }

        return Task.FromResult(records);
    }

}
