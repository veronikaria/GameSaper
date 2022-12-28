using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{

    public partial class FormSetting : Form
    {
        private FormGame Game;
        public FormSetting(FormGame Game)
        {
            InitializeComponent();
            this.Game = Game;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Game.WidthField = Convert.ToInt32(numericUpDownWidth.Value);
            this.Game.LengthField = Convert.ToInt32(numericUpDownHeight.Value);
            this.Game.Bomb = Convert.ToInt32(numericUpDownBomb.Value);
            this.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
