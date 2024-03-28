using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using libme_scrapper.code.dto;
using static libme_scrapper.code.IndexHelper;

namespace libme_scrapper.code;

partial class Index : Window {
    // bool isDividedByVolumes = false; // TODO под вопросом
    // bool isDividedByNChapters = false;
    int nChapters = 50;
    List<Chapter>? tableOfContents; // TODO уточнить
    
    public Index() {
        InitializeComponent();
        #if DEBUG
                this.AttachDevTools();
        #endif
    }

    void GetTableOfContents(object? sender, RoutedEventArgs e) {
        GetTableOfContentsButton.IsEnabled = false;
        string url = AddLinkField.Text?.Trim() ?? "";

        if (CheckUrl(url, ref FooterTextBlock)) {
            SetDisable(SaveToLocalButton, GlobalCheckbox, ReverseTableShowButton);
            // ChaptersGrid.Focus = true; // TODO 
            
            if (tableOfContents != null && tableOfContents?.Count > 0) {
                tableOfContents.Clear();
            }

            // Parser.GetTableOfContents(url);
            
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
            new Window 
            { 
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