using Level;
using Level.Obstacle;
using Level.Point;
using Player;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 1)] private float _pointSpawnProbability = 0.7f;
        [SerializeField] private PointController _pointController;
        [SerializeField] private ObstacleController _obstacleController;
        [SerializeField] private PlayerController _player;
        [SerializeField] private LevelMover _levelMover;
        
        private void Awake()
        {
            _obstacleController.ObstacleChangedPosition += OnObstacleChangedPosition;
            _pointController.RewardAdded += OnRewardAdded;
            _player.PlayerDied += OnPlayerDied;
        }
        
        /// <summary>
        /// Обработчик события изменения позиции препятствий.
        /// </summary>
        private void OnObstacleChangedPosition(Vector3 position)
        {
            // Генерируем случайное число от 0 до 1
            var randomValue = Random.value;
        
            // Проверяем вероятность создания точки
            if (randomValue <= _pointSpawnProbability)
            {
                _pointController.SpawnPoint(position);
            }
        }
        
        /// <summary>
        /// Обработчик события сбора поинтов.
        /// </summary>
        private void OnRewardAdded(int rewardPerPoint)
        {
            Debug.Log(rewardPerPoint);
        }
        
        private void OnDestroy()
        {
            _obstacleController.ObstacleChangedPosition -= OnObstacleChangedPosition;
            _pointController.RewardAdded -= OnRewardAdded;
            _player.PlayerDied -= OnPlayerDied;
        }
        
        private void OnPlayerDied()
        {
            // Отключаем движение уровня и уничтожаем объекты
            _levelMover.enabled = false;
            _obstacleController.DestroyAllObstacles();
            _pointController.DestroyAllPoints();
        }
    }
}