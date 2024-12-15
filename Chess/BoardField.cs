using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chess
{
    internal class BoardField : INotifyPropertyChanged, ICloneable
    {
        static ChessPiece? toMove = null;
        static BoardField? fromMove = null;
        public static List<int> Highlighted = new();
        public int Row { get; set; }
        public int Column { get; set; }
        private ChessPiece? _occupant;
        private string _backgroundColor;
        Board board;
        public ChessPiece? Occupant
        {
            get => _occupant;
            set
            {
                if (_occupant != value)
                {
                    _occupant = value;
                    OnPropertyChanged(nameof(Occupant));
                }
            }
        }
        public string BackgroundColor1
        {
            get => (Row + Column) % 2 == 0 ? "White" : "Gray";
        }
        public string BackgroundColor
        {
            get
            {
                if(_backgroundColor == null)
                {
                    _backgroundColor = (Row + Column) % 2 == 0 ? "White" : "Gray";
                }
                return _backgroundColor;
            }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }
        public ICommand FieldClickedCommand { get; }

        public BoardField(Board board)
        {
            FieldClickedCommand = new RelayCommand(Move);
            this.board = board;
        }

        private void InvokeOccupantAction()
        {
            Occupant?.Move();
        }
        public void HighlightPossible()
        {
            if (_occupant == null)
            {
                return;
            }
            foreach (var field in Occupant.GetPossibleMoves(Row * 8 + Column))
            {
                if (field < 0 || field > 63) continue;

                Board.SaveState(board.Turn);
                Board.Grid[field].Occupant = this.Occupant;
                this.Occupant = null;
                if (board.IAmChecked(board.Turn))
                {
                    board.StepBack();
                    continue;
                }
                else
                {
                    board.StepBack();
                }

                Highlighted.Add(field);
                Board.Grid[field].BackgroundColor = "Green";
            }
        }
        public void UnHighlightPossible()
        {
            foreach (var field in Highlighted)
            {
                Board.Grid[field].BackgroundColor = (Board.Grid[field].Row + Board.Grid[field].Column) % 2 == 0 ? "White" : "Gray";
            }
            Highlighted.Clear();
        }
        private void Move()
        {
            if (Occupant == null && Board.PieceSelected == false)
            {
                return;
            }
            if (fromMove == this)
            {
                fromMove.BackgroundColor = (fromMove.Row + fromMove.Column) % 2 == 0 ? "White" : "Gray";

                fromMove = null;
                toMove = null;
                Board.PieceSelected = false;
                UnHighlightPossible();

                return;
            }
            if (Board.PieceSelected == false)
            {
                if(board.Turn != Occupant.Party)
                {
                    return;
                }
                BackgroundColor = "Red";
                HighlightPossible();
                toMove = Occupant;
                fromMove = this;
                Board.PieceSelected = true;
            }
            else
            {
                if(BackgroundColor != "Green")
                {
                    return;
                }
                Board.SaveState(board.Turn);

                fromMove.BackgroundColor = (fromMove.Row + fromMove.Column) % 2 == 0 ? "White" : "Gray";
                if (Occupant != null)
                {
                    Occupant.IsAlive = false;
                }
                this.Occupant = toMove;
                if(Occupant.Untouched == true)
                {
                    Occupant.Untouched = false;
                }
                Board.PieceSelected = false;
                UnHighlightPossible();
                fromMove.Occupant = null;
                if(board.Turn == ChessPiece.PartyEnum.Black)
                {
                    board.Turn = ChessPiece.PartyEnum.White;
                }
                else
                {
                    board.Turn = ChessPiece.PartyEnum.Black;
                }
                board.StateCheck();
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            var clone = new BoardField(board);
            clone.Column = Column;
            clone.Row = Row;
            clone.BackgroundColor = BackgroundColor;
            if(Occupant != null)
            {
                clone.Occupant = (ChessPiece)Occupant.Clone();
            }
            return clone;
        }
    }
}
