using Level;
using Level.Obstacle;
using Level.Point;
using Player;
using UI;
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
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private ScoreView _scoreView;
        
        private void Awake()
        {
            _obstacleController.ObstacleChangedPosition += OnObstacleChangedPosition;
            _pointController.RewardAdded += _scoreController.AddScore;
            _scoreController.ScoreChanged += _scoreView.UpdateScoreLabel;
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
        
        private void OnDestroy()
        {
            _obstacleController.ObstacleChangedPosition -= OnObstacleChangedPosition;
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