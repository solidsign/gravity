using UnityEngine;

namespace Game
{
    public abstract class GravityObserver : MonoBehaviour
    {
        [SerializeField] private bool observing = true;
        public bool Observing => observing;
        public abstract void GravityInit(GravityState gravityState);
        public abstract void GravityChangeStarted(GravityState prevState,GravityState newState, float gravityChangeTime);
        public abstract void GravityChangeFinished();
    }
}