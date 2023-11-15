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
        private int playerWinCount = 0; //integer zur darstellung der Wins des Spielers
        private int CPUWinCount = 0; //integer zur darstellung der Wins der CPU
        private MainGame _currentGame; //dekleration einer Instanz names _currentGame von MainGame (dem eigentlichen Spiel)

        private MainGame CurrentGame //
        {
            get => _currentGame; set //gibt den Wert von _currentgame zurück und weißt _currentgame einen wert zu der im if statement bestimmt wird
            {
                if (_currentGame != null) //prüft ob _current game nicht null ist, falls es nicht Null ist wird der weitere code ausgeführt
                    _currentGame.PropertyChanged -= CurrentGame_PropertyChanged; //der Event "CurrentGame_PropertyChanged" hendler des vorherigen spiels wird entfernt falls _current game nicht null ist
                _currentGame = value; //setzt den neuen wert für _currentGame auf value; 
                _currentGame.PropertyChanged -= CurrentGame_PropertyChanged; //entfernt den alten eventhandler aus propertychanged unabhänig ob es null ist oder nicht
                _currentGame.PropertyChanged += CurrentGame_PropertyChanged; //wenn _currentGame nicht null ist wird der eventhandler propertychanged wieder hinzugefügt
                DisplayGame(_currentGame); //ruft die methode displaygame auf und dient dazu das aktuelle Spiel auf dem Formular anzuzeigen
            }
        }

        public Form1() //konstruktor von Form1
        {
            InitializeComponent(); //Generiert und initialisiert alle visuallen Komponentet wie Buttons, Labels, Textboxen etc.
            RestartGame(); //Ruft die RestarGame() methode auf, dies führt dazu dass bei jeder neuen Instanz des Formulars das spiel neu geladen wird, stellt sicher dass das spiel immer mit einem Frischen spielstand beginnt
        }

        private void DisplayGame(MainGame game)  //Methode zum anzeigen der buttons auf dem Formular basierend auf dem aktuellen Zustand des Tic-Tac-Toe spiels
        {
            var field = game.GetField(); //Ruft die methode GetField der MainGame klasse auf um das aktuelle spielfeld abzurufen.
            foreach (var btn in Controls.OfType<Button>().Where(a => !String.IsNullOrWhiteSpace(a.Tag?.ToString()))) //Geht durch alle "Contorls" vom typ "ofType" button und ein nicht leeres tag haben (!String.IsNullOrWhiteSpace) 
            {
                SetButton(btn, field[Convert.ToInt32(btn.Tag)]); //ruft die methode "SetButton" auf um den Text und den Status der Taste zu aktualisieren 
            }                                                    //[Convert.ToInt32(btn.Tag)] == tag des buttons zu int convertiert um aufs entsprechende feld zugreifen zu können, speichert dann für jeden button der nicht null ist den jeweiligen wert zu, true oder false

        }
        private void SetButton(Button btn, bool? val) //methode zum aktuallisieren einer Schaltfläche (button), basierend auf den übergebenen Werten
        {
            btn.Enabled = (val == null); //Enabled den betroffenen Button wenn val gleich null ist -> wenn das feld also leer ist wird der button enabled ansonsten disabled
            btn.Text = val.HasValue ? (val.Value ? "X" : "O") : ""; //prüft ob Val einen wert besitzt. Falls val.value true ist X falls es false ist 0 und wenn es keinen wert hat nichts
        }
        private void PlayerClickButton(object sender, EventArgs e) //methode zum entgegennehmen des spielerinputs eines clicks auf einen button
        {
            //if (currentGame.CurrentPlayer != ownPlayer) return;
            if (sender is Button btn) CurrentGame.PlayerTurn(Convert.ToInt32(btn.Tag)); //prüft ob der sender ein button ist und ruft die methode PlayerTurn auf und übergibt ihr den tag des buttons
        }

        private void RestartGameClick(object sender, EventArgs e) //methode für Userinput des clicks auf den RestartButton
        {
            RestartGame(); //Ruft die restart methode auf
        }

        private void RestartGame() //methode zum restart des games
        {
            CurrentGame = new MainGame(); //setzt das aktuelle spiel auf den zustand eines neuen spiels -> neues game
        }

        private void CurrentGame_PropertyChanged(object sender, PropertyChangedEventArgs e) //Event-Handler für PropertyChanged der MainGame klasse. Wird aufgerufen wenn sich ein Eigenschaftswert in MainGame ändert
        {
            if (sender is MainGame game) //prüft ob der sender das MainGame formular ist und gibt ihm die Veriable game
            {
                DisplayGame(game); //ruft die methode DisplayGame auf -> Aktualisiert die anzeige des Spiels basierend auf dem aktuellen Zustand

                if (game.State == MainGame.GameState.WinPlayer1) //prüft ob spieler 1 gewonnen hat
                {
                    playerWinCount++; //erhöht die Variable playerwincount um 1 -> sie stellt die anzahl der gewonnen Spiele da
                    lblPlayerWins.Text = $"Player Wins: {playerWinCount}"; //editiert den text auf dem Label welches die anzahl der gewonnen spiele für spieler 1 darstellen soll, nutzt die variable playerwincount

                    MessageBox.Show("Player 1 Wins!"); //gbit eine Messagebox aus falls Spieler 1 gewonnen hat und Text der in der MessageBox ausgegeben wird

                    RestartGame(); //ruft die restartGame funktion auf -> startet das spiel neu
                }
                else if (game.State == MainGame.GameState.WinPlayer2) //prüft ob spieler 2 gewonnen hat
                {

                    CPUWinCount++; //erhöht die Variable CPUWinCount um 1 -> sie stellt die anzahl der gewonnen Spiele da
                    lblCPUWins.Text = $"CPU Wins: {CPUWinCount}"; //editiert den text auf dem Label welches die anzahl der gewonnen spiele für spieler 1 darstellen soll, nutzt die variable playerwincount

                    MessageBox.Show("Player 2 Wins!"); //gbit eine Messagebox aus falls Spieler 1 gewonnen hat und Text der in der MessageBox ausgegeben wird

                    RestartGame(); //ruft die restartGame funktion auf -> startet das spiel neu

                }
            }
        }

        private void InGameExitClick(object sender, EventArgs e) //buttonclick event for hiding the game and switching to the menu - Source: eigener Code
        {
            MenuScreen menu = new MenuScreen();   //deklarierung eines neuen Menuscreen objects
            menu.Show(); //show funktion zeigt menu an
            this.Hide(); //hide funktion versteckt das aktuelle fenster (this)

        }
    }
}
