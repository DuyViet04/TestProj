using System.Collections.Generic;
using UnityEngine;

namespace _Data.Scripts
{
    public struct ClosestData
    {
        public Transform Goal;
        public float Distance;
    }

    public class FindClosestService
    {
        public ClosestData ClosestGoal(List<Transform> goals, Transform ball)
        {
            ClosestData closest = new ClosestData();
            float min = float.MaxValue;
            
            if (ball == null) return new ClosestData();
            foreach (var goal in goals)
            {
                var dis = Vector3.Distance(goal.position, ball.position);
                if (dis < min)
                {
                    min = dis;
                    closest = new ClosestData() { Goal = goal, Distance = min };
                }
            }

            return closest;
        }
    }
}