using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZigZag {
    /// <summary>
    /// Контроллер ввода
    /// </summary>
    public class InputController : MonoBehaviour {

        #region Events

        // тап по дисплею
        public static event Action OnClickOnDisplay;

        #endregion

        #region Game events

        // тап по дисплею
        public void ClickOnDisplay () {
            OnClickOnDisplay?.Invoke();
        }

        #endregion

    }
}