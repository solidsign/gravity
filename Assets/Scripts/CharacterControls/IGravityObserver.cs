namespace Game
{
    public interface IGravityObserver
    {
        void GravityChangeStarted(GravityState prevState,GravityState newState);
        void GravityChangeFinished();
    }
}