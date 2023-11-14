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
    public partial class Form1 : Form
    {
        public enum Player         //source: eigener code
        {
            X, O
        }

        Player currentPlayer;
        Random random = new Random();
        int playerWinCount = 0;
        int CPUWinCount = 0;
        List<Button> buttons;
        private bool CPUMoves = false;  //prevents multiple inputs from the user while the cpu moves - Source: eigener Code

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void CPUmove(object sender, EventArgs e)
        {
            if (buttons.Count > 0 && CPUMoves)
            {
                int index = random.Next(buttons.Count);     //sets a random enemy field, marks it, disables the button, switches the button color and removes the button from the list
                buttons[index].Enabled = false;             //source: youtube tutorial & eigener code
                currentPlayer = Player.O;
                buttons[index].Text = currentPlayer.ToString();
                buttons[index].BackColor = Color.OrangeRed;
                buttons.RemoveAt(index);
                CheckGame();
                CPUMoves = false;
                CPUTimer.Stop();
            }
        }

        private void PlayerClickButton(object sender, EventArgs e)
        {
            if (!CPUMoves)
            {
                var button = (Button)sender;        //sets a random enemy field, marks it, disables the button, switches the button color and removes the button from the list 
                currentPlayer = Player.X;
                button.Text = currentPlayer.ToString(); //source: youtube tutorial & eigener code
                button.Enabled = false;
                button.BackColor = Color.GreenYellow;
                buttons.Remove(button);
                CheckGame();
                CPUMoves = true;
                CPUTimer.Start();
            }
        }

        private void RestartGameClick(object sender, EventArgs e)
        {
            RestartGame(); //Runs the restart method - source: eigener code
        }

        private void CheckGame()                          //checks if theres 3 in a row, gives the winner credits and disables all buttons
        {                                                 //source: eigener code
            if (button1.Text == "X" && button2.Text == "X" && button3.Text == "X"
                || button4.Text == "X" && button5.Text == "X" && button6.Text == "X"
                || button7.Text == "X" && button8.Text == "X" && button9.Text == "X"
                || button1.Text == "X" && button4.Text == "X" && button7.Text == "X"
                || button2.Text == "X" && button5.Text == "X" && button8.Text == "X"
                || button3.Text == "X" && button6.Text == "X" && button9.Text == "X"
                || button1.Text == "X" && button5.Text == "X" && button9.Text == "X"
                || button3.Text == "X" && button5.Text == "X" && button7.Text == "X")
            {
                CPUTimer.Stop();
                MessageBox.Show("The Human wins against the AI!");
                playerWinCount += 1;
                label1.Text = "Player Wins: " + playerWinCount;
                RestartGame();
            }
            else if (button1.Text == "O" && button2.Text == "O" && button3.Text == "O"
                || button4.Text == "O" && button5.Text == "O" && button6.Text == "O"
                || button7.Text == "O" && button8.Text == "O" && button9.Text == "O"
                || button1.Text == "O" && button4.Text == "O" && button7.Text == "O"
                || button2.Text == "O" && button5.Text == "O" && button8.Text == "O"
                || button3.Text == "O" && button6.Text == "O" && button9.Text == "O"
                || button1.Text == "O" && button5.Text == "O" && button9.Text == "O"
                || button3.Text == "O" && button5.Text == "O" && button7.Text == "O")
            {
                CPUTimer.Stop();
                MessageBox.Show("The AI wins against the Human!");
                CPUWinCount += 1;
                label2.Text = "CPU Wins: " + playerWinCount;
                RestartGame();
            }
        }

        private void RestartGame()
        {
            buttons = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 }; //liste für alle buttons -Source: eigener code

            foreach (Button x in buttons)
            {
                x.Enabled = true;                     //resets all buttons to their default color and stage - Source: eigener Code
                x.Text = "?";
                x.BackColor = DefaultBackColor;


            }
        }

        private void InGameExitClick(object sender, EventArgs e) //buttonclick event for hiding the game and switching to the menu - Source: eigener Code
        {
            MenuScreen menu = new MenuScreen();
            menu.Show();
            this.Hide();
        }
    }
}
