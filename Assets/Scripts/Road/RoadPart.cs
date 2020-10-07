using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Элемент дороги
    /// </summary>
    public class RoadPart : Poolable, IRoadPart {

        #region Private variables

        private bool fall = false;
        private Coroutine coroutine = null;

        #endregion

        #region Implementation IRoadPart

        public Vector3 GetPosition {
            get {
                if (cachTransfrom == null) {
                    cachTransfrom = transform;
                }
                return cachTransfrom.position;
            }
        }

        // упасть и вернуться в пулл
        public void Fall () {
            fall = true;
            coroutine = StartCoroutine(WaitReturnToPool());
        }

        #endregion

        #region Unity events

        private void Awake () {
            GameMaster.OnResetGame += OnResetGame;
        }

        private void OnDestroy () {
            GameMaster.OnResetGame -= OnResetGame;
        }

        private void Update () {
            if (fall) {
                cachTransfrom.position = new Vector3(cachTransfrom.position.x, cachTransfrom.position.y - 6f * Time.deltaTime, cachTransfrom.position.z);
            }
        }

        #endregion

        #region Game events

        private void OnResetGame () {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                fall = false;
                ReturnToPool();
            }
        }

        #endregion

        #region Logic

        private IEnumerator WaitReturnToPool () {
            yield return new WaitForSeconds(3);
            fall = false;
            ReturnToPool();
        }

        #endregion

    }
}