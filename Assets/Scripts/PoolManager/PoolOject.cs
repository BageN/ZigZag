using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag {
    /// <summary>
    /// Объект пула
    /// </summary>
    public class PoolOject : MonoBehaviour, IPoolObject {

        #region Private variables

        private Transform cachTransfrom = null;
        // клонируемый префаб
        private IPoolable poolable = null;

        #endregion

        #region Implementation IPoolObject

        public Transform GetTransform {
            get {
                if (cachTransfrom == null) {
                    cachTransfrom = transform;
                }
                return cachTransfrom;
            }
        }

        public void Initialization(Transform parentTransform, IPoolable poolable) {
            GetTransform.SetParent(parentTransform);
            this.poolable = poolable;
            poolable.SetPoolObject(this);
            CreateClones();
        }

        // получить объект из пула
        public IPoolable Pull () {
            IPoolable getPoolable = null;
            if (GetTransform.childCount > 0) {
                getPoolable = GetTransform.GetChild(0).GetComponent<IPoolable>();
                getPoolable.GetTransform.SetParent(null);
            } else {
                getPoolable = poolable.Clone(this);
            }
            return getPoolable;
        }

        // получить объект из пула с определенным типом
        public T Pull<T>() where T: class {
            return Pull().GetGameObject.GetComponent<T>();
        }

        // вернуть объект в пулл
        public void ReturnToPool (IPoolable poolable) {
            poolable.GetTransform.SetParent(cachTransfrom);
        }

        #endregion

        #region Logic

        private void CreateClones () {
            for (uint i = 0; i < poolable.GetInitializationQuantity; i++) {
                poolable.Clone(this, GetTransform);
            }
        }

        #endregion

    }
}