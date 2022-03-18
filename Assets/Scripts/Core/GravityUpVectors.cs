using System;
using UnityEngine;

namespace Game
{
    public static class GravityUpVectors
    {
        public static readonly Vector3 White = new Vector3(0, 1, 0);
        public static readonly Vector3 Yellow = new Vector3(0, -1, 0);
        public static readonly Vector3 Green = new Vector3(1, 0, 0);
        public static readonly Vector3 Blue = new Vector3(-1, 0, 0);
        public static readonly Vector3 Red = new Vector3(0, 0, 1);
        public static readonly Vector3 Orange = new Vector3(0, 0, -1);

        public static GravityState ClosestStateForLookingVector(Vector3 vector)
        {
            if (vector.magnitude < Single.Epsilon) return GravityState.None;
            var state = GravityState.None;
            var mindot = 2f;
            var dot = Vector3.Dot(White, vector);
            if (mindot > dot)
            {
                mindot = dot;
                state = GravityState.White;
            }
            dot = Vector3.Dot(Yellow, vector);
            if (mindot > dot)
            {
                mindot = dot;
                state = GravityState.Yellow;
            }
            dot = Vector3.Dot(Green, vector);
            if (mindot > dot)
            {
                mindot = dot;
                state = GravityState.Green;
            }
            dot = Vector3.Dot(Blue, vector);
            if (mindot > dot)
            {
                mindot = dot;
                state = GravityState.Blue;
            }
            dot = Vector3.Dot(Red, vector);
            if (mindot > dot)
            {
                mindot = dot;
                state = GravityState.Red;
            }
            if (mindot > Vector3.Dot(Orange, vector))
            {
                state = GravityState.Orange;
            }

            return state;
        }
    }
}