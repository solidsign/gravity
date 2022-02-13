using System;
using UnityEngine;

namespace Game
{
    public class PlayerWeaponShooter : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        private bool _switchable;
        private bool _alt1;
        private bool _alt2;
        private ISwitchable _switcher;
        private IAlternative1 _alternative1;
        private IAlternative2 _alternative2;
        public Weapon Weapon
        {
            set
            {
                weapon = value;
                if (weapon is ISwitchable switchable)
                {
                    _switchable =true;
                    _switcher = switchable;
                }
                else
                {
                    _switchable = false;
                    _switcher = null;
                }

                if (weapon is IAlternative1 alternative1)
                {
                    _alt1 =true;
                    _alternative1 = alternative1;
                }
                else
                {
                    _alt1 = false;
                    _alternative1 = null;
                }

                if (weapon is IAlternative2 alternative2)
                {
                    _alt2 =true;
                    _alternative2 = alternative2;
                }
                else
                {
                    _alt2 = false;
                    _alternative2 = null;
                }
            }
        }

        private void Awake()
        {
            Weapon = weapon;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                weapon.Shoot();
            }

            if (_switchable)
            {
                var scroll = Input.mouseScrollDelta.y;
                if (Mathf.Abs(scroll) < 0.2f) return;
                _switcher.Switch(scroll > 0f);
            }

            if (_alt1)
            {
                if(Input.GetMouseButtonDown(1)) _alternative1.Alternative1();
            }
            if (_alt2)
            {
                if(Input.GetMouseButtonDown(2)) _alternative2.Alternative2();
            }
        }
    }
}