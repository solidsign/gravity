using UnityEngine;

namespace Game
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("How is it viewed")] 
        [SerializeField] protected WeaponColorSetter colorSetter;
        [SerializeField] protected PlayableEffect shootEffect;
        public abstract void Shoot();
    }
}