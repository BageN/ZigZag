using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZigZag {
    /// <summary>
    /// Объект игрока
    /// </summary>
    public class Player : MonoBehaviour {

        #region Inspector fields

        // скорость движения
        [SerializeField] private float speed = 3f;

        #endregion

        #region Events

        // падение
        public static event Action OnFall;

        #endregion

        #region Private variables

        protected Transform cachTransfrom = null;
        private Vector3 startPosition = Vector3.zero;
        private bool active = false;
        // включено падение
        private bool fall = false;
        // направление
        private enum Direction {
            Right,
            Up
        }
        private Direction direction = Direction.Right;
        Ray ray; 

        #endregion

        #region Unity events

        private void Awake () {
            cachTransfrom = transform;
            startPosition = cachTransfrom.position;
            GameMaster.OnGameStatus += OnGameStatus;
            GameMaster.OnTurn += OnTurn;
            GameMaster.OnResetGame += OnResetGame;
        }

        private void OnDestroy () {
            GameMaster.OnGameStatus -= OnGameStatus;
            GameMaster.OnTurn -= OnTurn;
            GameMaster.OnResetGame -= OnResetGame;
        }

        private void Update () {
            if (active) {
                Move();
                CheckFall();
            } else if (fall) {
                Fall();
                CheckStopFall();
            }
        }

        #endregion

        #region Game events

        private void OnGameStatus(GameMaster.State state) {
            this.active = state == GameMaster.State.Game;
        }

        private void OnTurn () {
            switch (direction) {
                case Direction.Right:
                    direction = Direction.Up;
                    break;
                case Direction.Up:
                    direction = Direction.Right;
                    break;
            }
        }

        private void OnResetGame () {
            fall = false;
            direction = Direction.Right;
            cachTransfrom.position = startPosition;
        }

        #endregion

        #region Logic

        // двигать игрока
        private void Move () {
            Vector3 pos = cachTransfrom.position;
            float speed = this.speed * Time.deltaTime;
            switch (direction) {
                case Direction.Right:
                    pos.x += speed;
                    break;
                case Direction.Up:
                    pos.z += speed;
                    break;
            }
            if (fall) {
                pos.y -= speed * 2.5f;
            }
            cachTransfrom.position = pos;
        }

        // проверка падения
        private void CheckFall () {
            ray = new Ray(cachTransfrom.position, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction * 10f);
            if (!Physics.Raycast(ray, 10f, 1 << 8)) {
                fall = true;
                OnFall?.Invoke();
            }
        }

        // падение
        private void Fall () {
            Move();
        }

        // проверка завершения падения
        private void CheckStopFall () {
            if (Vector3.Distance(CameraController.Instance.GetPosition, cachTransfrom.position) > 20f) {
                fall = false;
            }
        }

        #endregion

    }
}
