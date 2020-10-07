using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Контроллер камеры
    /// </summary>
    public class CameraController : MonoBehaviour {

        #region Inspector fields

        [SerializeField] private Transform playerTransform = null;

        #endregion

        #region Public variables

        public static CameraController Instance { get; private set; }

        #endregion

        #region Private variables 

        private Transform cachTransform = null;
        private bool active = false;

        #endregion

        #region Public fields

        public Vector3 GetPosition {
            get {
                return cachTransform.position;
            }
        }

        #endregion

        #region Unity events

        private void Awake () {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            GameMaster.OnGameStatus += OnGameStatus;
            GameMaster.OnResetGame += OnResetGame;
            Instance = this;
            cachTransform = transform;
        }
        
        private void OnDestroy () {
            GameMaster.OnGameStatus -= OnGameStatus;
            GameMaster.OnResetGame -= OnResetGame;
        }

        private void LateUpdate () {
            if (!active) {
                return;
            }
            Vector3 pos = new Vector3(Mathf.Lerp(cachTransform.position.x, playerTransform.position.x, Time.deltaTime * 5f), cachTransform.position.y, playerTransform.position.z);
            cachTransform.position = pos;
        }

        #endregion

        #region Game events

        private void OnGameStatus (GameMaster.State state) {
            this.active = state == GameMaster.State.Game;
        }

        private void OnResetGame () {
            StartCoroutine(WaitReset());
        }

        #endregion

        #region Logic

        private IEnumerator WaitReset () {
            yield return new WaitForEndOfFrame();
            Vector3 pos = new Vector3(playerTransform.position.x, cachTransform.position.y, playerTransform.position.z);
            cachTransform.position = pos;
        }

        #endregion

    }
}
