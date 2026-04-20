using UnityEngine;

namespace _Data.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerIA _playerIa;
        [SerializeField] private EnvController envController;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private SphereCollider playerCollider;
        [SerializeField] private GameObject kickButton;

        [SerializeField] private float moveSpeed = 7f;

        private Vector3 _moveDir;
        private float _moveParam;

        private void Awake()
        {
            _playerIa = new PlayerIA();
            kickButton.SetActive(false);
        }

        private void OnEnable()
        {
            _playerIa.Enable();
        }

        private void OnDisable()
        {
            _playerIa.Disable();
        }

        private void Update()
        {
            Look();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                envController.Ball = other.transform;
                kickButton.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                kickButton.SetActive(false);
            }
        }

        void Move()
        {
            playerAnimator.SetFloat("MoveSpeed", _moveParam);

            var moveDir = _playerIa.Player.Move.ReadValue<Vector2>();
            _moveDir = new Vector3(moveDir.x, 0, moveDir.y).normalized;

            if (_moveDir.magnitude > 0.1f)
            {
                _moveParam = 1;
            }
            else
            {
                _moveParam = 0;
            }

            playerRigidbody.linearVelocity = moveSpeed * _moveDir;
        }

        void Look()
        {
            transform.rotation = Quaternion.LookRotation(_moveDir);
        }
    }
}