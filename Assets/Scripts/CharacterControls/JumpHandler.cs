using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(GravityApplier), typeof(MovementController))]
    public class JumpHandler : MonoBehaviour
    {
        [Header("Jump")] [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpTime;
        [SerializeField] private float wallJumpSideMultiplier;
        [SerializeField] private float wallJumpUpMultiplier;
        [SerializeField] private Effect jumpEffect;
        
        private MovementController _mover;
        private GravityApplier _gravity;
        private Coroutine _jumping;
        private Vector3 _jumpDirection;
        
        private void Awake()
        {
            _mover = GetComponent<MovementController>();
            _gravity = GetComponent<GravityApplier>();
        }

        public void StartJump()
        {
            if (!_mover.HasContacts()) return;
            
            InterruptJump();

            CalculateJumpDirection();

            _jumping = StartCoroutine(Jumping());
        }

        private void CalculateJumpDirection()
        {
            _mover.Grounded(-transform.up, out var sumNormal, out var onFloor, out var onWall);
            if (onFloor)
            {
                _jumpDirection = transform.up;
                return;
            }

            if (onWall)
            {
                _jumpDirection = sumNormal * wallJumpSideMultiplier + transform.up * wallJumpUpMultiplier;
                return;
            }

            _jumpDirection = transform.up;
        }

        public void InterruptJump()
        {
            if (_jumping == null) return;
            StopCoroutine(_jumping);
            _gravity.JumpFinished();
            jumpEffect?.ToggleOff();
        }

        private IEnumerator Jumping()
        {
            _gravity.JumpStarted();
            jumpEffect?.ToggleOn();
            float timer = 0f, t = 0f, lastEval = 0f;

            while (t < 1f)
            {
                timer += Time.deltaTime;
                t = timer / jumpTime;
                var eval = jumpCurve.Evaluate(t);
                var delta = eval - lastEval;
                delta *= jumpHeight;
                _mover.Move(_jumpDirection * delta);
                lastEval = eval;
                yield return null;
                if (_mover.HasContacts())
                {
                    _jumping = null;
                    _gravity.JumpFinished();
                    jumpEffect?.ToggleOff();
                    yield break;
                }
            }
            _jumping = null;
            _gravity.JumpFinished();
            jumpEffect?.ToggleOff();
        }
    }
}