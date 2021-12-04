using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(GravityController), typeof(MovementController))]
    public class GravityApplier : GravityObserver
    {
        [SerializeField] private float gravityAcceleration;
        [SerializeField] private float onWallGravityVelocity;
        private MovementController _mover;
        private Vector3 _down;
        private float _velocity;

        private void Awake()
        {
            _mover = GetComponent<MovementController>();
        }

        private void Update()
        {
            if (_mover.OnWall(_down))
            {
                _velocity = onWallGravityVelocity * Time.deltaTime;
            }
            else
            {
                _velocity += Time.deltaTime * gravityAcceleration;
            }
            _mover.Move(_velocity * _down);
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
            _velocity = 0;
            UpdateAxisBasis(newState);
        }

        public override void GravityChangeFinished() { }

        public void JumpStarted()
        {
            _velocity = 0f;
            enabled = false;
        }

        public void JumpFinished()
        {
            _velocity = 0f;
            enabled = true;
        }
    }
}