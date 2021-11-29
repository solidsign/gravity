namespace Game
{
    public interface IGravityObserver
    {
        public void GravityChangeStarted(GravityState prevState,GravityState newState);
        public void GravityChangeFinished();
    }
}