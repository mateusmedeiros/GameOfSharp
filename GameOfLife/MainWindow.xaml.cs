using System;
using System.Collections.Generic;
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

namespace GameOfLife {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            int width = 43;
            int height = 49;

            for (var i = 0; i < width - 1; i++) {
                this.Main.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (var i = 0; i < height - 1; i++) {
                this.Main.RowDefinitions.Add(new RowDefinition());
            }

            for (var i = 0; i < width - 1; i++) {
                for (var j = 0; j < height - 1; j++) {
                    var rec = new Rectangle() {
                        Fill = new SolidColorBrush(System.Windows.Media.Colors.DimGray),
                        Stroke = new SolidColorBrush(System.Windows.Media.Colors.LightSlateGray),
                        StrokeThickness = 0.3
                    };
                    this.Main.Children.Add(rec);
                    Grid.SetColumn(rec, i);
                    Grid.SetRow(rec, j);
                }
            }
        }
    }
}