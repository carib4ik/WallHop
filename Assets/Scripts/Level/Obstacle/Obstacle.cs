using System;
using DG.Tweening;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.Obstacle
{
    public class Obstacle : MonoBehaviour
    {
        public event Action PlayerPassedObstacle;

        [SerializeField] private float _minMoveY;
        [SerializeField] private float _maxMoveY;
        [SerializeField] private float _moveDuration;
        // Поле которое представляет вероятность (от 0 до 1), с которой препятствие будет перемещаться в игре.
        [SerializeField] private float _probabilityOfMoving = 0.3f;
        
        [SerializeField] private ObstacleMoveTrigger _obstacleMoveTrigger;
        [SerializeField] private ObstaclePassedTrigger _obstaclePassedTrigger;

        private SpriteRenderer _sprite;
        
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            
            _obstacleMoveTrigger.PlayerEntered += MoveObstacleWithRandomChance;
            _obstaclePassedTrigger.PlayerPassedObstacle += OnPlayerPassedObstacle;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController playerController))
            {
                playerController.DestroyPlayer();
            }
        }
        
        public void Initialize(float height)
        {
            var newSize = new Vector2(_sprite.size.x, height);
            _sprite.size = newSize;
        }
        
        
        /// <summary>
        /// Перед препятсвием стоит триггер, который вызывает перемещение препятствия по координате по "y" 
        /// </summary>
        private void MoveObstacleWithRandomChance()
        {
            // Генерируем случайную вероятность от о до 1.
            var randomChance = Random.value;

            // Если вероятность меньше или равна заданной, то перемещаем препятствие
            if (randomChance <= _probabilityOfMoving)
            {
                var randomMoveY = Random.Range(_minMoveY, _maxMoveY);
                var nextPosition = transform.position.y + randomMoveY;
                transform.DOMoveY(nextPosition, _moveDuration);
            }
        }
        
        private void OnPlayerPassedObstacle()
        {
            PlayerPassedObstacle?.Invoke();
        }
        
        private void OnDestroy()
        {
            _obstacleMoveTrigger.PlayerEntered -= MoveObstacleWithRandomChance;
            _obstaclePassedTrigger.PlayerPassedObstacle -= OnPlayerPassedObstacle;
        }
    }
}