using ExcelImport.Abstract;
using ExcelImport.Contracts;
using ExcelImport.Exporter;
using ExcelImport.Extensions;
using ExcelImport.Models;

using Microsoft.Extensions.DependencyInjection;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>

    services.AddExchangeInformation(svc =>
        {
            svc.AddTransient<IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>, ExcelImportHandler>();
            svc.AddTransient<IExchangeInformationTranslator<IEnumerable<Contract>, IEnumerable<Model>>, ExcelImportTranslator>();

            svc.AddTransient<IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>, ExcelExportHandler>();
            svc.AddTransient<IExchangeInformationTranslator<IEnumerable<Model>, IEnumerable<Contract>>, ExcelExportTranslator>();
        },
        factory => factory
        .AddHandler<IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>>(() =>
            typeof(IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>))
        .AddHandler<IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>>(() =>
            typeof(IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>))));

using (IHost host = builder.Build())
{
    await host.StartAsync();

    using (IServiceScope scope = host.Services.CreateScope())
    {
        IExchangeInformationFactory factory = scope.ServiceProvider.GetRequiredService<IExchangeInformationFactory>();

        IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>> importHandler = factory.GetHandler<IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>>();
        IEnumerable<Model> imported = await importHandler.Translate(await importHandler.GetData());

        IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>> exportHandler = factory.GetHandler<IExportInformationHandler<IEnumerable<Model>, IEnumerable<Contract>>>();
        await exportHandler.SendData(await exportHandler.Translate(imported));
    }

    await host.StopAsync();
}


public class MyWorker
{
    private readonly IExchangeInformationFactory factory;

    public MyWorker(IExchangeInformationFactory factory)
    {
        this.factory = factory;
    }

    public async Task Execute()
    {
        IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>> handler = factory.GetHandler<IImportInformationHandler<IEnumerable<Contract>, IEnumerable<Model>>>();
        IEnumerable<Model> result = await handler.Translate(await handler.GetData());
    }
}
