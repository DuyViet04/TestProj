using System.Collections.Generic;
using UnityEngine;

namespace _Data.Scripts
{
    public struct FarthestData
    {
        public Transform Ball;
        public float Distance;
    }

    public class FindFarthestService
    {
        public FarthestData FarthestBall(List<Transform> balls, Transform player)
        {
            FarthestData farthest = new FarthestData();
            float max = float.MinValue;

            foreach (var ball in balls)
            {
                var dis = Vector3.Distance(ball.position, player.position);
                if (dis > max)
                {
                    max = dis;
                    farthest = new FarthestData() { Ball = ball, Distance = max };
                }
            }

            return farthest;
        }
    }
}