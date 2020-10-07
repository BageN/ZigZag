using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZigZag {
    /// <summary>
    /// Контроллер UI
    /// </summary>
    public class UIController : MonoBehaviour {

        #region Inspector fields

        [SerializeField] private GameObject gameoverText = null;
        [SerializeField] private Text textScore = null;

        #endregion

        #region Unity events

        private void Awake () {
            if (gameoverText != null) {
                gameoverText.SetActive(false);
            }
            GameMaster.OnGameStatus += OnGameStatus;
            GameMaster.OnResetGame += OnResetGame;
            ScoreManager.OnUpdateScore += OnUpdateScore;
        }

        private void OnDestroy () {
            GameMaster.OnGameStatus -= OnGameStatus;
            GameMaster.OnResetGame -= OnResetGame;
            ScoreManager.OnUpdateScore -= OnUpdateScore;
        }

        #endregion

        #region Game events

        private void OnGameStatus (GameMaster.State state) {
            if (state == GameMaster.State.Gameover) {
                if (gameoverText != null) {
                    gameoverText.SetActive(true);
                }
            }
        }

        private void OnResetGame () {
            if (gameoverText != null) {
                gameoverText.SetActive(false);
            }
        }

        private void OnUpdateScore(uint score) {
            if (textScore != null) {
                textScore.text = score.ToString();
            }
        }

        #endregion

    }
}