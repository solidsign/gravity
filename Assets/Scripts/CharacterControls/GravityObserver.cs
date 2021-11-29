using UnityEngine;

namespace Game
{
    public abstract class GravityObserver : MonoBehaviour
    {
        public abstract void GravityInit(GravityState gravityState);
        public abstract void GravityChangeStarted(GravityState prevState,GravityState newState, float gravityChangeTime);
        public abstract void GravityChangeFinished();
    }
}