using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Пулл объектов
    /// </summary>
    public class PoolManager : MonoBehaviour {

        #region Public variables

        public static PoolManager Instance { get; private set; }

        #endregion

        #region Private variables

        // связи GameObect (prefab) и poolObject
        private Dictionary<IPoolable, IPoolObject> prefab2pool = new Dictionary<IPoolable, IPoolObject>();
        private Transform cachTransform = null;

        #endregion

        #region Unity events

        private void Awake () {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            cachTransform = transform;
        }

        #endregion

        #region Logic

        // получить пулл объектов
        public IPoolObject GetPoolObject (GameObject prefab) {
            IPoolable poolable = prefab.GetComponent<IPoolable>();
            if (poolable == null) {
                Debug.LogError("[PoolManager].GetPool: Not found IPoolable interface.");
                return null;
            }
            IPoolObject poolObject = null;
            if (!prefab2pool.TryGetValue(poolable, out poolObject)) {
                poolObject = CreatePoolObject(poolable);
                poolObject.GetTransform.gameObject.SetActive(false);
                prefab2pool.Add(poolable, poolObject);
            }
            return poolObject;
        }

        // создать пулл объектов
        private IPoolObject CreatePoolObject (IPoolable poolable) {
            GameObject go = new GameObject(poolable.GetGameObject.name);
            IPoolObject poolObject = go.AddComponent<PoolOject>();
            poolObject.Initialization(cachTransform, poolable);
            return poolObject;
        }

        #endregion
    }
}