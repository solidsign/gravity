using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MovementController))]
    public class GravityController : MonoBehaviour
    {
        [SerializeField] private GravityState startGravity;
        [Min(0f)] [SerializeField] private float gravityChangeTime;
        
        private MovementController _movement;
        private GravityState _currentGravity = GravityState.None;

        public event Action<GravityState, GravityState> GravityChangeStarted;
        public event Action GravityChangeFinished;

        public float GravityChangeTime => gravityChangeTime;
        public GravityState CurrentGravity => _currentGravity;
        private void Start()
        {
            SetGravity(startGravity);
        }

        public async void SetGravity(GravityState gravityState)
        {
            if (_currentGravity == gravityState) return;
            GravityChangeStarted?.Invoke(_currentGravity, gravityState);
            
            await Task.Delay((int) (gravityChangeTime * 1000));
            
            _currentGravity = gravityState;
            GravityChangeFinished?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            switch (startGravity)
            {
                case GravityState.White:
                    Gizmos.color = Color.white;
                    Gizmos.DrawRay(transform.position, new Vector3(0f, -1f, 0f) * 2f);
                    break;
                case GravityState.Yellow:
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawRay(transform.position, new Vector3(0f,1f,0f) * 2f);
                    break;
                case GravityState.Blue:
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(transform.position, new Vector3(0f,0f,-1f) * 2f);
                    break;
                case GravityState.Green:
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(transform.position, new Vector3(0f,0f,1f) * 2f);
                    break;
                case GravityState.Red:
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(transform.position, new Vector3(-1f,0f,0f) * 2f);
                    break;
                case GravityState.Orange:
                    Gizmos.color = new Color(255f, 125f, 0f);
                    Gizmos.DrawRay(transform.position, new Vector3(1f,0f,0f) * 2f);
                    break;
            }
        }
    }

    public enum GravityState
    {
        White, Yellow,
        Blue, Green,
        Red, Orange,
        None
    }
}