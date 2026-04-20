using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Data.Scripts
{
    public class EnvController : MonoBehaviour
    {
        [SerializeField] private List<Transform> goals;
        [SerializeField] private List<Transform> balls;
        [SerializeField] private Transform player;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject resetButton;

        private readonly Dictionary<int, Vector3> _ballPositions = new Dictionary<int, Vector3>();
        private readonly Stack<Transform> _ballStack = new Stack<Transform>();

        private Vector3 _playerPosition;
        public Transform Ball { get; set; }
        private Transform _goal;
        private bool _isMoving;
        public bool IsMoving => _isMoving;
        private bool _isSpawn = false;

        private readonly FindClosestService _findClosestService = new();
        private readonly FindFarthestService _farthestService = new();

        private void Awake()
        {
            for (int i = 0; i < balls.Count; i++)
            {
                _ballPositions.Add(i, balls[i].position);
            }

            _playerPosition = player.position;
        }

        public void OnResetBtnClick()
        {
            for (int i = 0; i < _ballStack.Count; i++)
            {
                balls.Add(_ballStack.Pop());
            }

            player.position = _playerPosition;

            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].position = _ballPositions[i];
            }

            Ball = null;
        }

        public void OnKickBtnClick()
        {
            ClosestData closestData = _findClosestService.ClosestGoal(goals, Ball);
            _goal = closestData.Goal;
            _isMoving = true;
            _isSpawn = false;
            StartCoroutine(MoveTo(Ball, _goal));
        }

        public void OnAutoKickBtnClick()
        {
            FarthestData farthestData = _farthestService.FarthestBall(balls, player);
            Ball = farthestData.Ball;
            ClosestData closestData = _findClosestService.ClosestGoal(goals, Ball);
            _goal = closestData.Goal;
            _isMoving = true;
            _isSpawn = false;
            StartCoroutine(MoveTo(Ball, _goal));
        }

        IEnumerator MoveTo(Transform ball, Transform goal)
        {
            resetButton.SetActive(false);
            float dis = Vector3.Distance(ball.transform.position, goal.transform.position);
            while (dis > 0.1f)
            {
                ball.transform.position = Vector3.Lerp(ball.transform.position, goal.transform.position, 0.3f);
                dis = Vector3.Distance(ball.transform.position, goal.transform.position);
                yield return null;
            }

            _ballStack.Push(ball);
            balls.Remove(ball);

            StartCoroutine(Wait());

            if (!_isSpawn)
            {
                var par = Instantiate(particle, goal.position, Quaternion.identity);
                par.Play();
                _isSpawn = true;
            }
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(2f);
            _isMoving = false;
            resetButton.SetActive(true);
        }
    }
}