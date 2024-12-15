using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class ChessPiece : INotifyPropertyChanged, ICloneable
    {
        //private int[][] _baseMoves;
        //private int[][]? _possibleMoves;
        private PieceNames _name; // debug
        private bool _isAlive = true;
        private string _icon = "C:\\Users\\Professional\\source\\repos\\Chess\\Chess\\bin\\Debug\\net8.0-windows\\images.png";
        private PartyEnum _party; // debug
        private string _fullName; // debug
        public bool Untouched { get; set; } = true;

        //public int[][] BaseMoves
        //{
        //    get => _baseMoves;
        //    set
        //    {
        //        if (_baseMoves != value)
        //        {
        //            _baseMoves = value;
        //            OnPropertyChanged(nameof(BaseMoves));
        //        }
        //    }
        //}
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }
        //public int[][]? PossibleMoves
        //{
        //    get => _possibleMoves;
        //    set
        //    {
        //        if (_possibleMoves != value)
        //        {
        //            _possibleMoves = value;
        //            OnPropertyChanged(nameof(PossibleMoves));
        //        }
        //    }
        //}

        public PieceNames Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    FullName = $"{Party} {_name}";
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                if (_isAlive != value)
                {
                    _isAlive = value;
                    OnPropertyChanged(nameof(IsAlive));
                }
            }
        }

        public string Icon
        {
            get => _icon;
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        public PartyEnum Party
        {
            get => _party;
            set
            {
                if (_party != value)
                {
                    _party = value;
                    FullName = $"{Party} {_name}";
                    OnPropertyChanged(nameof(Party));
                }
            }
        }

        public enum PieceNames
        {
            Default,
            King,
            Queen,
            Rook,
            Bishop,
            Knight,
            Pawn
        }

        public enum PartyEnum
        {
            White,
            Black
        }

        public override string ToString()
        {
            return $"{Party} {Name}";
        }

        public void Click()// debug
        {
            Name = PieceNames.Queen;
        }
        public void Move()
        {

        }
        public virtual int[] GetAllMoves(int currPos)
        {
            return Array.Empty<int>();
        }
        public virtual int[] GetPossibleMoves(int currPos)
        {
            return Array.Empty<int>();
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            var output = (ChessPiece)Activator.CreateInstance(this.GetType(),args:Party);
            output.Name = Name;
            output.Party = Party;
            output.Untouched = Untouched;
            output.FullName = FullName;
            return output;
        }
    }

}
