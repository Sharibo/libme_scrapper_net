using Avalonia.Controls;

namespace libme_scrapper.code;

static class IndexHelper {
    
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
    
    // static int showChapters(List<Chapter> tableOfContents,
    //                                   TableView<TableRow> tableView) {
    //
    //     isReversed = false;
    //     if (!tableList.isEmpty()) {
    //         tableList.clear();
    //     }
    //
    //     for (Chapter chapter : tableOfContents) {
    //         Hyperlink url = new Hyperlink(chapter.getChapterLink());
    //
    //         Tooltip tooltip = new Tooltip(chapter.getChapterLink());
    //         tooltip.setShowDelay(new Duration(700));
    //         url.setTooltip(tooltip);
    //
    //         url.setText("url \u2B0F");
    //         url.setOnAction(e -> {
    //             url.setVisited(false);
    //             Desktop desktop = Desktop.getDesktop();
    //             try {
    //                 desktop.browse(java.net.URI.create(chapter.getChapterLink()));
    //             } catch (IOException ex) {
    //                 log.error("Error by opening chapter-url");
    //             }
    //         });
    //
    //         TableRow tableRow = new TableRow(false, chapter.getChapterName(), url);
    //         tableList.add(tableRow);
    //     }
    //
    //     tableListReversed.setAll(tableList);
    //     FXCollections.reverse(tableListReversed);
    //
    //     tableView.getItems().setAll(tableList);
    //
    //     return tableList.size();
    // }

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