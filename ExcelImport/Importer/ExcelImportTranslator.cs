using ExcelImport.Abstract;
using ExcelImport.Contracts;
using ExcelImport.Models;

using Microsoft.Extensions.Logging;

public class ExcelImportTranslator : IExchangeInformationTranslator<IEnumerable<Contract>, IEnumerable<Model>>
{
    private readonly ILogger<ExcelImportTranslator> logger;

    public ExcelImportTranslator(ILogger<ExcelImportTranslator> logger)
    {
        this.logger = logger;
    }

    public Task<IEnumerable<Model>> Translate(IEnumerable<Contract> data)
    {
        logger.LogInformation("Translating {count} Contract to Model", data.Count());
        return Task.FromResult(data.Select(c => new Model
        {
            Id = c.Id,
            Phonenumber = c.Phonenumber,
            PhoneType = c.PhoneType,
            NumberType = c.NumberType,
            Name = c.Name,
            Comments = c.Comments
        }));
    }
}
