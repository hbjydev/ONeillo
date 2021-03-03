using System;

namespace O_Neillo
{
    public class GameState
    {
        public int Turn = 0;
        public int[] Board;

        public void Start()
        {
            InitBoard();
            Turn = 1;
        }

        public static int GetRow(int index) { return Convert.ToInt32(Math.Floor(Convert.ToDecimal(index) / 8)); }

        private bool CheckIndex(int index) { return Board[index] > 0; }

        private bool CheckRange(int from, int to)
        {
            bool found = false;

            if (from < 0) from = 0;
            if (to > 63) to = 63;

            for (var i = from; i <= to; i++)
            {
                if (found) break;
                found = CheckIndex(i);
            }

            return found;
        }

        public bool ValidateMove(int index)
        {
            var early = CheckRange(index - 9, index - 7);
            var current = CheckRange(index - 1, index + 1);
            var late = CheckRange(index + 7, index + 9);

            // Is this not connected to any existing pieces?
            if (!early && !current && !late) return false;

            // Is the *current* piece in use?
            if (Board[index] != 0) return false;

            // Can the *current* piece be played?
            bool canMove = CheckMoveUp(index) || CheckMoveNW(index) || CheckMoveNE(index) ||
                CheckMoveDown(index) || CheckMoveSW(index) || CheckMoveSE(index) ||
                CheckMoveLeft(index) || CheckMoveRight(index);

            return canMove;
        }

        private bool CheckMoveUp(int index)
        {
            int end = 999;

            for (int i = index; i > 0; i -= 8)
            {
                if (i == index - 8 && Board[i] == Turn) break;
                if (i == index - 8 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private bool CheckMoveNW(int index)
        {
            int end = 999;

            for (int i = index; i > 0; i -= 9)
            {
                if (i == index - 9 && Board[i] == Turn) break;
                if (i == index - 9 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private bool CheckMoveNE(int index)
        {
            int end = 999;

            for (int i = index; i > 0; i -= 7)
            {
                if (i == index - 7 && Board[i] == Turn) break;
                if (i == index - 7 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private bool CheckMoveDown(int index)
        {
            int end = 999;

            for (int i = index; i < 64; i += 8)
            {
                if (i == index + 8 && Board[i] == Turn) break;
                if (i == index + 8 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private bool CheckMoveSW(int index)
        {
            int end = 999;

            for (int i = index; i < 64; i += 7)
            {
                if (i == index + 7 && Board[i] == Turn) break;
                if (i == index + 7 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private bool CheckMoveSE(int index)
        {
            int end = 999;

            for (int i = index; i < 64; i += 9)
            {
                if (i == index + 9 && Board[i] == Turn) break;
                if (i == index + 9 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private bool CheckMoveLeft(int index)
        {
            int end = 999;

            for (int i = index; i > GetRow(index) * 8; i -= 1)
            {
                if (i == index - 1 && Board[i] == Turn) break;
                if (i == index - 1 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private bool CheckMoveRight(int index)
        {
            int end = 999;

            for (int i = index; i < GetRow(index) * 8 + 8; i += 1)
            {
                if (i == index - 1 && Board[i] == Turn) break;
                if (i == index - 1 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            return end != 999;
        }

        private void MakeMoveUp(int index)
        {
            int end = 999;

            for (int i = index; i > 0; i -= 8)
            {
                if (i == index - 8 && Board[i] == Turn) break;
                if (i == index - 8 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i > end; i -= 8)
                {
                    Board[i] = Turn;
                }
            }
        }

        private void MakeMoveNW(int index)
        {
            int end = 999;

            for (int i = index; i > 0; i -= 9)
            {
                if (i == index - 9 && Board[i] == Turn) break;
                if (i == index - 9 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i > end; i -= 9)
                {
                    Board[i] = Turn;
                }
            }
        }

        private void MakeMoveNE(int index)
        {
            int end = 999;

            for (int i = index; i > 0; i -= 7)
            {
                if (i == index - 7 && Board[i] == Turn) break;
                if (i == index - 7 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i > end; i -= 7)
                {
                    Board[i] = Turn;
                }
            }
        }

        private void MakeMoveDown(int index)
        {
            int end = 999;

            for (int i = index; i < 64; i += 8)
            {
                if (i == index + 8 && Board[i] == Turn) break;
                if (i == index + 8 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i < end; i += 8)
                {
                    Board[i] = Turn;
                }
            }
        }

        private void MakeMoveSW(int index)
        {
            int end = 999;

            for (int i = index; i < 64; i += 7)
            {
                if (i == index + 7 && Board[i] == Turn) break;
                if (i == index + 7 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i < end; i += 7)
                {
                    Board[i] = Turn;
                }
            }
        }

        private void MakeMoveSE(int index)
        {
            int end = 999;

            for (int i = index; i < 64; i += 9)
            {
                if (i == index + 9 && Board[i] == Turn) break;
                if (i == index + 9 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i < end; i += 9)
                {
                    Board[i] = Turn;
                }
            }
        }

        private void MakeMoveLeft(int index)
        {
            int end = 999;

            for (int i = index; i > GetRow(index) * 8; i -= 1)
            {
                if (i == index - 1 && Board[i] == Turn) break;
                if (i == index - 1 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i > end; i -= 1)
                {
                    Board[i] = Turn;
                }
            }
        }

        private void MakeMoveRight(int index)
        {
            int end = 999;

            for (int i = index; i < GetRow(index) * 8 + 8; i += 1)
            {
                if (i == index + 1 && Board[i] == Turn) break;
                if (i == index + 1 && Board[i] == 0) break;
                if (Board[i] == Turn) end = i;
            }

            if (end != 999)
            {
                for (int i = index; i < end; i += 1)
                {
                    Board[i] = Turn;
                }
            }
        }

        public void MakeMove(int index)
        {
            // Make all directional moves
            MakeMoveUp(index);
            MakeMoveNW(index);
            MakeMoveNE(index);
            MakeMoveDown(index);
            MakeMoveSW(index);
            MakeMoveSE(index);
            MakeMoveLeft(index);
            MakeMoveRight(index);

            // Turn the current piece
            Board[index] = Turn;

            // Next turn
            Turn = (Turn == 1 ? 2 : 1);
        }

        public void InitBoard()
        {
            var newBoard = new int[64];

            for (var i = 0; i < 64; i++)
            {
                newBoard[i] = 0;
            }

            newBoard[27] = 1;
            newBoard[28] = 2;
            newBoard[35] = 2;
            newBoard[36] = 1;

            Board = newBoard;
        }
    }
}
