using System;
using Game;
using UnityEngine;

namespace Level.Point
{
    public class PointMissedTrigger : MonoBehaviour
    {
        // Событие используется для оповещения о том, что игрок попустил point
        public event Action PointMissed;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PointMissed?.Invoke();
            }
        }
    }
}