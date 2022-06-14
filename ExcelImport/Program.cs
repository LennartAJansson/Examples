using ExcelImport.Abstract;
using ExcelImport.Contracts;
using ExcelImport.Exporter;
using ExcelImport.Extensions;
using ExcelImport.Models;

using Microsoft.Extensions.DependencyInjection;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddExchangeInformation(factory => factory
        .AddBuilder<IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>>(() =>
            typeof(IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>))
        .AddBuilder<IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>>(() =>
            typeof(IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>)));

    services.AddTransient<IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>, ExcelImportHandler>();
    services.AddTransient<IExchangeInformationTranslator<IEnumerable<Contract>, IEnumerable<Model>>, ExcelImportTranslator>();

    services.AddTransient<IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>, ExcelExportHandler>();
    services.AddTransient<IExchangeInformationTranslator<IEnumerable<Model>, IEnumerable<Contract>>, ExcelExportTranslator>();
});

using (IHost host = builder.Build())
{
    await host.StartAsync();

    using (IServiceScope scope = host.Services.CreateScope())
    {
        IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>> importHandler = scope.ServiceProvider.GetRequiredService<IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>>();
        IEnumerable<Model> imported = await importHandler.Translate(await importHandler.GetData());

        IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>> exportHandler = scope.ServiceProvider.GetRequiredService<IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>>();
        await exportHandler.SendData(await exportHandler.Translate(imported));
    }

    await host.StopAsync();
}
