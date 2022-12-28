using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp.Interfaces;

namespace WindowsFormsApp
{
    public partial class FormGame : Form, IField, IBomb, IGame
    {
        public static string file = "file.txt";
        public static int number = 1;
        const int max_width = 64;
        const int max_len = 32;

        private StreamWriter sw;
        private Point focus;
        public int width;
        public int length;
        public int bomb;
        private bool symb;

        private bool isGame;

        private bool mouse_letf;
        private bool mouse_right;

        private int[,] arr_bomb = new int[max_width, max_len];
        private int[,] status = new int[max_width, max_len];

        private int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        private int[] dy = { 1, 1, 1, 0, 0, -1, -1, -1 };
        private int[] px = { 1, -1, 0, 0, 1, -1, 1, -1 };
        private int[] py = { 0, 0, 1, -1, 1, 1, -1, -1 };

        public bool IsGame { get => this.isGame; set => this.isGame = value; }
        public bool Mouse_letf { get => this.mouse_letf; set => this.mouse_letf=value; }
        public bool Mouse_right { get => this.mouse_right; set => this.mouse_right=value; }
        public bool Symb { get => this.symb; set => this.symb=value; }
        public int[,] Status { get => this.status; set => this.status=value; }
        public int Bomb { get => this.bomb; set => this.bomb=value; }
        public int[,] Arr_bomb { get => this.arr_bomb; set => this.arr_bomb=value; }
        public int LengthField { get => this.length; set => this.length=value; }
        public int WidthField { get => this.width; set => this.width=value; }
        public int[] Dx { get => this.dx; set => this.dx=value; }
        public int[] Dy { get => this.dy; set => this.dy=value; }
        public int[] Px { get => this.px; set => this.px=value; }
        public int[] Py { get => this.py; set => this.py=value; }

