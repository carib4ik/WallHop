using System;
using Game;
using UnityEngine;

namespace Level.Obstacle
{
    public class ObstaclePassedTrigger : MonoBehaviour
    {
        public event Action PlayerPassedObstacle;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PlayerPassedObstacle?.Invoke();
            }
        }
    }
}