using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MovementController), typeof(JumpHandler))]
    public class PlayerMovement : GravityObserver
    {
        [Header("Run")] [SerializeField] private float speed;
        [SerializeField] private float runSpeed;
        [SerializeField] private Effect runEffect;
        
        private MovementController _mover;
        private JumpHandler _jumper;
        private bool _running = false;
        private Coroutine _jumping;
        private void Awake()
        {
            _mover = GetComponent<MovementController>();
            _jumper = GetComponent<JumpHandler>();
        }

        private void Update()
        {
            if (!Input.anyKey) return;

            var displacement = Vector3.zero;
            
            if (Input.GetKey(KeyCode.W))
            {
                displacement = transform.forward;

                switch (_running)
                {
                    case false when Input.GetKey(KeyCode.LeftShift):
                        runEffect.ToggleOn();
                        break;
                    case true when !Input.GetKey(KeyCode.LeftShift):
                        runEffect.ToggleOff();
                        break;
                }
                _running = Input.GetKey(KeyCode.LeftShift);
            }
            else
            {
                if(_running) runEffect.ToggleOff();
                _running = false;
            }
            
            if (!_running)
            {
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
            }

            var currentSpeed = _running ? runSpeed : speed;
            _mover.Move(currentSpeed * Time.deltaTime * displacement);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumper.StartJump();
            }
        }
        public override void GravityInit(GravityState gravityState)
        {
        }

        public override void GravityChangeStarted(GravityState prevState, GravityState newState,
            float gravityChangeTime)
        {
            enabled = false;
            _jumper.InterruptJump();
            runEffect.ToggleOff();
            _running = false;
        }

        public override void GravityChangeFinished()
        {
            enabled = true;
        }
    }
}