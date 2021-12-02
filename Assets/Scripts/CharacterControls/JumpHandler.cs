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
        [Tooltip("Reload of one jump")] [SerializeField] private float jumpReloadTime;
        [SerializeField] private Effect jumpEffect;
        [SerializeField] private int jumpsAmount;
        [SerializeField] private JumpStaminaUI _jumpStaminaUI;
        
        private MovementController _mover;
        private GravityApplier _gravity;
        private Coroutine _jumping;
        private float _jumpStamina;
        private bool _fullStamina = true;
        
        private void Awake()
        {
            _mover = GetComponent<MovementController>();
            _gravity = GetComponent<GravityApplier>();
            _jumpStamina = jumpsAmount;
            _jumpStaminaUI.Init(jumpsAmount);
        }

        private void Update()
        {
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
            if (_jumpStamina < 1f) return;
            _fullStamina = false;
            --_jumpStamina;
            _jumpStaminaUI.Show(jumpReloadTime / 5f);
            InterruptJump();
            _jumping = StartCoroutine(Jumping());
        }

        public void InterruptJump()
        {
            if (_jumping != null)
            {
                StopCoroutine(_jumping);
                _gravity.JumpFinished();
                jumpEffect?.ToggleOff();
            }
        }

        private IEnumerator Jumping()
        {
            _gravity.JumpStarted();
            jumpEffect?.ToggleOn();
            float timer = 0f, t = 0f, lastEval = 0f;

            while (t < 1f)
            {
                t = timer / jumpTime;
                var eval = jumpCurve.Evaluate(t);
                var delta = eval - lastEval;
                delta *= jumpHeight;
                _mover.Move(transform.up * delta);
                lastEval = eval;
                timer += Time.deltaTime;
                yield return null;
            }
            _jumping = null;
            _gravity.JumpFinished();
            jumpEffect?.ToggleOff();
        }
    }
}