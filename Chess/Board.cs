using Chess.Lib;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Chess.ChessPiece;

namespace Chess
{
    internal class Board : INotifyPropertyChanged
    {
        private bool _shortCastlingAvailable;
        private bool _longCastlingAvailable;
        public bool ShortCastingAvailable
        {
            get { return _shortCastlingAvailable; }
            set
            {
                _shortCastlingAvailable = value;
                OnPropertyChanged(nameof(ShortCastingAvailable));
            }
        }
        public bool LongCastingAvailable
        {
            get { return _longCastlingAvailable; }
            set
            {
                _longCastlingAvailable = value;
                OnPropertyChanged(nameof(LongCastingAvailable));
            }
        }
        public static BoardField[] Grid { get; set; } = new BoardField[64];
        static Stack<BoardField[]> History { get; set; } = new();
        static Stack<string> StatusHistory { get; set; } = new();
        static Stack<PartyEnum> SideTurnHistory { get; set; } = new();
        private PartyEnum _turn = PartyEnum.White;
        public PartyEnum Turn
        {
            get => _turn; 
            set
            {
                _turn = value;
                OnPropertyChanged(nameof(Turn));
            }
        }

        public enum StatusEnum
        {
            Checked,
            Checkmated,
            _
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public static bool PieceSelected { get; set; } = false;

        public Board()
        {
            StepBackCommand = new RelayCommand(StepBackAboba);
            ShortCastlingCommand = new RelayCommand(ShortCastling);
            LongCastlingCommand = new RelayCommand(LongCastling);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Grid[i * 8 + j] = new BoardField(this)
                    {
                        Row = i,
                        Column = j
                    };
                }
            }

            for (int i = 8; i<16; i++)
            {
                Grid[i].Occupant = new Pawn(PartyEnum.Black);
                Grid[i+8*5].Occupant = new Pawn(PartyEnum.White);
            }
            Grid[0].Occupant = new Rook(PartyEnum.Black);
            Grid[7].Occupant = new Rook(PartyEnum.Black);
            Grid[56].Occupant = new Rook(PartyEnum.White);
            Grid[63].Occupant = new Rook(PartyEnum.White);
            Grid[1].Occupant = new Knight(PartyEnum.Black);
            Grid[6].Occupant = new Knight(PartyEnum.Black);
            Grid[57].Occupant = new Knight(PartyEnum.White);
            Grid[62].Occupant = new Knight(PartyEnum.White);

            Grid[2].Occupant = new Bishop(PartyEnum.Black);
            Grid[5].Occupant = new Bishop(PartyEnum.Black);
            Grid[58].Occupant = new Bishop(PartyEnum.White);
            Grid[61].Occupant = new Bishop(PartyEnum.White);

            Grid[3].Occupant = new Queen(PartyEnum.Black);
            Grid[59].Occupant = new Queen(PartyEnum.White);

            Grid[4].Occupant = new King(PartyEnum.Black);
            Grid[60].Occupant = new King(PartyEnum.Black);
            Grid[60].Occupant = new King(PartyEnum.White);
        }
        public static void SaveState(PartyEnum Turn, string status)
        {
            BoardField[] toSave = new BoardField[64];
            for (int i = 0; i < toSave.Length; i++)
            {
                toSave[i] = (BoardField)Grid[i].Clone();
            }
            History.Push(toSave);
            SideTurnHistory.Push(Turn);
            StatusHistory.Push(status);
        }

        public ICommand StepBackCommand { get; }
        public ICommand ShortCastlingCommand { get; }
        public ICommand LongCastlingCommand { get; }

        private void StepBackAboba()
        {
            StepBack(true);
        }
        private void ShortCastling()
        {
            SaveState(Turn, Status);
            if(Turn== PartyEnum.Black)
            {
                Grid[6].Occupant = Grid[4].Occupant;
                Grid[4].Occupant = null;
                Grid[5].Occupant = Grid[7].Occupant;
                Grid[7].Occupant = null;
                Turn = PartyEnum.White;
            }
            else
            {
                Grid[62].Occupant = Grid[60].Occupant;
                Grid[60].Occupant = null;
                Grid[61].Occupant = Grid[63].Occupant;
                Grid[63].Occupant = null;
                Turn = PartyEnum.Black;
            }
            ShortCastingAvailable = false;
            CastlingCheck();

        }
        private void LongCastling()
        {
            SaveState(Turn, Status);
            if (Turn == PartyEnum.Black)
            {
                Grid[2].Occupant = Grid[4].Occupant;
                Grid[4].Occupant = null;
                Grid[3].Occupant = Grid[0].Occupant;
                Grid[0].Occupant = null;
                Turn = PartyEnum.White;
            }
            else
            {
                Grid[58].Occupant = Grid[60].Occupant;
                Grid[60].Occupant = null;
                Grid[59].Occupant = Grid[56].Occupant;
                Grid[56].Occupant = null;
                Turn = PartyEnum.Black;
            }
            LongCastingAvailable = false;
            CastlingCheck();
        }
        public void StepBack(bool manual = false)
        {
            if(History.Count == 0)
            {
                return;
            }
            BoardField[] newGrid = new BoardField[64];
            BoardField[] oldGrid = History.Pop();
            for (int i = 0;i < newGrid.Length; i++)
            {
                Grid[i].Occupant = oldGrid[i].Occupant;
                if (manual)
                {
                    BoardField.toMove = null;
                    BoardField.fromMove = null;
                    Grid[i].BackgroundColor = (oldGrid[i].Row + oldGrid[i].Column) % 2 == 0 ? "White" : "Gray";
                    PieceSelected = false;
                }
            }
            Turn = SideTurnHistory.Pop();
            Status = StatusHistory.Pop();
            CastlingCheck();
            OnPropertyChanged(nameof(Grid));
        }

