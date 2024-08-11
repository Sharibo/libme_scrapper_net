using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using libme_scrapper.code.dto;
using MathNet.Numerics;
using static libme_scrapper.code.Index;
using Serilog;
using System.Collections.ObjectModel;
using Avalonia.Data;
using System.Linq;

namespace libme_scrapper.code;

static class IndexHelper {

    static bool isReversed = false;
    // static void initializeNChaptersField(TextField nChaptersField, int nChapters) {
    //     nChaptersField.setId("nChaptersField");
    //     nChaptersField.setAlignment(Pos.CENTER);
    //     nChaptersField.setMaxHeight(28.0);
    //     nChaptersField.setMinHeight(28.0);
    //     nChaptersField.setMaxWidth(48.0);
    //     nChaptersField.setMinWidth(48.0);
    //     nChaptersField.setAccessibleText("nChaptersField");
    //     nChaptersField.setTextFormatter(new TextFormatter<>(new IntegerStringConverter(), nChapters, change -> {
    //         String newText = change.getControlNewText();
    //         if (newText.matches("([1-9][0-9]{0,3})?")) {
    //         return change;
    //     }
    //     return null;
    //     }));
    // }


    // static void initializeTableView(TableView<TableRow> tableView) {
    //     tableView.setId("tableView");
    //     tableView.getStyleClass().add("noheader");
    //     tableView.setEditable(true);
    //     tableView.setColumnResizePolicy(TableView.CONSTRAINED_RESIZE_POLICY);
    //
    //     TableColumn<TableRow, Boolean> checkboxColumn = new TableColumn<>(); //TODO переименовать
    //     checkboxColumn.setCellValueFactory(new PropertyValueFactory<>("checkbox"));
    //     checkboxColumn.setCellFactory(new Callback<TableColumn<TableRow, Boolean>, TableCell<TableRow, Boolean>>() {
    //         @Override
    //         public TableCell<TableRow, Boolean> call(TableColumn<TableRow, Boolean> param) {
    //             CheckBoxTableCell<TableRow, Boolean> cell = new CheckBoxTableCell<TableRow, Boolean>();
    //             cell.addEventFilter(MouseEvent.MOUSE_CLICKED, new CheckboxMouseClickEventHandler());
    //             return cell;
    //         }
    //     });
    //     checkboxColumn.setId("checkboxColumn");
    //     checkboxColumn.setMinWidth(32.0);
    //     checkboxColumn.setMaxWidth(32.0);
    //
    //
    //     TableColumn<TableRow, String> nameColumn = new TableColumn<>("Содержание");
    //     nameColumn.setCellValueFactory(new PropertyValueFactory<>("name"));
    //     nameColumn.setId("nameColumn");
    //
    //     TableColumn<TableRow, Hyperlink> urlColumn = new TableColumn<>();
    //     urlColumn.setCellValueFactory(new PropertyValueFactory<>("url"));
    //     urlColumn.setId("urlColumn");
    //     urlColumn.setMinWidth(48.0);
    //     urlColumn.setMaxWidth(48.0);
    //
    //     tableView.getColumns().add(checkboxColumn);
    //     tableView.getColumns().add(nameColumn);
    //     tableView.getColumns().add(urlColumn);
    //
    //     // selecting actions
    //     TableView.TableViewSelectionModel<TableRow> selectionModel = tableView.getSelectionModel();
    //     selectionModel.setSelectionMode(SelectionMode.MULTIPLE);
    //     selectionModel.selectedItemProperty().addListener(new ChangeListener<TableRow>() {
    //         @Override
    //         public void changed(ObservableValue<? extends TableRow> observable, TableRow oldValue, TableRow newValue) {
    //             if (newValue != null) {
    //                 selectedIndices = selectionModel.getSelectedIndices();
    //             }
    //         }
    //     });
    //
    //     tableView.addEventHandler(KeyEvent.KEY_PRESSED, new EventHandler<KeyEvent>() {
    //         @Override
    //         public void handle(KeyEvent event) {
    //             if (event.getCode() == KeyCode.ESCAPE) {
    //                 selectionModel.clearSelection();
    //             }
    //         }
    //     });
    //
    // }

