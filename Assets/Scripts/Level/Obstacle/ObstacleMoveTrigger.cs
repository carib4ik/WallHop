using System;
using Game;
using UnityEngine;

namespace Level.Obstacle
{
    public class ObstacleMoveTrigger : MonoBehaviour
    {
        public event Action PlayerEntered;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PlayerEntered?.Invoke();
            }
        }
    }
}