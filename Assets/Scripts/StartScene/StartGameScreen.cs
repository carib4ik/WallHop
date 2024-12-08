using Game;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartScene
{
    public class StartGameScreen : MonoBehaviour
    {
        [SerializeField] private ColorProvider _colorProvider;
        [SerializeField] private TextMeshProUGUI _bestScoreLabel;
        
        private void Start()
        {
            // Получаем случайный цвет из провайдера и устанавливаем его в качестве цвета фона камеры.
            var randomColor = _colorProvider.GetRandomColor();
            _colorProvider.CurrentColor = randomColor;
            Camera.main.backgroundColor = randomColor;
            
            // Получаем лучший счет из сохраненных данных и отображаем его на экране.
            var bestScore = PlayerPrefs.GetInt(GlobalConstants.BEST_SCORE_PREFS_KEY, 0);
            _bestScoreLabel.text = $"BEST {bestScore.ToString()}";
        }
        
        public void StartGame()
        {
            // Загружаем сцену игры, чтобы начать новую игру.
            SceneManager.LoadSceneAsync(GlobalConstants.GAME_SCENE);
        }
    }
}