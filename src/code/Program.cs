using Avalonia;
using System;
using Avalonia.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Logger = Serilog.Core.Logger;

namespace libme_scrapper.code;

class Program {
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
           .CreateLogger();
        
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        Log.Information("App exit");
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
           .UsePlatformDetect()
           .WithInterFont()
           .LogToTrace(LogEventLevel.Debug, 
                LogArea.Visual,
                LogArea.Layout,
                LogArea.Control,
                LogArea.Binding,
                LogArea.Property
                ); // TODO убрать не забыть
}