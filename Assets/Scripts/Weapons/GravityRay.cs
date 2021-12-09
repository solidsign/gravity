using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class GravityRay : Weapon, ISwitchable, IAlternative1
    {
        [Header("How is it viewed")] 
        [SerializeField] private GravityRayCrosshairView crosshairView;
        [SerializeField] private PlayableEffect shootEffect;
        [Header("Allowed Gravity states cycle")]
        [SerializeField] private List<GravityState> statesOrder;
        [Header("Raycast properties")]
        [SerializeField] private Transform raycastPoint;
        [SerializeField] private float reloadTime;
        [SerializeField] private float maxDistance;
        [SerializeField] private float rayRadius;
        [SerializeField] private LayerMask layer;
        
        private bool _loaded = true;
        private int _currentState = 0;
        
        private TargetFinder _targetFinder;
        private bool _locked = false;
        private static readonly int Reload1 = Animator.StringToHash("reload");
        private Animator _animator;

        private void Awake()
        {
            _targetFinder = new TargetFinder(raycastPoint, maxDistance, rayRadius, layer);
            _animator = GetComponent<Animator>();
            _animator.speed = 1f / reloadTime;
        }

        private void Update()
        {
            if (!_locked && !_targetFinder.UpdateTarget())
            {
                crosshairView.Hide();
                return;
            }
            crosshairView.SetPosition(_targetFinder.Target.transform.position);
            crosshairView.Show();
        }

        public override void Shoot()
        {
            if (!_loaded) return;
            if (_targetFinder.Target == null) return;
            _targetFinder.Target.SetGravity(statesOrder[_currentState]);
            shootEffect?.Play();
            StartCoroutine(Reload());
        }

        public void Switch(bool delta)
        {
            _currentState += delta ? 1 : -1;
            _currentState %= statesOrder.Count;
            if (_currentState < 0) _currentState += statesOrder.Count;
            crosshairView.Reload(0f, statesOrder[_currentState]);
        }
        
        private IEnumerator Reload()
        {
            _loaded = false;
            _animator.SetTrigger(Reload1);
            crosshairView.Reload(reloadTime, statesOrder[_currentState]);
            yield return new WaitForSeconds(reloadTime);
            _loaded = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            var position = raycastPoint.position;
            Gizmos.DrawWireSphere(position, rayRadius);
            var forward = raycastPoint.forward;
            Gizmos.DrawWireSphere(position + forward * maxDistance, rayRadius);
            Gizmos.DrawRay(position, forward * maxDistance);
        }

        public void Alternative1()
        {
            if(_targetFinder.Target != null) _locked = !_locked;
        }
    }
}