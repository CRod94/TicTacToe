﻿using System;
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
        private KIPlayer _currentKIPlayer;
        private bool ownPlayer = true;
        private MenuScreen _menuScreen;


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

        public Form1(MenuScreen menu) //konstruktor von Form1
        {
            InitializeComponent(); //Generiert und initialisiert alle visuallen Komponentet wie Buttons, Labels, Textboxen etc.
            _menuScreen = menu;
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
        private void SetButton(Button btn, bool? val)  //methode zum aktualisieren der eigenschaft eines buttons, nimmt einen button und ein bool entgegen
        {
            if (btn.InvokeRequired)   ////prüft ob aktueller thread der UI-Thread ist, gibt true zurück wenn der aufrufende thread nicht der UI-Thread ist
            {
                btn.Invoke(new Action(() => SetButton(btn, val))); //führt methode SetButton mit den Parametern btn, val im UI thread aus
            }
            else //wenn der aktuelle thread der UI-Thread ist wird folgendes ausgeführt
            {
                btn.Enabled = (val == null); //Enabled den button sofern val null ist
                btn.Text = val.HasValue ? (val.Value ? "X" : "O") : ""; //ändert den button text in den vert von vall, bei true "x" bei false "o" und bei null ""
            }
        }

        private void PlayerClickButton(object sender, EventArgs e) //methode zum entgegennehmen des spielerinputs eines clicks auf einen button
        {
            if (CurrentGame.State == WaP(ownPlayer)) //prüft ob der aktuell Zustand des Spiels dem zustand entspricht indem darauf gewartet wird dass ein Spieler seinen zug macht
            {
                if (sender is Button btn) CurrentGame.PlayerTurn(Convert.ToInt32(btn.Tag)); //prüft ob der sender ein button ist und ruft die methode PlayerTurn auf und übergibt ihr den tag des buttons

            }
        }


        private void RestartGameClick(object sender, EventArgs e) //methode für Userinput des clicks auf den RestartButton
        {
            RestartGame(); //Ruft die restart methode auf
        }

        private void RestartGame() //methode zum restart des games
        {
            CurrentGame = new MainGame(); //setzt das aktuelle spiel auf den zustand eines neuen spiels -> neues game
            ownPlayer = !ownPlayer;
            _currentKIPlayer = new KIPlayer(CurrentGame, !ownPlayer, _menuScreen.CheckBoxStatus ? KiMode.Hardcore : KiMode.Random); //erstellt neue instanz der KIPlayer klasse mit folgenden Parametern:
        }    //currentgame: repräsentiert das aktuelle spiel, !ownplayer: invertiert den werd von ownplayer, falls menuScreen.CheckBoxStatus -> true, dann kImode.hardcore, false dann Kimode.random

        private void CurrentGame_PropertyChanged(object sender, PropertyChangedEventArgs e) //Event-Handler für PropertyChanged der MainGame klasse. Wird aufgerufen wenn sich ein Eigenschaftswert in MainGame ändert
        {
            if (InvokeRequired) //prüft ob aktueller thread der UI-Thread ist, gibt true zurück wenn der aufrufende thread nicht der UI-Thread ist
            {
                Invoke(new Action(() => CurrentGame_PropertyChanged(sender, e))); //Invoke wird verwendet um den Event-Handler im Ui-Thread aufzurufen.
                return; //nach dem aufrufen des UI hendlers wird dieser Thread beendet
            }
            if (sender is MainGame game) //prüft ob der sender das MainGame formular ist und gibt ihm die Veriable game
            {
                DisplayGame(game); //ruft die methode DisplayGame auf -> Aktualisiert die anzeige des Spiels basierend auf dem aktuellen Zustand

                if (game.State == WaP(ownPlayer)) //prüft ob Spieler 1 an der Reihe ist
                {
                    lblCPUTurn.Text = ""; //Entfernt den Text aus dem label "lvlCPUTurn"
                    lblPlayerTurn.Text = "-------------------------"; //ändert den Text in dem label "lblPlayerTurn"
                }
                if (game.State == WaP(!ownPlayer)) //prüft ob Spieler 2 an der Reihe ist
                {
                    lblCPUTurn.Text = "-----------------------"; //ändert den Text in dem lavel "lvlPlayerTurn"
                    lblPlayerTurn.Text = ""; //entfernt den text vom label "lblPlayerturn"
                }
                if (game.State == WiP(ownPlayer)) //prüft ob spieler 1 gewonnen hat
                {
                    playerWinCount++; //erhöht die Variable playerwincount um 1 -> sie stellt die anzahl der gewonnen Spiele da
                    lblPlayerWins.Text = $"Player Wins: {playerWinCount}"; //editiert den text auf dem Label welches die anzahl der gewonnen spiele für spieler 1 darstellen soll, nutzt die variable playerwincount

                    Invoke(new Action(() => MessageBox.Show(this, "Player 1 Wins!")));
                    //gbit eine Messagebox aus falls Spieler 1 gewonnen hat und Text der in der MessageBox ausgegeben wird

                    RestartGame(); //ruft die restartGame funktion auf -> startet das spiel neu
                }
                else if (game.State == WiP(!ownPlayer)) //prüft ob spieler 2 gewonnen hat
                {

                    CPUWinCount++; //erhöht die Variable CPUWinCount um 1 -> sie stellt die anzahl der gewonnen Spiele da
                    lblCPUWins.Text = $"CPU Wins: {CPUWinCount}"; //editiert den text auf dem Label welches die anzahl der gewonnen spiele für spieler 1 darstellen soll, nutzt die variable playerwincount

                    MessageBox.Show(this, "Player 2 Wins!"); //gbit eine Messagebox aus falls Spieler 1 gewonnen hat und Text der in der MessageBox ausgegeben wird

                    RestartGame(); //ruft die restartGame funktion auf -> startet das spiel neu
                }
                else if (game.State == MainGame.GameState.Tie)   //überprüft ob das spiel sich in einem Unentschieden/Tie befindet
                {
                    MessageBox.Show(this, "It's a Tie!"); //gibt eine Messagebox aus falls es Unentschieden/Tie vorliegt und welcher Text in der MessageBox ausgegeben wird
                    RestartGame(); //Ruft die RestartGame methode auf, welche das Spiel neu startet bzw zurück setzt
                }
            }
        }
        private void InGameExitClick(object sender, EventArgs e) //buttonclick event for hiding the game and switching to the menu - Source: eigener Code
        {
            _menuScreen.Show(); //show funktion zeigt menu an
            this.Close(); //hide funktion versteckt das aktuelle fenster (this)

        }

        MainGame.GameState WaP(bool player) => (player ? MainGame.GameState.WaitPlayer2 : MainGame.GameState.WaitPlayer1); //beschreibt den zustand in dem auf spieler 2 gewartet wird
        MainGame.GameState WiP(bool player) => (player ? MainGame.GameState.WinPlayer2 : MainGame.GameState.WinPlayer1); //beschreibt den zustand in dem auf spieler 1 gewartet wird
        //und speichert beides als GameState ab, entweder WaP und WiP
    }
}
