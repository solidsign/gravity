using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class GravityController : MonoBehaviour
    {
        [SerializeField] private GravityState startGravity;
        [Min(0f)] [SerializeField] private float gravityChangeTime;
        [SerializeField] private List<GravityObserver> gravityObservers;
        
        private GravityState _currentGravity = GravityState.None;
        private Coroutine _setGravity;
        private void Start()
        {
            _currentGravity = startGravity;
            foreach (var gravityObserver in gravityObservers)
            {
                gravityObserver.GravityInit(_currentGravity);
            }
        }

        // Update for debug
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) SetGravity(GravityState.White);
            if(Input.GetKeyDown(KeyCode.Alpha2)) SetGravity(GravityState.Yellow);
            if(Input.GetKeyDown(KeyCode.Alpha3)) SetGravity(GravityState.Green);
            if(Input.GetKeyDown(KeyCode.Alpha4)) SetGravity(GravityState.Red);
            if(Input.GetKeyDown(KeyCode.Alpha5)) SetGravity(GravityState.Orange);
            if(Input.GetKeyDown(KeyCode.Alpha6)) SetGravity(GravityState.Blue);
        }

        public void SetGravity(GravityState gravityState)
        {
            if (_setGravity != null)
            {
                StopCoroutine(_setGravity);
                foreach (var gravityObserver in gravityObservers)
                {
                    gravityObserver.GravityChangeFinished();
                }
            }

            _setGravity = StartCoroutine(SetGravityAsync(gravityState));
        }

        private IEnumerator SetGravityAsync(GravityState gravityState)
        {
            if (_currentGravity == gravityState) yield break;
            foreach (var gravityObserver in gravityObservers)
            {
                gravityObserver.GravityChangeStarted(_currentGravity, gravityState, gravityChangeTime);
            }

            yield return new WaitForSeconds(gravityChangeTime);
            
            _currentGravity = gravityState;

            foreach (var gravityObserver in gravityObservers)
            {
                gravityObserver.GravityChangeFinished();
            }

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

    public enum GravityState
    {
        White, Yellow,
        Blue, Green,
        Red, Orange,
        None
    }
}