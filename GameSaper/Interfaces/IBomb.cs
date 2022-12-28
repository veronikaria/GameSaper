
namespace WindowsFormsApp.Interfaces
{
    interface IBomb
    {
        int Bomb { get; set; }
        int[,] Arr_bomb  { get; set; }

        void SetBomb();
        void SetBomb(int w);
        void SetBomb(int w, int l);
        bool BombOpen();
    }
}
