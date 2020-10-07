using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Интерфейс подбираемого объекта
    /// </summary>
    public interface IScoreObject : IPoolable {

        void SetScoreManager (IScoreManager scoreManager);

    }
}
