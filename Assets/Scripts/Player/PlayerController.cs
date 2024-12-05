using System;
using Game;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Событие, вызываемое при смерти игрока.
        public event Action PlayerDied;
        
        [SerializeField] private GameObject _playerDiedFxPrefab;
        [SerializeField] private float _jumpForce = 6;
        [SerializeField] private int _maxJumpCount = 2;
        [SerializeField] private AudioSource _moveAudio;
        [SerializeField] private AudioSource _landingAudio;
        
        private EchoEffect _echoEffect;
        private int _jumpCount;
        private Rigidbody2D _rb;
        private bool _isGrounded;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _echoEffect = GetComponent<EchoEffect>();
            _jumpCount = 0;
        }
    
        private void Update()
        {
            var isJumpRequested = CheckJumpInput();

            if (isJumpRequested && CanJump())
            {
                Jump();
                _echoEffect.CanShowEcho(true);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag(GlobalConstants.FLOOR_TAG) && !_isGrounded)
            {
                _landingAudio.Play();
                _jumpCount = _maxJumpCount;
                _isGrounded = true;
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision2D)
        {
            if (collision2D.collider.CompareTag(GlobalConstants.FLOOR_TAG))
            {
                _isGrounded = false;
            }
        }

        private void Jump()
        {
            _moveAudio.Play();
            _rb.linearVelocity = Vector2.up * _jumpForce * _rb.gravityScale;
            _jumpCount--;
        }
    
        private bool CheckJumpInput()
        {
            var isSpaceButton = Input.GetKeyDown(KeyCode.Space);
            var isTouchInput = Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

            return isSpaceButton || isTouchInput;
        }
    
        private bool CanJump()
        {
            return _jumpCount > 0;
        }
        
        public void DestroyPlayer()
        {
            Instantiate(_playerDiedFxPrefab, transform.position, Quaternion.identity);
            PlayerDied?.Invoke();
            Destroy(gameObject);
        }
    }
}
