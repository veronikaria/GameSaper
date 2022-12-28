namespace WindowsFormsApp.Interfaces
{
    interface IField
    {
        int WidthField { get; set; }
        int LengthField { get; set; }
        int[] Dx { get; set; }
        int[] Dy { get; set; }
        int[] Px { get; set; }
        int[] Py { get; set; }

        void SetFocus();
        void SetFocus(int x, int y);
    }   
}
