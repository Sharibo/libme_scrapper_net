using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

using libme_scrapper.code.dto;

using Serilog;

using System;
using System.Collections.Generic;

using static libme_scrapper.code.IndexHelper;

namespace libme_scrapper.code;

partial class Index : Window {
    static readonly ILogger LOG = Log.ForContext<Index>();
    // bool isDividedByVolumes = false; // TODO под вопросом
    // bool isDividedByNChapters = false;
    int nChapters = 50;
    IList<Branch>? tableOfContents;

    public Index() {
        InitializeComponent();
        #if DEBUG
                this.AttachDevTools();
        #endif

        LOG.Information("Hello, Index!");
    }

    void GetTableOfContents(object? sender, RoutedEventArgs e) {
        LOG.Information("Hello2222222, Index!");
        // Logger.TryGet(LogEventLevel.Fatal, LogArea.Control)?.Log(this, "Avalonia GetTableOfContents");
        // System.Diagnostics.Debug.WriteLine("System Diagnostics GetTableOfContents");
        // log.Log(this, "Avalonia LOG GetTableOfContents");
        Serilog.Log.Information("GetTableOfContents");
        GetTableOfContentsButton.IsEnabled = false;
        string url = AddLinkField.Text?.Trim() ?? "";

        if (CheckUrl(url, ref FooterTextBlock)) {
            //SetDisable(SaveToLocalButton, GlobalCheckbox, ReverseTableShowButton); //TODO 
            //Log.Information("before focus");
            ChaptersGrid.Focus(); // TODO
            //Log.Information("after focus");

            if (tableOfContents != null && tableOfContents?.Count > 0) {
                tableOfContents.Clear();
            }

            tableOfContents = Parser.GetTableOfContents(url);

            foreach (Branch branch in tableOfContents) {
                Console.WriteLine(branch.ToString());
                //Log.Information(branch.ToString());
            }
            //Log.Information("after foreach");

            // if (Exception == null) { // TODO 
            //     int chapters = showChapters(tableOfContents, tableView);
            //     setFooterLabelAsync("Оглавление загружено. Всего глав: " + chapters);
            //     setTableOfContentsAsync(tableOfContents);
            //     setEnable(saveToLocalButton, globalCheckbox, reverseTableShowButton);
            // } else {
            //     log.error("getTableOfContents() - slave thread failed: " + throwable.getLocalizedMessage());
            //     setFooterLabelAsync("Возникла ошибка при загрузке!");е
            // }

            GetTableOfContentsButton.IsEnabled = true;
        // }); // non-blocking
        } else {
            GetTableOfContentsButton.IsEnabled = false;
        }
    }

    void SetLocalPath(object? sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    void SaveToLocal(object? sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    void ReverseTableShow(object? sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    void IsDividedByVolumesChapters(object? sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    void IsDividedByNChapters(object? sender, RoutedEventArgs e) {
        throw new NotImplementedException();
    }

    void GetAbout(object? sender, RoutedEventArgs e) {
        // Create the window object
        Window sampleWindow =
            new Window {
                Title = "Sample Window",
                Width = 200,
                Height = 200
            };

        // open the window
        // sampleWindow.Show();
        // open the modal (dialog) window
        sampleWindow.ShowDialog(this);
        // throw new NotImplementedException();
    }

    void ChangeTheme(object? sender, RoutedEventArgs e) {
        Close();
    }
    
    void SetEnable(params Control[] controls) {
        foreach (Control control in controls) {
            control.IsEnabled = true;
        }
    }

    void SetDisable(params Control[] controls) {
        foreach (Control control in controls) {
            control.IsEnabled = false;
        }
    }
}