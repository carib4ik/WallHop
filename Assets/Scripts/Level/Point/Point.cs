using System;
using Game;
using UnityEngine;

namespace Level.Point
{
    public class Point : MonoBehaviour
    {
        // Событие, возникающее при сборе поинта игроком.
        public event Action<Point> PointCollected;
        // Событие, возникающее, если игрок пропустил поинт.
        public event Action<Point> PointMissed;

        [SerializeField] private PointMissedTrigger _pointMissedTrigger;

        // Награда за точку
        public int Reward { get; set; }
        
        private void Start()
        {
            _pointMissedTrigger.PointMissed += OnPointMissed;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PointCollected?.Invoke(this);
            }
        }
        
        private void OnPointMissed()
        {
            PointMissed?.Invoke(this);
        }
        
        private void OnDestroy()
        {
            _pointMissedTrigger.PointMissed -= OnPointMissed;
        }
        
    }
}