    public static int ShowChapters(ref Grid chaptersGrid, ref DataGrid dataGrid) {
    // public static int ShowChapters(ref Grid chaptersGrid) {
        // public static int ShowChapters() {
        Log.Information("ShowChapters!");
        isReversed = false;
        // if (chaptersGrid.Children.Count > 0) {
        //     tableList.clear();
        // }

        // ObservableCollection<string[]> dataSource = [
        //     [ "A", "B", "C" ],
        //     [ "C", "B", "A" ],
        // ];

        // foreach (int idx in dataSource[0].Select((value, index) => index)) {
        //     dataGrid.Columns.Add(
        //         new DataGridTextColumn { Header = $"{idx + 1}. column", 
        //         Binding = new Binding($"[{idx}]") }
        //     );
        // }

        // dataGrid.AutoGenerateColumns = false;
        // dataGrid.ItemsSource = dataSource;
        // Log.Information("binding complete!");
        // chaptersGrid.ShowGridLines = true;
        // chaptersGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        // chaptersGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(4,
        //     GridUnitType.Pixel)));
        // chaptersGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        // chaptersGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        // chaptersGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        // chaptersGrid.Children
        //    .Add(new Border {
        //        Width = 100,
        //        Height = 25,
        //        [Grid.ColumnSpanProperty] = 3,
        //    });
        // chaptersGrid.Children
        //    .Add(new Border {
        //        Width = 150,
        //        Height = 25,
        //        [Grid.RowProperty] = 1,
        //    });
        // chaptersGrid.Children
        //    .Add(new Border {
        //        Width = 50,
        //        Height = 25,
        //        [Grid.RowProperty] = 1,
        //        [Grid.ColumnProperty] = 2,
        //    });

        // chaptersGrid.Measure(Size.Infinity);

        // // Issue #25 only appears after a second measure
        // chaptersGrid.InvalidateMeasure();
        // chaptersGrid.Measure(Size.Infinity);

        // chaptersGrid.Arrange(new Rect(chaptersGrid.DesiredSize));

        // Log.Information(new Size(204, 50).Equals(chaptersGrid.Bounds.Size).ToString());
        // Log.Information(150d.AlmostEqual(chaptersGrid.ColumnDefinitions[0].ActualWidth).ToString());
        // Log.Information(4d.AlmostEqual(chaptersGrid.ColumnDefinitions[1].ActualWidth).ToString());
        // Log.Information(50d.AlmostEqual(chaptersGrid.ColumnDefinitions[2].ActualWidth).ToString());
        // Log.Information(new Rect(52, 0, 100, 25).Equals(chaptersGrid.Children[0].Bounds).ToString());
        // Log.Information(new Rect(0, 25, 150, 25).Equals(chaptersGrid.Children[1].Bounds).ToString());
        // Log.Information(new Rect(154, 25, 50, 25).Equals(chaptersGrid.Children[2].Bounds).ToString());

        // foreach (Chapter chapter in TableOfContents![0].Chapters) {
        //     // сhapter
        // }

        // {
        //     Hyperlink url = new Hyperlink(chapter.getChapterLink());
        //
        //     Tooltip tooltip = new Tooltip(chapter.getChapterLink());
        //     tooltip.setShowDelay(new Duration(700));
        //     url.setTooltip(tooltip);
        //
        //     url.setText("url \u2B0F");
        //     url.setOnAction(e -> {
        //         url.setVisited(false);
        //         Desktop desktop = Desktop.getDesktop();
        //         try {
        //             desktop.browse(java.net.URI.create(chapter.getChapterLink()));
        //         } catch (IOException ex) {
        //             log.error("Error by opening chapter-url");
        //         }
        //     });
        //
        //     TableRow tableRow = new TableRow(false, chapter.getChapterName(), url);
        //     tableList.add(tableRow);
        // }
        //
        // tableListReversed.setAll(tableList);
        // FXCollections.reverse(tableListReversed);
        //
        // tableView.getItems().setAll(tableList);
        //
        // return tableList.size();
        return -1;
    }

    public static bool CheckUrl(string url, ref TextBlock footerTextBlock) {
        if (url.Equals("")) {
            footerTextBlock.Text = "URL-адрес не задан!";
            return false;
        }

        // TODO:
        // Pattern p = Pattern.compile("^https://ranobelib.me/[A-Za-z0-9-]+[/?].*$");
        // Matcher m = p.matcher(url);
        // if (m.find()) {
        // return true;
        // } else {
        // log.error("Url is not walid: " + url);
        // footerTextBlock.setText("Проверьте URL-адрес!");
        // }

        return false;
    }
}