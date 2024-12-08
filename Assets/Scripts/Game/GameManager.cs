using System.Collections;
using Level;
using Level.Obstacle;
using Level.Point;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float _sceneChangeDelay = 1f;
        [SerializeField] [Range(0.1f, 1)] private float _pointSpawnProbability = 0.7f;
        [SerializeField] private PointController _pointController;
        [SerializeField] private ObstacleController _obstacleController;
        [SerializeField] private PlayerController _player;
        [SerializeField] private LevelMover _levelMover;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private BackgroundColorController _backgroundColorController;
        [SerializeField] private int _difficultyIncreasePeriodInPoints = 10;
        
        [Tooltip("Points required to change background color")]
        [SerializeField] private int _colorChangePeriodInPoints = 5;
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            
            _obstacleController.ObstacleChangedPosition += OnObstacleChangedPosition;
            _pointController.RewardAdded += _scoreController.AddScore;
            _scoreController.ScoreChanged += _scoreView.UpdateScoreLabel;
            _scoreController.ScoreChanged += OnScoreChanged;
            _player.PlayerDied += OnPlayerDied;
        }
        
        /// <summary>
        /// Загружает сцену Game Over с небольшой задержкой после смерти игрока,
        /// чтобы успели проиграться анимация и звук смерти игрока
        /// </summary>
        private IEnumerator LoadGameOverSceneWithDelay()
        {
            yield return new WaitForSeconds(_sceneChangeDelay);
            SceneManager.LoadSceneAsync(GlobalConstants.GAME_OVER_SCENE);
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
        /// Обработчик события изменения очков
        /// </summary>
        private void OnScoreChanged(int score)
        {
            _scoreView.UpdateScoreLabel(score);
            
            // При достижении определенного количества очков, меняем цвет фона
            if (score % _colorChangePeriodInPoints == 0)
            {
                _backgroundColorController.ChangeColor();
            }
            
            // При достижении определенного количества очков, меняем скорость уровня
            if (score % _difficultyIncreasePeriodInPoints == 0)
            {
                _levelMover.IncreaseSpeed();
            }
        }
        
        private void OnDestroy()
        {
            _obstacleController.ObstacleChangedPosition -= OnObstacleChangedPosition;
            _player.PlayerDied -= OnPlayerDied;
            _pointController.RewardAdded -= _scoreController.AddScore;
            _scoreController.ScoreChanged -= _scoreView.UpdateScoreLabel;
            _scoreController.ScoreChanged -= OnScoreChanged;
        }
        
        private void OnPlayerDied()
        {
            // Отключаем движение уровня и уничтожаем объекты
            _levelMover.enabled = false;
            _obstacleController.DestroyAllObstacles();
            _pointController.DestroyAllPoints();
            
            StartCoroutine(LoadGameOverSceneWithDelay());
        }
    }
}