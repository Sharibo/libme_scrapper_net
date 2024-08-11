using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using libme_scrapper.code.dto;
using static libme_scrapper.code.IndexHelper;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Data;
using libme_scrapper.src.code;

namespace libme_scrapper.code;

partial class Index : Window {
    // bool isDividedByVolumes = false; // TODO под вопросом
    // bool isDividedByNChapters = false;
    int nChapters = 50;
    public static List<Branch>? TableOfContents { get; set; }
    public ObservableCollection<Person> People { get; }
    public Index() {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
        ObservableCollection<string[]> dataSource = [
            [ "A", "B", "C" ],
            [ "C", "B", "A" ],
        ];
        DataGrid1.ItemsSource = dataSource;
        // foreach (int idx in dataSource[0].Select((value, index) => index)) {
        //     DataGrid.Columns.Add(
        //         new DataGridTextColumn { Header = $"{idx + 1}. column", 
        //         Binding = new Binding($"[{idx}]") }
        //     );
        // }

        // DataGrid.AutoGenerateColumns = false;
        // DataGrid.ItemsSource = dataSource;

        List<Person> people = new() {
                new Person("Neil", "Armstrong"),
                new Person("Buzz", "Lightyear"),
                new Person("James", "Kirk")
            };
        
        People = new ObservableCollection<Person>(people);
        Log.Information("binding complete!");
    }

    void GetTableOfContents(object? sender, RoutedEventArgs e) {
        Log.Information("GetTableOfContents started");
        GetTableOfContentsButton.IsEnabled = false;
        string url = AddLinkField.Text?.Trim() ?? "";

        // if (CheckUrl(url, ref FooterTextBlock)) {
            SetDisable(SaveToLocalButton, GlobalCheckbox, ReverseTableShowButton);
            // ChaptersGrid.Focus = true; // TODO 
            Log.Information("GetTableOfContents 2");
            if (TableOfContents != null && TableOfContents?.Count > 0) {
                TableOfContents.Clear();
            }
            Log.Information("GetTableOfContents 3");
            Parser.GetTableOfContents(url);
            // int i = ShowChapters(ref ChaptersGrid, ref DataGrid);
            // Log.Information(i.ToString());

            // if (Exception == null) { // TODO 
            //     int chapters = showChapters(tableOfContents, tableView);
            //     setFooterLabelAsync("Оглавление загружено. Всего глав: " + chapters);
            //     setTableOfContentsAsync(tableOfContents);
            //     setEnable(saveToLocalButton, globalCheckbox, reverseTableShowButton);
            // } else {
            //     log.error("getTableOfContents() - slave thread failed: " + throwable.getLocalizedMessage());
            //     setFooterLabelAsync("Возникла ошибка при загрузке!");
            // }

            GetTableOfContentsButton.IsEnabled = true;
            SetEnable(SaveToLocalButton, GlobalCheckbox, ReverseTableShowButton);
            // }); // non-blocking
        // } else {
        //     GetTableOfContentsButton.IsEnabled = false;
        // }
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
        Log.Information("Hello, GetAbout!");
        Console.WriteLine("Hello, GetAbout2!");
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