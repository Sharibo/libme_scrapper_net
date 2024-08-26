using Avalonia;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using System;
using System.Text;

namespace libme_scrapper.code;

class Program {

    static readonly ILogger LOG = Log.ForContext<Program>();
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) {
        Log.Logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .MinimumLevel.Debug()
           .WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                applyThemeToRedirectedOutput: true,
                theme: AnsiConsoleTheme.Sixteen
           )
        //    .WriteTo.Debug(
        //         outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}"
        //    )
           .WriteTo.File(
                "logs/log.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                encoding: Encoding.UTF8,
                shared: true,
                retainedFileCountLimit: 1
           )
           .CreateLogger();

        LOG.Information("app start");
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        LOG.Information("app exit");
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
           .UsePlatformDetect()
           .WithInterFont();
        //    .LogToTrace(
        //         Avalonia.Logging.LogEventLevel.Debug,
        //         Avalonia.Logging.LogArea.Layout,
        //         Avalonia.Logging.LogArea.Control,
        //         Avalonia.Logging.LogArea.Binding,
        //         Avalonia.Logging.LogArea.Property
        //     );
}