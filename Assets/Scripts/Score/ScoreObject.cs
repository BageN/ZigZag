using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Подбираемый объект
    /// </summary>
    public class ScoreObject : Poolable, IScoreObject {

        #region Private variables

        private IScoreManager scoreManager = null;

        #endregion

        #region Implementation IScoreObject

        public void SetScoreManager (IScoreManager scoreManager) {
            this.scoreManager = scoreManager;
        }

        #endregion

        #region Logic

        private void OnTriggerEnter (Collider other) {
            scoreManager.Collect(this);
        }

        #endregion
    }
}
