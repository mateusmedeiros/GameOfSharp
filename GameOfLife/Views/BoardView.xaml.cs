using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameOfLife.ViewModels;

namespace GameOfLife.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BoardView : Window {

        public BoardView() {
            InitializeComponent();

            var vm = new BoardViewModel();
            var width = int.Parse(ConfigurationManager.AppSettings["width"]);
            var height = int.Parse(ConfigurationManager.AppSettings["height"]);

            for (var i = 0; i < width - 1; i++) {
                this.Main.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (var i = 0; i < height - 1; i++) {
                this.Main.RowDefinitions.Add(new RowDefinition());
            }

            for (var i = 0; i < width - 1; i++) {
                for (var j = 0; j < height - 1; j++) {
                    var rec = new Rectangle() {
                        Style = this.FindResource("CellStyle") as Style,
                        DataContext = vm.Cells[i][j]
                    };
                    this.Main.Children.Add(rec);
                    Grid.SetColumn(rec, i);
                    Grid.SetRow(rec, j);
                }
            }

            vm.StartGame();
        }
    }
}