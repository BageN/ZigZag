using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZigZag {
    /// <summary>
    /// Управление игрой
    /// </summary>
    public class GameMaster : MonoBehaviour {

        #region Events

        // статус игры
        public static event Action<State> OnGameStatus;
        // сбросить игру
        public static event Action OnResetGame;
        // повернуть игрока
        public static event Action OnTurn;

        #endregion

        #region Private variables

        // состояние игры
        public enum State {
            Ready,
            Game,
            Gameover
        }
        private State state = State.Ready;

        #endregion

        #region Unity events

        private void Awake () {
            InputController.OnClickOnDisplay += OnClickOnDisplay;
            Player.OnFall += OnFall;
        }

        private void OnDestroy () {
            InputController.OnClickOnDisplay -= OnClickOnDisplay;
            Player.OnFall -= OnFall;
        }

        #endregion

        #region Events game

        private void OnClickOnDisplay() {
            switch (state) {
                case State.Ready:
                    state = State.Game;
                    UpdateGameStatus();
                    break;
                case State.Game:
                    OnTurn?.Invoke();
                    break;
                case State.Gameover:
                    state = State.Ready;
                    OnResetGame?.Invoke();
                    break;
            }
        }


        // игрок упал
        private void OnFall() {
            state = State.Gameover;
            UpdateGameStatus();
        }

        #endregion

        #region Logic

        private void UpdateGameStatus () {
            OnGameStatus?.Invoke(state);
        }

        #endregion

    }
}