using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Интерфейс элемента дороги
    /// </summary>
    public interface IRoadPart : IPoolable  {
        
        Vector3 GetPosition { get; }
        // упасть и вернуться в пулл
        void Fall ();

    }
}