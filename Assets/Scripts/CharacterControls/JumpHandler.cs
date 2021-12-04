using System;
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
        [SerializeField] private float wallJumpCheckDistance;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float wallJumpMultiplier;
        [Tooltip("Reload of one jump")] [SerializeField] private float jumpReloadTime;
        [SerializeField] private Effect jumpEffect;
        [SerializeField] private int jumpsAmount;
        [SerializeField] private JumpStaminaUI _jumpStaminaUI;
        
        private MovementController _mover;
        private GravityApplier _gravity;
        private Coroutine _jumping;
        private float _jumpStamina;
        private bool _fullStamina = true;
        private Vector3 _jumpDirection;
        private readonly int _wallLayerMask = 1 << 8;
        
        private void Awake()
        {
            _mover = GetComponent<MovementController>();
            _gravity = GetComponent<GravityApplier>();
            _jumpStamina = jumpsAmount;
            _jumpStaminaUI.Init(jumpsAmount);
        }

        private void Update()
        {
            #region Debugging
            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                _jumpStaminaUI.AddJumps(1);
                ++jumpsAmount;
                ++_jumpStamina;
                _fullStamina = false;
                _jumpStaminaUI.Show(jumpReloadTime / 5f);
            }
            if(Input.GetKeyDown(KeyCode.KeypadMinus) && jumpsAmount > 0)
            {
                _jumpStaminaUI.DeleteJumps(1);
                --jumpsAmount;
                --_jumpStamina;
                if (_jumpStamina < 0f) _jumpStamina = 0f;
                _jumpStaminaUI.Show(jumpReloadTime / 5f);
            }
            #endregion

            if (_fullStamina) return;
            _jumpStamina += Time.deltaTime * (1f / jumpReloadTime);
            if (_jumpStamina > jumpsAmount)
            {
                _jumpStamina = jumpsAmount;
                _fullStamina = true;
                _jumpStaminaUI.Hide(jumpReloadTime / 3f);
            }
            _jumpStaminaUI.SetStamina(_jumpStamina);
        }

        public void StartJump()
        {
            /*
            #region StaminaVariant
            if (_jumpStamina < 1f) return;
            _fullStamina = false;
            --_jumpStamina;
            _jumpStaminaUI.Show(jumpReloadTime / 5f);
            #endregion
*/

            #region GroundVariant
            if (!_mover.HasContacts()) return;
            #endregion
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
                _jumpDirection = sumNormal * wallJumpMultiplier + transform.up;
                _jumpDirection.Normalize();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, wallJumpCheckDistance);
            Gizmos.DrawRay(position, -transform.up * groundCheckDistance);
        }
    }
}