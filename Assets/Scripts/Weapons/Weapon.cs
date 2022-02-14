using UnityEngine;

namespace Game
{
    public abstract class Weapon : MonoBehaviour
    {
        public virtual void ChargeShoot(){}
        public virtual void ReleaseShot(){}
    }
}