using UnityEngine;

namespace _Data.Scripts
{
    public class CamController : MonoBehaviour
    {
        [SerializeField] private EnvController envController;
        [SerializeField] private Transform player;

        private Vector3 _playerPos;

        private void Start()
        {
            Move();
        }

        private void FixedUpdate()
        {
            if (!envController.IsMoving)
            {
                Move();
            }
            else
            {
                MoveToBall(envController.Ball);
            }
        }

        void Move()
        {
            _playerPos = player.position;
            transform.position = Vector3.Lerp(transform.position, _playerPos, 0.5f);
        }

        void MoveToBall(Transform target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
        }
    }
}