        public bool IAmChecked(PartyEnum party)
        {
            int myKingPos = Grid.Where(x=>x.Occupant != null).Where(x => x.Occupant!.Name == PieceNames.King).Where(x => x.Occupant.Party == party).Select(x=>x.Row*8+x.Column).First();
            foreach (BoardField field in Grid.Where(x=> x.Occupant != null).Where(x=> x.Occupant!.Party != party))
            {
                foreach(int i in field.Occupant.GetPossibleMoves(field.Row * 8 + field.Column))
                {
                    if (i == myKingPos)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IAmCheckMated(PartyEnum party)
        {
            foreach(BoardField field in Grid.Where(x=>x.Occupant != null).Where(x=>x.Occupant!.Party == party))
            {
                field.HighlightPossible();//
            }
            if (BoardField.Highlighted.Count == 0)
            {
                return true;
            }
            foreach (BoardField field in Grid)
            {
                field.UnHighlightPossible();
            }
            return false;
        }
        private void CastlingCheck()
        {
            if (Turn == PartyEnum.White)
            {
                ShortCastingAvailable = false;
                LongCastingAvailable = false;
                if (!IAmChecked(PartyEnum.White))
                {
                    if (Grid[60].Occupant?.Untouched == true)
                    {
                        if (Grid[56].Occupant?.Untouched == true)
                        {
                            if (Grid[57].Occupant == null && Grid[58].Occupant == null && Grid[59].Occupant == null)
                            {
                                Grid[58].Occupant = Grid[60].Occupant;
                                Grid[60].Occupant = null;
                                if (!IAmChecked(PartyEnum.White))
                                {
                                    LongCastingAvailable = true;
                                }
                                else
                                {
                                    LongCastingAvailable = false;
                                }
                                Grid[60].Occupant = Grid[58].Occupant;
                                Grid[58].Occupant = null;
                            }
                            else LongCastingAvailable = false ;
                        }
                        else LongCastingAvailable = false;
                        if (Grid[63].Occupant?.Untouched == true)
                        {
                            if (Grid[61].Occupant == null && Grid[62].Occupant == null)
                            {
                                Grid[62].Occupant = Grid[60].Occupant;
                                Grid[60].Occupant= null;
                                if (!IAmChecked(PartyEnum.White))
                                {
                                    ShortCastingAvailable = true;
                                }
                                else
                                {
                                    ShortCastingAvailable = false;
                                }
                                Grid[60].Occupant = Grid[62].Occupant;
                                Grid[62].Occupant = null;
                            }

                        }
                    }
                }
            }
            
            else
            {
                ShortCastingAvailable = false;
                LongCastingAvailable = false;
                if (!IAmChecked(PartyEnum.Black))
                {
                    if (Grid[4].Occupant?.Untouched == true)
                    {
                        if (Grid[0].Occupant?.Untouched == true)
                        {
                            if (Grid[1].Occupant == null && Grid[2].Occupant == null && Grid[3].Occupant == null)
                            {
                                Grid[2].Occupant = Grid[4].Occupant;
                                Grid[4].Occupant = null;
                                if (!IAmChecked(PartyEnum.Black))
                                {
                                    LongCastingAvailable = true;
                                }
                                else
                                {
                                    LongCastingAvailable = false;
                                }
                                Grid[4].Occupant = Grid[2].Occupant;
                                Grid[2].Occupant = null;
                            }
                        }
                        //Grid[6].Occupant = Grid[4].Occupant;
                        //Grid[4].Occupant = null;
                        //Grid[5].Occupant = Grid[7].Occupant;
                        //Grid[7].Occupant = null;
                        if (Grid[7].Occupant?.Untouched == true)
                        {
                            if (Grid[5].Occupant == null && Grid[6].Occupant == null)
                            {
                                Grid[6].Occupant = Grid[4].Occupant;
                                Grid[4].Occupant = null;
                                if (!IAmChecked(PartyEnum.Black))
                                {
                                    ShortCastingAvailable = true;
                                }
                                else
                                {
                                    ShortCastingAvailable = false;
                                }
                                Grid[4].Occupant = Grid[6].Occupant;
                                Grid[6].Occupant = null;
                            }
                        }
                    }
                }
            }
        }
        public void StateCheck()//invoke after party changed
        {
            CastlingCheck();
            foreach (BoardField field in Grid.Where(x=>x.Occupant != null && x.Occupant is Pawn))
            {
                Pawn pawn = field.Occupant as Pawn;
                if(pawn.Party == PartyEnum.White && field.Row == 0)
                {
                    field.Occupant = new Queen(PartyEnum.White);
                }
                else if (pawn.Party == PartyEnum.Black && field.Row == 7)
                {
                    field.Occupant = new Queen(PartyEnum.Black);
                }
            }
            if (IAmChecked(Turn))
            {
                if (IAmCheckMated(Turn))
                {
                    Status = $"{Turn} {StatusEnum.Checkmated}";
                    //end game
                    return;
                }
                Status = $"{Turn} {StatusEnum.Checked}";
                return;
            }
            Status = string.Empty;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
