using UnityEngine;

namespace Game
{
    public abstract class GravityObserver : MonoBehaviour
    {
        public abstract void GravityChangeStarted(GravityState prevState,GravityState newState);
        public abstract void GravityChangeFinished();
    }
}