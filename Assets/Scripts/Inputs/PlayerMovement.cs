using DG.Tweening;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MovementController))]
    public class PlayerMovement : GravityObserver
    {
        [Header("Run")] [SerializeField] private float speed;
        [Header("Jump")] [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpTime;
        private MovementController _mover;

        private void Awake()
        {
            _mover = GetComponent<MovementController>();
        }

        private void Update()
        {
            if (!Input.anyKey) return;

            var displacement = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                displacement = transform.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                displacement -= transform.right;
            }

            if (Input.GetKey(KeyCode.S))
            {
                displacement -= transform.forward;
            }

            if (Input.GetKey(KeyCode.D))
            {
                displacement += transform.right;
            }

            _mover.Move(Time.deltaTime * speed * displacement);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
            }
        }

        public override void GravityInit(GravityState gravityState)
        {
        }

        public override void GravityChangeStarted(GravityState prevState, GravityState newState,
            float gravityChangeTime)
        {
            enabled = false;
            StopAllCoroutines();
        }

        public override void GravityChangeFinished()
        {
            enabled = true;
        }
    }
}