
using System.Drawing;

namespace WindowsFormsApp.Interfaces
{
    interface IGame
    {
        bool IsGame { get; set; }
        bool Mouse_letf { get; set; }
        bool Mouse_right { get; set; }
        bool Symb { get; set; }
        int[,] Status { get; set; }
        void Choose();
        void PaintGame(Graphics g);
        void DimensionsUp();
        void GameWin();
        void play(int x, int y);
        void GameLost();
    }
}
