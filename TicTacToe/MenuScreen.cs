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
            Form1 game = new Form1();
            game.Show();
            this.Hide();
        }
    }
}
