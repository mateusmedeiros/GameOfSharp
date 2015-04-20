using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife.ViewModels {
    class BoardViewModel {

        private readonly Timer _timer;

        private readonly Cell[][] _cells = new Cell[int.Parse(ConfigurationManager.AppSettings["width"])][];

        /// <summary>
        /// Create a new instance of BoardViewModel with a default interval for the Timer to update the cells.
        /// </summary>
        public BoardViewModel() : this(200) {}

        /// <summary>
        /// Create a new instance of BoardViewModel with the specified interval as the Timer to update the cells.
        /// </summary>
        /// <param name="interval">Time in milliseconds to set the internal Timer</param>
        public BoardViewModel(long interval) {
            for (var i = 0; i < _cells.Length; i++) {
                _cells[i] = new Cell[int.Parse(ConfigurationManager.AppSettings["height"])];

                for (var j = 0; j < int.Parse(ConfigurationManager.AppSettings["height"]); j++) {
                    _cells[i][j] = new Cell(new Models.Cell());
                }
            }

            // Enumerable of arrays
            var initialPositions = ConfigurationManager.AppSettings["cells"].Split(',').
                Select(s => s.Split(':').Select(int.Parse).ToArray());
            
            foreach (var position in initialPositions) {
                int i = position[0];
                int j = position[1];
                _cells[i][j].Rise();
            }

            _timer = new Timer() { Interval = interval };
            _timer.Elapsed += OnTimerEvent;
        }

        public Cell[][] Cells {
            get {
                return _cells;
            }
        }


        public void StartGame() {
            _timer.Enabled = true;
        }

        private List<Cell> GetNeighbors(int x, int y, Cell[][] oldBoard) {
            var list = new List<Cell>();

            if (x > 0) {
                list.Add(oldBoard[x - 1][y]);

                if (y > 0) {
                    list.Add(oldBoard[x - 1][y - 1]);
                }

                if (y < oldBoard[0].Length - 1) {
                    list.Add(oldBoard[x - 1][y + 1]);
                }
            }

            if (y > 0) {
                list.Add(oldBoard[x][y - 1]);

                if (x < oldBoard.Length - 1) {
                    list.Add(oldBoard[x + 1][y - 1]);
                }
            }

            if (x < oldBoard.Length - 1) {
                list.Add(oldBoard[x + 1][y]);

                if (y < oldBoard[0].Length - 1) {
                    list.Add(oldBoard[x + 1][y + 1]);
                }
            }

            if (y < oldBoard[0].Length - 1) {
                list.Add(oldBoard[x][y + 1]);
            }

            return list;
        }

        private void OnTimerEvent(object sender, ElapsedEventArgs elapsedEventArgs) {
            Cell[][] oldBoard = Cells.Select(arr => arr.Select(c => new Cell(c)).ToArray()).ToArray();
            for (int i = 0; i < _cells.Length; i++) {
                for (int j = 0; j < _cells[i].Length; j++) {

                    Cell cell = _cells[i][j];
                    int livingNeighbors = GetNeighbors(i, j, oldBoard).Count(c => c.Alive);

                    if (cell.Alive) {
                        if (livingNeighbors < 2 || livingNeighbors > 3) {
                            cell.Die();
                        }
                    } else {
                        if (livingNeighbors == 3) {
                            cell.Rise();
                        }  
                    }
                }
            }
        }

        public class Cell : INotifyPropertyChanged {

            private readonly Models.Cell _innerCell;

            public Cell(Cell cell) {
                _innerCell = new Models.Cell() { Alive = cell.Alive };
            }

            public Cell(Models.Cell cell) {
                _innerCell = cell;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public bool Alive {
                get {
                    return _innerCell.Alive;
                }

                set {
                    if (value == _innerCell.Alive) {
                        return;
                    }
                    _innerCell.Alive = value;
                    OnPropertyChanged("Alive");
                }
            }

            public void Die() {
                Alive = false;
            }

            public void Rise() {
                Alive = true;
            }

            protected virtual void OnPropertyChanged(string propertyName) {
                var handler = PropertyChanged;
                if (handler != null) {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        }
    }
}
