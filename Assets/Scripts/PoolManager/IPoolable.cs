using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Интерфейс клонированного объекта пула
    /// </summary>
    public interface IPoolable {


        Transform GetTransform { get; }
        GameObject GetGameObject { get; }
        uint GetInitializationQuantity { get; }
        // установить объекту пулл родителя
        void SetPoolObject (IPoolObject poolObject);
        // клонировать объект
        IPoolable Clone (IPoolObject poolObject, Transform parentTransform = null);
        // вернуться в пулл
        void ReturnToPool ();

    }
}