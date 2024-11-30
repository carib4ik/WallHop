using Game;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _jumpForce = 6;
        [SerializeField] private int _maxJumpCount = 2;
        [SerializeField] private AudioSource _moveAudio;
        [SerializeField] private AudioSource _landingAudio;

        private int _jumpCount;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _jumpCount = 0;
        }
    
        private void Update()
        {
            var isJumpRequested = CheckJumpInput();

            if (isJumpRequested && CanJump())
            {
                Jump();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag(GlobalConstants.FLOOR_TAG))
            {
                _landingAudio.Play();
                _jumpCount = _maxJumpCount;
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
    }
}
