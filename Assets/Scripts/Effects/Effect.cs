using UnityEngine;

namespace Game
{
    public abstract class Effect : MonoBehaviour
    {
        public abstract void ToggleOn();
        public abstract void ToggleOff();
    }
}