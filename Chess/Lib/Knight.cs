using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.ChessPiece;
using System.Xml.Linq;

namespace Chess.Lib
{
    internal class Knight : ChessPiece
    {
        public override int[] GetAllMoves(int currPos)
        {
            int[] result = new int[8];
            result[0] = currPos-8*2-1;
            result[1] = currPos-8*2+1;
            result[2] = currPos+2-8;
            result[3] = currPos+2+8;

            result[4] = currPos + 8 * 2 - 1;
            result[5] = currPos + 8 * 2 + 1;
            result[6] = currPos - 2 - 8;
            result[7] = currPos - 2 + 8;
            return result;
        }
        public override int[] GetPossibleMoves(int currPos)
        {
            List<int> result = new List<int>();
            int[] all = GetAllMoves(currPos);
            if (all[0]>=0 && all[0] <64 && all[0] / 8 != currPos / 8 -3 &&
                (Board.Grid[all[0]].Occupant == null|| Board.Grid[all[0]].Occupant.Party != Party))
            {
                result.Add(all[0]);
            }
            if (all[1] >= 0 && all[1] < 64 && all[1] / 8 != currPos / 8 - 1 &&
                (Board.Grid[all[1]].Occupant == null || Board.Grid[all[1]].Occupant.Party != Party))
            {
                result.Add(all[1]);
            }
            if (all[2] >= 0 && all[2] < 64 && all[2] % 8 == currPos % 8 + 2 &&
                (Board.Grid[all[2]].Occupant == null || Board.Grid[all[2]].Occupant.Party != Party))
            {
                result.Add(all[2]);
            }
            if (all[3] >= 0 && all[3] < 64 && all[3] % 8 == currPos % 8 + 2 &&
                (Board.Grid[all[3]].Occupant == null || Board.Grid[all[3]].Occupant.Party != Party))
            {
                result.Add(all[3]);
            }

            if (all[4] >= 0 && all[4] < 64 && all[4] / 8 != currPos / 8 + 1 &&
                (Board.Grid[all[4]].Occupant == null || Board.Grid[all[4]].Occupant.Party != Party))
            {
                result.Add(all[4]);
            }
            if (all[5] >= 0 && all[5] < 64 && all[5] / 8 != currPos / 8 + 3 &&
                (Board.Grid[all[5]].Occupant == null || Board.Grid[all[5]].Occupant.Party != Party))
            {
                result.Add(all[5]);
            }
            if (all[6] >= 0 && all[6] < 64 && all[6] % 8 == currPos % 8 - 2 &&
                (Board.Grid[all[6]].Occupant == null || Board.Grid[all[6]].Occupant.Party != Party))
            {
                result.Add(all[6]);
            }
            if (all[7] >= 0 && all[7] < 64 && all[7] % 8 == currPos % 8 - 2 &&
                (Board.Grid[all[7]].Occupant == null || Board.Grid[all[7]].Occupant.Party != Party))
            {
                result.Add(all[7]);
            }
            return result.ToArray();
        }
        public Knight(PartyEnum partyEnum)
        {
            Party = partyEnum;
            Name = PieceNames.Knight;
            if (partyEnum == PartyEnum.White)
            {
                FullName = "♘";
            }
            else
            {
                FullName = "♞";
            }
        }
    }
}
