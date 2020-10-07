using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Клонированный объект пула
    /// </summary>
    public class Poolable : MonoBehaviour, IPoolable {

        #region Inspector fields
        
        // количество клонов при старте игры 
        [SerializeField] private uint initializationQuantity = 1;

        #endregion

        #region Private variables

        protected Transform cachTransfrom = null;
        private IPoolObject poolObject = null;

        #endregion

        #region IPoolable implementation

        public Transform GetTransform {
            get {
                if (cachTransfrom == null) {
                    cachTransfrom = transform;
                }
                return cachTransfrom;
            }
        }

        public GameObject GetGameObject {
            get {
                return gameObject;
            }
        }

        public uint GetInitializationQuantity {
            get {
                return initializationQuantity;
            }
        }

        // установить объекту пулл родителя
        public void SetPoolObject (IPoolObject poolObject) {
            this.poolObject = poolObject;
        }

        // клонировать объект
        public IPoolable Clone (IPoolObject poolObject, Transform parentTransform = null) {
            GameObject go = Instantiate(gameObject, parentTransform);
            IPoolable poolable = go.GetComponent<IPoolable>();
            poolable.SetPoolObject(poolObject);
            return poolable;
        }

        // вернуться в пулл
        public void ReturnToPool () {
            poolObject.ReturnToPool(this);
        }

        #endregion
    }
}