using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using static TicTacToe.Form1;

namespace TicTacToe
{

    internal class MainGame : INotifyPropertyChanged  //refreshed aktuells game sobald event ausgelöst wird
    {
        public bool? CurrentPlayer { get; private set; } = false;
        private bool?[] Field = new bool?[9];
        private GameState state; //Wieso grün?
        public bool? Gewinner = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool?[] GetField() => Field.ToList().ToArray();  //gibt array von nullable bools zurück source: daniel

        public GameState State { get => state; private set { state = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State))); } }
        //name of gibt exakten namen zurück
        public void PlayerTurn(int fieldNr)
        {
            if (State != GameState.WaitPlayer1 && State != GameState.WaitPlayer2) return;
            if (fieldNr < 0 || fieldNr > Field.Length) return;
            if (Field[fieldNr] != null) return;
            Field[fieldNr] = CurrentPlayer;
            CurrentPlayer = !CurrentPlayer;
            State = CheckGame();
            if (State != GameState.WaitPlayer1 && State != GameState.WaitPlayer2) CurrentPlayer = null;
        }

        private GameState CheckGame()
        {
            if ((Field[0] != null || Field[4] != null || Field[8] != null) && Field.Count(a => a != null) >= 5) // Prüfen ob Gewinner möglich
            {

                bool? winner = null;
                if (Field[4] != null) // +
                {
                    if (Field[4] == Field[1] && Field[4] == Field[7]) Gewinner = winner = Field[4];// |
                    else if (Field[4] == Field[0] && Field[4] == Field[8]) Gewinner = winner = Field[4];
                    else if (Field[4] == Field[2] && Field[4] == Field[6]) Gewinner = winner = Field[4]; // /
                    else if (Field[4] == Field[3] && Field[4] == Field[5]) Gewinner = winner = Field[4];  // -

                }
                if (winner == null && Field[0] != null)
                {
                    if (Field[0] == Field[1] && Field[0] == Field[2]) winner = Field[0]; // -
                    else if (Field[0] == Field[3] && Field[0] == Field[6]) winner = Field[0];// |
                }
                if (winner == null && Field[8] != null)
                {
                    if (Field[8] == Field[7] && Field[8] == Field[6]) winner = Field[0]; // -
                    else if (Field[8] == Field[2] && Field[8] == Field[5]) winner = Field[0];// |
                }
                if (winner != null) return winner.Value ? GameState.WinPlayer2 : GameState.WinPlayer1;
            }
            if ((State == GameState.WaitPlayer1 || State == GameState.WaitPlayer2) && !Field.Any(a => a == null)) return GameState.Tie;
            return CurrentPlayer.HasValue ? (CurrentPlayer.Value ? GameState.WaitPlayer2 : GameState.WaitPlayer1) : State;
        }

        public void WhoWins() //HIER WEITER MACHEN!!!!
        {
            if (Gewinner == false)
            {
                
            }
            else if (Gewinner == true)
            {


            }
            else
            {

            }

        }

        public enum GameState
        {
            WaitPlayer1,
            WaitPlayer2,
            WinPlayer1,
            WinPlayer2,
            Tie
        }
    }
}
