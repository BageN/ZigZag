using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Интерфейс менеджера очков
    /// </summary>
    public interface IScoreManager {

        void Collect (IScoreObject scoreObject);

    }
    
}
