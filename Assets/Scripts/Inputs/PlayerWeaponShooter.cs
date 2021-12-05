using UnityEngine;

namespace Game
{
    public class PlayerWeaponShooter : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;


        public Weapon Weapon
        {
            set => weapon = value;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                weapon.Shoot();
            }
        }
    }
}