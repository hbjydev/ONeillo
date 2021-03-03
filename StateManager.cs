using System;

namespace O_Neillo
{
    class StateManager
    {
        public string Serialize(int[] board, int turn, string player1, string player2)
        {
            return $"{player1};{player2};{turn};{string.Join(",", board)}";
        }

        public void SaveFile(System.IO.FileStream fs, string data)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(data);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}
