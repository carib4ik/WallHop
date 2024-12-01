using UnityEngine;

namespace Level
{
    public class LevelMover : MonoBehaviour
    {
        [SerializeField] private float _floorSpeed;
    
        private void Update()
        {
            // Перемещаем объект влево
            transform.Translate(-Time.deltaTime * _floorSpeed, 0, 0);
        }
    }
}