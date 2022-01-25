using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GravityController : MonoBehaviour
    {
        [SerializeField] private GravityState startGravity;
        [Min(0f)] [SerializeField] private float gravityChangeTime;
        
        private List<GravityObserver> gravityObservers;
        private GravityState _currentGravity = GravityState.None;
        private Coroutine _setGravity;
        private void Start()
        {
            _currentGravity = startGravity;

            InitGravityObservers();
            
        }

        private void InitGravityObservers()
        {
            gravityObservers = GetComponents<GravityObserver>().ToList(); 
            gravityObservers.ForEach((el) => {if(el.Observing) el.GravityInit(_currentGravity);});
        }

        public void SetGravity(GravityState gravityState)
        {
            if (_setGravity != null)
            {
                StopCoroutine(_setGravity);
                gravityObservers.ForEach((el) => el.GravityChangeFinished());
            }

            _setGravity = StartCoroutine(SetGravityAsync(gravityState));
        }

        private IEnumerator SetGravityAsync(GravityState gravityState)
        {
            if (_currentGravity == gravityState) yield break;
            gravityObservers.ForEach((el) => el.GravityChangeStarted(_currentGravity, gravityState, gravityChangeTime));

            yield return new WaitForSeconds(gravityChangeTime);
            
            _currentGravity = gravityState;

            gravityObservers.ForEach((el) => el.GravityChangeFinished());

            _setGravity = null;
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

    
}