using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(GravityController), typeof(MovementController))]
    public class GravityApplier : GravityObserver
    {
        [SerializeField] private float gravityForce; 
        private MovementController _movement;
        private Vector3 _down;

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
        }

        private void FixedUpdate()
        {
            _movement.Move(gravityForce * Time.fixedDeltaTime * _down);
        }

        public override void GravityInit(GravityState gravityState)
        {
            UpdateAxisBasis(gravityState);
        }

        private void UpdateAxisBasis(GravityState gravityState)
        {
            switch (gravityState)
            {
                case GravityState.White:
                {
                    _down = new Vector3(0, -1, 0);
                    break;
                }
                case GravityState.Blue:
                {
                    _down = new Vector3(1, 0, 0);
                    break;
                }
                case GravityState.Red:
                {
                    _down = new Vector3(0, 0, -1);
                    break;
                }
                case GravityState.Yellow:
                {
                    _down = new Vector3(0, 1, 0);
                    break;
                }
                case GravityState.Green:
                {
                    _down = new Vector3(-1, 0, 0);
                    break;
                }
                case GravityState.Orange:
                {
                    _down = new Vector3(0, 0, 1);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(gravityState), gravityState, null);
            }
        }
        
        public override void GravityChangeStarted(GravityState prevState, GravityState newState, float gravityChangeTime)
        {
            UpdateAxisBasis(newState);
        }

        public override void GravityChangeFinished() { }
    }
}