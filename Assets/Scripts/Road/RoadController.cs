using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZigZag {
    /// <summary>
    /// Контроллер дороги
    /// </summary>
    public class RoadController : MonoBehaviour {

        #region Events

        // добавлен один блок из тайлов
        public static event Action<Vector3> OnAddedBlockTales;

        #endregion

        #region Inspector fields

        // один элемент дороги
        [SerializeField] private GameObject prefabCoub = null;
        // ширина дороги
        [SerializeField] private float widthRoad = 3;
        // растояние на которое необходимо созать дорогу
        [SerializeField] private float distanseRoad = 10f;
        // растояние после которого необходимо удалять дорогу
        [SerializeField] private float distanseRemoveRoad = 1f;

        #endregion

        #region Private variables

        private IPoolObject poolObjct = null;
        // смещение блока относительно соседнего
        private float shiftBlock = 0;
        private Transform cachTransfrom = null;
        private List<IRoadPart> roadObjects = new List<IRoadPart>();
        private List<int> tailsInBlock = new List<int>();
        // первый элемент дороги
        private IRoadPart firstRoadPart = null;
        // последний элемент дороги
        private IRoadPart lastRoadPart = null;

        #endregion

        #region Unity events

        private void Awake () {
            cachTransfrom = transform;
            GameMaster.OnResetGame += OnResetGame;
        }

        private void OnDestroy () {
            GameMaster.OnResetGame -= OnResetGame;
        }

        private void Start () {
            poolObjct = PoolManager.Instance.GetPoolObject(prefabCoub);
            BoxCollider boxCollider = prefabCoub.GetComponent<BoxCollider>();
            shiftBlock = boxCollider.size.x;
            CreateStartRoad();
        }

        private void Update () {
            RoadGeneration();
        }

        #endregion

        #region Game events

        private void OnResetGame () {
            for (int i = 0; i < roadObjects.Count; i++) {
                roadObjects[i].ReturnToPool();
            }
            roadObjects.Clear();
            firstRoadPart = null;
            lastRoadPart = null;
            tailsInBlock.Clear();
            StartCoroutine(WaitReset());
        }

        #endregion

        #region Road generation 

        private IEnumerator WaitReset () {
            yield return new WaitForEndOfFrame();
            CreateStartRoad();
        }

        // создать стартовую площадку
        private void CreateStartRoad () {
            CreateRoadPart(new Vector3(-shiftBlock, 0, -shiftBlock), 3, true);
            for (int i = 0; i < 40; i++) {
                AddRoadPart();
            }
        }

        // Сгенерировать дорогу
        private void RoadGeneration () {
            if (ChechNeedAddRoad()) {
                AddRoadPart();
            }
            if (ChechNeedRemoveRoad()) {
                RemovePartRoad();
            }
        }

        // нужно ли еще удлинить дорогу
        private bool ChechNeedAddRoad () {
            if (lastRoadPart == null) {
                return false;
            }
            return Vector3.Distance(CameraController.Instance.GetPosition, lastRoadPart.GetPosition) < distanseRoad;
        }
        
        // нужно ли удалить дорогу
        private bool ChechNeedRemoveRoad () {
            if (firstRoadPart == null) {
                return false;
            }
            return Vector3.Distance(CameraController.Instance.GetPosition, firstRoadPart.GetPosition) > distanseRemoveRoad;
        }

        // добавить часть дороги
        private void AddRoadPart () {
            int topDirection = UnityEngine.Random.Range(0, 2);
            Vector3 pos = GetPositionGenerationPart(topDirection == 1);
            CreateRoadPart(pos, widthRoad);
        }

        // получить позицию начала генерации новой части дороги
        private Vector3 GetPositionGenerationPart (bool topDirection) {
            Vector3 pos = lastRoadPart.GetPosition;
            if (topDirection) {
                pos.z += shiftBlock;
                pos.x -= shiftBlock * (widthRoad - 1);
            } else {
                pos.z -= shiftBlock * (widthRoad - 1);
                pos.x += shiftBlock;
            }
            return pos;
        }

        // создать часть дороги
        private void CreateRoadPart (Vector3 pos, float widthRoad, bool startRoad = false) {
            if (poolObjct == null) {
                Debug.LogError("[RoadController].RoadGeneration: poolObjct is null");
                return;
            }
            // количество объектов в блоке
            int countTils = 0;
            IRoadPart poolable = null;
            for (int z = 0; z < widthRoad; z++) {
                for (int x = 0; x < widthRoad; x++) {
                    countTils++;
                    poolable = poolObjct.Pull<IRoadPart>();
                    poolable.GetTransform.SetParent(cachTransfrom);
                    poolable.GetTransform.localPosition = pos;
                    if (firstRoadPart == null) {
                        firstRoadPart = poolable;
                    }
                    roadObjects.Add(poolable);
                    pos.x += shiftBlock;
                }
                pos.z += shiftBlock;
                pos.x -= shiftBlock * widthRoad;
            }
            lastRoadPart = poolable;
            pos = lastRoadPart.GetPosition;
            if (!startRoad) {
                float shift = shiftBlock * ((widthRoad - 1f) / 2f);
                pos.x -= shift;
                pos.z -= shift;
                pos.y += shiftBlock * 1.5f;
                OnAddedBlockTales?.Invoke(pos);
            }
            tailsInBlock.Add(countTils);
        }

        // удалить ненужную часть дороги
        private void RemovePartRoad () {
            int countRemove = 1;
            if (tailsInBlock.Count > 0) {
                countRemove = tailsInBlock[0];
                tailsInBlock.RemoveAt(0);
            }
            for (int i = 0; i < countRemove; i++) {
                roadObjects.RemoveAt(0);
                firstRoadPart.Fall();
                firstRoadPart = roadObjects[0];
            }
            
        }

        #endregion

    }
}