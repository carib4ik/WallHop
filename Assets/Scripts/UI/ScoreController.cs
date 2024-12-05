using System;
using Game;
using UnityEngine;

namespace UI
{
    public class ScoreController : MonoBehaviour
    {
        // Событие, срабатывающее при изменении счета.
        public event Action<int> ScoreChanged;

        [SerializeField] private AudioSource _scoreChangeAudio;

        private int _score;
        
        public void AddScore(int score)
        {
            _score += score;
            // Вызываем событие для уведомления о изменении счета.
            ScoreChanged?.Invoke(_score);
            // Проигрываем звук изменения счета
            _scoreChangeAudio.Play();
        }
        
        private void OnDestroy()
        {
            // Записываем текущий счет в данные игры.
            PlayerPrefs.SetInt(GlobalConstants.SCORE_PREFS_KEY, _score);
            // Сохраняем изменения в данных.
            PlayerPrefs.Save();
        }
    }
}