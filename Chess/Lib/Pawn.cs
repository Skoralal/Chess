using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Chess.ChessPiece;

namespace Chess.Lib
{
    internal class Pawn : ChessPiece
    {
        public override int[] GetAllMoves(int currPos)
        {
            int[] result = new int[4];
            if (Party == PartyEnum.White)
            {
                result[0]= currPos - 8;
                result[1]= currPos - 16;
                result[2]= currPos - 7;
                result[3]= currPos - 9;

            }
            if (Party == PartyEnum.Black)
            {
                result[0] = currPos + 8;
                result[1] = currPos + 16;
                result[2] = currPos + 7;
                result[3] = currPos + 9;
            }
            return result;
        }
        public override int[] GetPossibleMoves(int currPos)
        {
            List<int> result = new List<int>();
            int[] all = GetAllMoves(currPos);
            if(Party == PartyEnum.White)
            {
                if (all[1] >= 0 && all[1] < 64 && Untouched && Board.Grid[all[1]].Occupant == null && Board.Grid[all[0]].Occupant == null)
                {
                    result.Add(all[1]);
                }
                if (all[0] >= 0 && all[0] < 64 && Board.Grid[all[0]].Occupant == null)
                {
                    result.Add(all[0]);
                }
                if (all[2] >= 0 && all[2] < 64 && Board.Grid[all[2]].Occupant != null && Board.Grid[all[2]].Occupant.Party != Party && currPos / 8 != all[2] / 8)
                {
                    result.Add(all[2]);

                }
                if (all[3] >= 0 && all[3] < 64 && Board.Grid[all[3]].Occupant != null && currPos % 8 != 0 && Board.Grid[all[3]].Occupant.Party != Party)
                {
                    result.Add(all[3]);
                }
            }
            else
            {
                if (all[1] >= 0 && all[1] < 64 && Untouched && Board.Grid[all[1]].Occupant == null && Board.Grid[all[0]].Occupant == null)
                {
                    result.Add(all[1]);
                }
                if (all[0] >= 0 && all[0] < 64 && Board.Grid[all[0]].Occupant == null)
                {
                    result.Add(all[0]);
                }
                if (all[2] >= 0 && all[2] < 64 && Board.Grid[all[2]].Occupant != null && Board.Grid[all[2]].Occupant.Party != Party && currPos / 8 == all[2] / 8-1)
                {
                    result.Add(all[2]);
                }
                if (all[3] >= 0 && all[3] < 64 && Board.Grid[all[3]].Occupant != null && currPos % 8 != 7 && Board.Grid[all[3]].Occupant.Party != Party)
                {
                    result.Add(all[3]);
                }
            }
            
            return result.ToArray();
        }
        public Pawn(PartyEnum partyEnum)
        {
            Party = partyEnum;
            Name = PieceNames.Pawn;
            if (partyEnum == PartyEnum.White)
            {
                FullName = "♙";
            }
            else
            {
                FullName = "♟︎";
            }
        }
    }
}
