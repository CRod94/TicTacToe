using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class MenuScreen : Form
    {
        public bool CheckBoxStatus //deklaration einer öffentliches Veriablen die den status der checkbox darstellt und es ermöglicht auf diesen von anderen Forms aus zuzugreifen
        {
            get { return HMCheckBox?.Checked ?? false; } //holt sich den wert der CheckBox HMCheckBox -> der wert ist true oder false
            set
            {
                if (HMCheckBox != null) HMCheckBox.Checked = value; //setzt HMCheckbox.checked auf den aktuellen wert
            }
        }
        public MenuScreen()
        {
            InitializeComponent();
        }
        private void ExitButtonClick(object sender, EventArgs e) //Buttonclickevent for closing the Application - source: eigener code
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e) //Buttonclickevent for starting the Game - source: eigener code
        {
            Form1 game = new Form1(this);
            game.Show();
            this.Hide();
        }
    }
}