        public FormGame()
        {
            InitializeComponent();

            this.WidthField = Properties.Settings.Default.Width;
            this.LengthField = Properties.Settings.Default.Height;
            this.Bomb = Properties.Settings.Default.BombCnt;

            this.Symb = Properties.Settings.Default.Flag;

            labelTime.Font = new Font("Arial", 20);
            labelBom.Font = new Font("Arial", 20);

            DimensionsUp();
            Choose();
            timer1.Interval = 1000;
            try
            {
                sw = new StreamWriter(file, append: true);
            }
            catch (Exception e)
            {
                MessageBox.Show("Виникла помилка: " + e.Message);
            }

        }


        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Array.Clear(Arr_bomb, 0, Arr_bomb.Length);
            Array.Clear(Status, 0, Status.Length);
            SetFocus();
            Random Rand = new Random();
            for (int i = 1; i <= Bomb;)
            {
                int x = Rand.Next(WidthField) + 1;
                int y = Rand.Next(LengthField) + 1;
                if (Arr_bomb[x, y] != -1)
                {
                    Arr_bomb[x, y] = -1; i++;
                }
            }
            for (int i = 1; i <= WidthField; i++)
            {
                for (int j = 1; j <= LengthField; j++)
                {
                    if (Arr_bomb[i, j] != -1)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            if (Arr_bomb[i + Dx[k], j + Dy[k]] == -1)
                            {
                                Arr_bomb[i, j]++;
                            }
                        }
                    }
                }
            }
            labelBom.Text = Bomb.ToString();
            labelTime.Text = "0";
            timer1.Enabled = true;
            IsGame = false;
        }

        public void SetBomb() 
        {
            this.Bomb = 10;
        }

        public void SetBomb(int width)
        {
            this.Bomb = width * width / 5;
        }
        public void SetBomb(int width, int length)
        {
            this.Bomb = width * length / 5;
        }

        private void initialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WidthField = 10;
            LengthField = 10;
            SetBomb();
            Choose();
            DimensionsUp();
        }

        private void complexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WidthField = 30;
            LengthField = 16;
            SetBomb(WidthField, LengthField);
            Choose();
            DimensionsUp();
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WidthField = 16;
            LengthField = 16;
            Bomb = 40;
            SetBomb(WidthField);
            Choose();
            DimensionsUp();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSetting formSetting = new FormSetting(this);
            formSetting.ShowDialog();
            DimensionsUp();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sw.Close();
            FormAbout formAbout = new FormAbout();

            formAbout.ShowDialog();
            sw = new StreamWriter(file, append: true);
        }


        public void Choose()
        {
            if (WidthField == 10 && LengthField == 10 && Bomb == 10)
            {
                initialToolStripMenuItem.Checked = true;
                mediumToolStripMenuItem.Checked = false;
                complexToolStripMenuItem.Checked = false;
                settingToolStripMenuItem.Checked = false;
            }
            else if (WidthField == 16 && LengthField == 16 && Bomb == 40)
            {
                initialToolStripMenuItem.Checked = false;
                mediumToolStripMenuItem.Checked = true;
                complexToolStripMenuItem.Checked = false;
                settingToolStripMenuItem.Checked = false;
            }
            else if (WidthField == 30 && LengthField == 16 && Bomb == 99)
            {
                initialToolStripMenuItem.Checked = false;
                mediumToolStripMenuItem.Checked = false;
                complexToolStripMenuItem.Checked = true;
                settingToolStripMenuItem.Checked = false;
            }
            else
            {
                initialToolStripMenuItem.Checked = false;
                mediumToolStripMenuItem.Checked = false;
                complexToolStripMenuItem.Checked = false;
                settingToolStripMenuItem.Checked = true;
            }
        }


        private void FormGame_Paint(object sender, PaintEventArgs e)
        {
            PaintGame(e.Graphics);
        }

        public void PaintGame(Graphics graphic)
        {
            graphic.Clear(Color.White);
            int x = 6;
            int y = 6 + menuStrip1.Height;
            for (int row = 1; row <= WidthField; row++)
            {
                for (int col = 1; col <= LengthField; col++)
                {
                    if (Status[row, col] != 1)
                    {
                        if (row == focus.X && col == focus.Y)
                        {
                            SolidBrush brush  = new SolidBrush(Color.FromArgb(100, Color.DarkViolet));
                            Rectangle rectang = new Rectangle(x + 34 * (row - 1) + 1, y + 34 * (col - 1) + 1, 32, 32);
                            graphic.FillRectangle(brush, rectang);
                        }
                        else
                        {
                            Rectangle rectang = new Rectangle(x + 34 * (row - 1) + 1, y + 34 * (col - 1) + 1, 32, 32);
                            graphic.FillRectangle(Brushes.DarkViolet, rectang);
                        }
                        if (Status[row, col] == 2)
                        {
                            graphic.DrawImage(Resource.Resource.flag_pic, x + 34 * (row - 1) + 1 + 4, y + 34 * (col - 1) + 1 + 2);    
                        }
                    }
                    else if (Status[row, col] == 1)
                    {
                        if (focus.X == row && focus.Y == col)
                        {
                            graphic.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.LightGray)), new Rectangle(x + 34 * (row - 1) + 1, y + 34 * (col - 1) + 1, 32, 32));
                        }
                        else
                        {
                            graphic.FillRectangle(Brushes.LightGray, new Rectangle(x + 34 * (row - 1) + 1, y + 34 * (col - 1) + 1, 32, 32));
                        }
                        if (Arr_bomb[row, col] > 0)
                        {
                            Brush DrawBrush = new SolidBrush(Color.Blue);
                            if (Arr_bomb[row, col] == 2)  
                                DrawBrush = new SolidBrush(Color.Green);  
                            if (Arr_bomb[row, col] == 3)  
                                DrawBrush = new SolidBrush(Color.Red);  
                            if (Arr_bomb[row, col] == 4)  
                                DrawBrush = new SolidBrush(Color.DarkBlue); 
                            if (Arr_bomb[row, col] == 5)  
                                DrawBrush = new SolidBrush(Color.DarkRed);  
                            if (Arr_bomb[row, col] == 6)  
                                DrawBrush = new SolidBrush(Color.DarkSeaGreen);  
                            if (Arr_bomb[row, col] == 7)  
                                DrawBrush = new SolidBrush(Color.Black);  
                            if (Arr_bomb[row, col] == 8)  
                                DrawBrush = new SolidBrush(Color.DarkGray); 
                            SizeF Size = graphic.MeasureString(Arr_bomb[row, col].ToString(), new Font("Consolas", 16));
                            Font font = new Font("Consolas", 16);
                            graphic.DrawString(Arr_bomb[row, col].ToString(), font, DrawBrush, x + 34 * (row - 1) + 1 + (32 - Size.Width) / 2, y + 34 * (col - 1) + 1 + (32 - Size.Height) / 2);
                        }
                        if (Arr_bomb[row, col] == -1)
                        {
                            graphic.DrawImage(Resource.Resource.bomb, x + 34 * (row - 1) + 1 + 4, y + 34 * (col - 1) + 1 + 2);
                        }
                    }
                }
            }
        }
        public void DimensionsUp()
        {
            int x = this.Width - this.ClientSize.Width;
            int y = this.Height - this.ClientSize.Height;
            int add_y = menuStrip1.Height + tableLayoutPanel1.Height;
            this.Width = 12 + 34 * WidthField + x;
            this.Height = 12 + 34 * LengthField + add_y + y;
            tableLayoutPanel1.Location = new Point(0, this.Height - 50);
            newGameToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormGame_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < 6 || e.X > 6 + WidthField * 34 ||
                e.Y < 6 + menuStrip1.Height ||
                e.Y > 6 + menuStrip1.Height + LengthField * 34)
            {
                SetFocus();
            }
            else
            {
                int x = (e.X - 6) / 34 + 1;
                int y = (e.Y - menuStrip1.Height - 6) / 34 + 1;
                SetFocus(x, y);
            }
            this.Refresh();
        }

        public void SetFocus() 
        {
            focus.X = 0; 
            focus.Y = 0;
        }

        public void SetFocus(int x, int y)
        {
            focus.X = x;
            focus.Y = y;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = Convert.ToString(Convert.ToInt32(labelTime.Text) + 1);
        }

        private void FormGame_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Mouse_letf = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                Mouse_right = true;
            }
        }

        private void FormGame_MouseUp(object sender, MouseEventArgs e)
        {
            if ((focus.X == 0 && focus.Y == 0) || isGame)
            {
                return;
            }

            if (Mouse_letf && Mouse_right)
            {
                if (Status[focus.X, focus.Y] == 1 && Arr_bomb[focus.X, focus.Y] > 0)
                {
                    int flag = 0, syst = Arr_bomb[focus.X, focus.Y];
                    for (int i = 0; i < 8; i++)
                    {
                        int x = focus.X + dx[i];
                        int y = focus.Y + dy[i];
                        if (Status[x, y] == 2)
                        {
                            flag++;
                        }
                        
                        if (flag == syst || flag == syst)
                        {
                            bool isFlag = BombOpen();
                            if (!isFlag)
                            {
                                GameLost();
                            }
                        }
                    }
                }
            }
            else if (Mouse_letf)
            {
                if (Arr_bomb[focus.X, focus.Y] != -1)
                {
                    if (Status[focus.X, focus.Y] == 0)
                    {
                        play(focus.X, focus.Y);
                    }
                }
                else
                {
                    GameLost();
                }
            }
            else if (Mouse_right)
            {
                if (Symb)
                {
                    if (Status[focus.X, focus.Y] == 0)
                    {
                        if (Convert.ToInt32(labelBom.Text) > 0)
                        {
                            Status[focus.X, focus.Y] = 2;
                            labelBom.Text = Convert.ToString(Convert.ToInt32(labelBom.Text) - 1);
                        }
                    }
                    else if (Status[focus.X, focus.Y] == 2)
                    {
                        Status[focus.X, focus.Y] = 3;
                        labelBom.Text = Convert.ToString(Convert.ToInt32(labelBom.Text) + 1);
                    }
                    else if (Status[focus.X, focus.Y] == 3)
                    {
                        Status[focus.X, focus.Y] = 0;
                    }
                }
            }
            this.Refresh();
            GameWin();
            Mouse_letf = false;
            Mouse_right = false;
        }

        public void GameWin()
        {
            int nCnt = 0;
            for (int i = 1; i <= WidthField; i++)
            {
                for (int j = 1; j <= LengthField; j++)
                {
                    if (Status[i, j] == 0 || Status[i, j] == 2 || Status[i, j] == 3)
                    {
                        nCnt++;
                    }
                }
            }
            if (nCnt == Bomb)
            {
                timer1.Enabled = false;
                sw.WriteLine("Номер гри: " + number.ToString() + ". Результат: виграш!");
                number++;
                MessageBox.Show(String.Format("Час：{0} ", labelTime.Text), "Вийти", MessageBoxButtons.OK);
                if (WidthField == 10 && LengthField == 10)
                {
                    if (Properties.Settings.Default.Initial > Convert.ToInt32(labelTime.Text))
                    {
                        Properties.Settings.Default.Initial = Convert.ToInt32(labelTime.Text);
                        Properties.Settings.Default.Save();
                    }
                }
                else if (WidthField == 16 && LengthField == 16)
                {
                    if (Properties.Settings.Default.Medium > Convert.ToInt32(labelTime.Text))
                    {
                        Properties.Settings.Default.Medium = Convert.ToInt32(labelTime.Text);
                        Properties.Settings.Default.Save();
                    }
                }
                else if (WidthField == 30 && LengthField == 16)
                {
                    if (Properties.Settings.Default.Complex > Convert.ToInt32(labelTime.Text))
                    {
                        Properties.Settings.Default.Complex = Convert.ToInt32(labelTime.Text);
                        Properties.Settings.Default.Save();
                    }
                }
                IsGame = true;
            }
        }

        public void play(int coor_x, int coor_y)
        {
            Status[coor_x, coor_y] = 1;
            for (int i = 0; i < 8; i++)
            {
                int x = coor_x + Px[i];
                int y = coor_y + Py[i];
                if (x >= 1 && x <= WidthField && y >= 1 && y <= LengthField &&
                    Arr_bomb[x, y] != -1 && Arr_bomb[coor_x, coor_y] == 0 &&
                    (Status[x, y] == 0 || Status[x, y] == 3))
                {
                    play(x, y);
                }
            }
        }

        public bool BombOpen()
        {
            bool bFlag = true;
            for (int i = 0; i < 8; i++)
            {
                int x = focus.X + dx[i];
                int y = focus.Y + dy[i];
                if (Status[x, y] == 0)
                {
                    Status[x, y] = 1;
                    if (Arr_bomb[x, y] != -1)
                    {
                        play(x, y);
                    }
                    else
                    {
                        bFlag = false;
                        break;
                    }
                }
            }
            return bFlag;
        }

        public void GameLost()
        {
            sw.WriteLine("Номер гри: " + number.ToString() + ". Результат: програш!");
            number++;

            for (int i = 1; i <= WidthField; i++)
            {
                for (int j = 1; j <= LengthField; j++)
                {
                    if (Arr_bomb[i, j] == -1 && (Status[i, j] == 0 || Status[i, j] == 3))
                    {
                        Status[i, j] = 1;
                    }
                }
            }

            timer1.Enabled = false;
            IsGame = true;
        }

        private void FormGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            sw.Close();
            File.WriteAllText(file, "");
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
