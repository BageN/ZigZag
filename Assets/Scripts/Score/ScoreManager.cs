using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZigZag {
    /// <summary>
    /// Менеджер очков 
    /// </summary>
    public class ScoreManager : MonoBehaviour, IScoreManager {

        #region Events

        // изменилось количество очков
        public static event Action<uint> OnUpdateScore;

        #endregion

        #region Inspector fields

        // один элемент дороги
        [SerializeField] private GameObject prefabScore = null;
        // режим генерации
        [SerializeField] private GenerationMode generationMode = GenerationMode.Random;

        #endregion

        #region Private variables

        private Transform cachTransfrom = null;
        private uint score = 0;
        private IPoolObject poolObjct = null;
        // количество созданных блоков
        private int numberBlocks = -1;
        private enum GenerationMode  {
            Random,
            Steps
        }
        // количество шагов через которое необходимо создать объект
        private int stepsForGeneration = -1;
        // счетчик шагов для генерации с 1 по 5
        private int stepCount = -1;
        private List<IScoreObject> scoreObjects = new List<IScoreObject>();

        #endregion

        #region Unity events

        private void Awake () {
            cachTransfrom = transform;
            GameMaster.OnResetGame += OnResetGame;
            RoadController.OnAddedBlockTales += OnAddedBlockTales;
        }

        private void Start () {
            poolObjct = PoolManager.Instance.GetPoolObject(prefabScore);
        }

        private void OnDestroy () {
            GameMaster.OnResetGame -= OnResetGame;
            RoadController.OnAddedBlockTales -= OnAddedBlockTales;
        }

        #endregion

        #region Game events

        private void OnResetGame () {
            for (int i = 0; i < scoreObjects.Count; i++) {
                scoreObjects[i].ReturnToPool();
            }
            stepCount = -1;
            numberBlocks = -1;
            stepsForGeneration = -1;
            score = 0;
            OnUpdateScore?.Invoke(score);
        }

        private void OnAddedBlockTales (Vector3 pos) {
            UpdateScores(pos);
        }

        #endregion

        #region Logic

        // обновить подбираемые предметы
        private void UpdateScores (Vector3 pos) {
            numberBlocks++;
            if (numberBlocks == 0) {
                GetCountStepsToCreateObject();
            }
            if (numberBlocks == 4) {
                numberBlocks = -1;
            }
            if (stepsForGeneration == 0) {
                SetScoreObject(pos);
            }
            if (stepsForGeneration >= 0) {
                stepsForGeneration--;
            }
        }

        // количество шагов через сколько добавится элемент
        private void GetCountStepsToCreateObject () {
            switch (generationMode) {
                case GenerationMode.Random:
                    stepsForGeneration = UnityEngine.Random.Range(0, 5);
                    break;
                case GenerationMode.Steps:
                    stepCount++;
                    stepsForGeneration = stepCount;
                    if (stepCount > 3) {
                        stepCount = -1;
                    }
                    break;
            }
        }

        // установить объект
        private void SetScoreObject(Vector3 pos) {
            IScoreObject poolable = poolObjct.Pull<IScoreObject>();
            poolable.GetTransform.SetParent(cachTransfrom);
            poolable.GetTransform.localPosition = pos;
            poolable.SetScoreManager(this);
            scoreObjects.Add(poolable);
        }

        #endregion

        #region Implementation IScoreManager

        public void Collect (IScoreObject scoreObject) {
            scoreObjects.Remove(scoreObject);
            scoreObject.ReturnToPool();
            score++; 
            OnUpdateScore?.Invoke(score);
        }

        #endregion

    }
}
