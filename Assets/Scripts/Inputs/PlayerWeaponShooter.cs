using System;
using UnityEngine;

namespace Game
{
    public class PlayerWeaponShooter : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        private bool _switchable;
        private bool _alt1C;
        private bool _alt2C;
        private bool _alt1R;
        private bool _alt2R;
        private ISwitchable _switcher;
        private IChargeAlternative1 _chargeAlternative1;
        private IChargeAlternative2 _chargeAlternative2;
        private IReleaseAlternative1 _releaseAlternative1;
        private IReleaseAlternative2 _releaseAlternative2;
        public Weapon Weapon
        {
            set
            {
                weapon = value;
                
                _switcher = weapon as ISwitchable;
                _switchable = _switcher != null;

                _chargeAlternative1 = weapon as IChargeAlternative1;
                _alt1C = _chargeAlternative1 != null;

                _chargeAlternative2 = weapon as IChargeAlternative2;
                _alt2C = _chargeAlternative2 != null;

                _releaseAlternative1 = weapon as IReleaseAlternative1;
                _alt1R = _releaseAlternative1 != null;

                _releaseAlternative2 = weapon as IReleaseAlternative2;
                _alt2R = _releaseAlternative2 != null;
            }
        }

        private void Awake()
        {
            Weapon = weapon;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0)) weapon.ChargeShoot();
            if(Input.GetMouseButtonUp(0)) weapon.ReleaseShot();

            if (_switchable)
            {
                var scroll = Input.mouseScrollDelta.y;
                if (Mathf.Abs(scroll) < 0.2f) return;
                _switcher.Switch(scroll > 0f);
            }

            if (_alt1C && Input.GetMouseButtonDown(1)) _chargeAlternative1.ChargeAlternative1();
            if (_alt2C && Input.GetMouseButtonDown(2)) _chargeAlternative2.ChargeAlternative2();
            if (_alt1R && Input.GetMouseButtonUp(1)) _releaseAlternative1.ReleaseAlternative1();
            if (_alt2R && Input.GetMouseButtonUp(2)) _releaseAlternative2.ReleaseAlternative2();
        }
    }
}