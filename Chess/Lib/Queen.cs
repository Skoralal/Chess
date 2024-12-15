using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.ChessPiece;
using System.Xml.Linq;

namespace Chess.Lib
{
    internal class Queen : ChessPiece
    {
        public override int[] GetPossibleMoves(int currPos)
        {
            List<int> result = new List<int>();
            for (int direction = 0; direction < 4; direction++)
            {
                for (int length = 1; length <= 7; length++)
                {
                    if (direction == 0)
                    {
                        if ((currPos - 8 * length + length) >= 0 && (currPos - 8 * length + length) < 64 &&
                            (currPos - 8 * length + length) / 8 == currPos / 8 - length)
                        {
                            if (Board.Grid[currPos - 8 * length + length].Occupant == null)
                            {
                                result.Add(currPos - 8 * length + length);
                            }
                            else if (Board.Grid[currPos - 8 * length + length].Occupant.Party == Party)
                            {
                                break;
                            }
                            else
                            {
                                result.Add(currPos - 8 * length + length);
                                break;
                            }
                        }
                    }
                    if (direction == 1)
                    {
                        if ((currPos + 8 * length + length) >= 0 && (currPos + 8 * length + length) < 64 &&
                            (currPos + 8 * length + length) / 8 == currPos / 8 + length)
                        {
                            if (Board.Grid[currPos + 8 * length + length].Occupant == null)
                            {
                                result.Add(currPos + 8 * length + length);
                            }
                            else if (Board.Grid[currPos + 8 * length + length].Occupant.Party == Party)
                            {
                                break;
                            }
                            else
                            {
                                result.Add(currPos + 8 * length + length);
                                break;
                            }
                        }
                    }

                    if (direction == 2)
                    {
                        if ((currPos + 8 * length - length) >= 0 && (currPos + 8 * length - length) < 64 &&
                            (currPos + 8 * length - length) / 8 == currPos / 8 + length)
                        {
                            if (Board.Grid[currPos + 8 * length - length].Occupant == null)
                            {
                                result.Add(currPos + 8 * length - length);
                            }
                            else if (Board.Grid[currPos + 8 * length - length].Occupant.Party == Party)
                            {
                                break;
                            }
                            else
                            {
                                result.Add(currPos + 8 * length - length);
                                break;
                            }
                        }
                    }
                    if (direction == 3)
                    {
                        if ((currPos - 8 * length - length) >= 0 && (currPos - 8 * length - length) < 64 &&
                            (currPos - 8 * length - length) / 8 == currPos / 8 - length)
                        {
                            if (Board.Grid[currPos - 8 * length - length].Occupant == null)
                            {
                                result.Add(currPos - 8 * length - length);
                            }
                            else if (Board.Grid[currPos - 8 * length - length].Occupant.Party == Party)
                            {
                                break;
                            }
                            else
                            {
                                result.Add(currPos - 8 * length - length);
                                break;
                            }
                        }
                    }

                }
            }
            for (int direction = 0; direction < 4; direction++)
            {
                for (int length = 1; length <= 7; length++)
                {
                    if (direction == 0)
                    {
                        if ((currPos - 8 * length) >= 0 && Board.Grid[currPos - 8 * length].Occupant == null)
                        {
                            result.Add(currPos - 8 * length);
                        }
                        if ((currPos - 8 * length) >= 0 && Board.Grid[currPos - 8 * length].Occupant != null &&
                            Board.Grid[currPos - 8 * length].Occupant.Party != Party)
                        {
                            result.Add(currPos - 8 * length);
                            break;
                        }
                        if ((currPos - 8 * length) >= 0 && (currPos - 8 * length) < 64 && Board.Grid[currPos - 8 * length].Occupant != null &&
                            Board.Grid[currPos - 8 * length].Occupant.Party == Party)
                        {
                            break;
                        }
                    }
                    if (direction == 1)
                    {
                        if ((currPos + length) >= 0 && (currPos + length) / 8 == currPos / 8 && Board.Grid[currPos + length].Occupant == null)
                        {
                            result.Add(currPos + length);
                        }
                        if ((currPos + length) >= 0 && (currPos + length) / 8 == currPos / 8 && Board.Grid[currPos + length].Occupant != null &&
                            Board.Grid[currPos + length].Occupant.Party != Party)
                        {
                            result.Add(currPos + length);
                            break;
                        }
                        if ((currPos + length) >= 0 && (currPos + length) < 64 && Board.Grid[currPos + length].Occupant != null &&
                            Board.Grid[currPos + length].Occupant.Party == Party)
                        {
                            break;
                        }
                    }

                    if (direction == 2)
                    {
                        if ((currPos + 8 * length) >= 0 && (currPos + 8 * length) < 64 && Board.Grid[currPos + 8 * length].Occupant == null)
                        {
                            result.Add(currPos + 8 * length);
                        }
                        if ((currPos + 8 * length) >= 0 && (currPos + 8 * length) < 64 && Board.Grid[currPos + 8 * length].Occupant != null &&
                            Board.Grid[currPos + 8 * length].Occupant.Party != Party)
                        {
                            result.Add(currPos + 8 * length);
                            break;
                        }
                        if ((currPos + 8 * length) >= 0 && (currPos + 8 * length) < 64 && Board.Grid[currPos + 8 * length].Occupant != null &&
                            Board.Grid[currPos + 8 * length].Occupant.Party == Party)
                        {
                            break;
                        }
                    }
                    if (direction == 3)
                    {
                        if ((currPos - length) >= 0 && (currPos - length) / 8 == currPos / 8 && Board.Grid[currPos - length].Occupant == null)
                        {
                            result.Add(currPos - length);
                        }
                        if ((currPos - length) >= 0 && (currPos - length) / 8 == currPos / 8 && Board.Grid[currPos - length].Occupant != null &&
                            Board.Grid[currPos - length].Occupant.Party != Party)
                        {
                            result.Add(currPos - length);
                            break;
                        }
                        if ((currPos - length) >= 0 && (currPos - length) < 64 && Board.Grid[currPos - length].Occupant != null &&
                            Board.Grid[currPos - length].Occupant.Party == Party)
                        {
                            break;
                        }
                    }
                }
            }
            return result.ToArray();
        }
        public Queen(PartyEnum partyEnum)
        {
            Party = partyEnum;
            Name = PieceNames.Queen;
            if (partyEnum == PartyEnum.White)
            {
                FullName = "♕";
            }
            else
            {
                FullName = "♛";
            }
        }
    }
}
