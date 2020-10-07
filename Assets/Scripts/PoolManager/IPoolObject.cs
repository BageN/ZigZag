using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Интерфейс объекта пула
    /// </summary>
    public interface IPoolObject {

        Transform GetTransform { get; }
        void Initialization (Transform parentTransform, IPoolable poolable);
        // получить объект из пула
        IPoolable Pull ();
        // получить объект из пула с определенным типом
        T Pull<T> () where T : class;
        // вернуть объект в пулл
        void ReturnToPool (IPoolable poolable);

    }
}