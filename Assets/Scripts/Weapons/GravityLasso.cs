using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class GravityLasso : Weapon, IChargeAlternative1, IReleaseAlternative1
    {
        [Header(("Properties that will be removed later"))] 
        [SerializeField] private RotationHandler rotationHandler;
        [Header("Weapon Properties")]
        [SerializeField] private float dischargeTime;
        [Header("View")] 
        [SerializeField] private GravityLassoCrosshairView _view;

        [Header("Input Properties")] 
        [SerializeField] private float zeroChargeBorder;
        [SerializeField] private float firstChargeBorder;
        [Header("Raycast properties")]
        [SerializeField] private Transform raycastPoint;
        [SerializeField] private float maxDistance;
        [SerializeField] private float rayRadius;
        [SerializeField] private LayerMask layer;
        
        
        private TargetFinder _targetFinder;
        private bool _charged = false;

        private Coroutine _charging = null;
        private Coroutine _discharging = null;
        private GravityState _charge = GravityState.None;

        private void Start()
        {
            _targetFinder = new TargetFinder(raycastPoint, maxDistance, rayRadius, layer);
        }

        private void LateUpdate()
        {
            if (_charged) return;
            
            if (_targetFinder.UpdateTarget())
            {
                _view.ShowTarget();
            }
            else
            {
                _view.HideTarget();
            }
        }

        public override void ChargeShoot()
        {
            if (_targetFinder.Target == null) return;
            _charged = true;
            _view.ShowCharger();
            _view.SetCharge(GravityState.None);
            _charging = StartCoroutine(Charging());
            _discharging = StartCoroutine(DischargeTimer());
        }

        private IEnumerator DischargeTimer()
        {
            yield return new WaitForSeconds(dischargeTime);
            _charged = false;
            _discharging = null;
            StopCoroutine(_charging);
            _view.HideAll();
        }

        public override void ReleaseShot()
        {
            StopAllCoroutines();
            _discharging = null;
            if (!_charged || _charge == GravityState.None)
            {
                _view.HideAll();
                return;
            }
            _charged = false;
            _view.ReleaseAnim();
            _targetFinder.Target.SetGravity(_charge);
        }

        private IEnumerator Charging()
        {
            Vector3 startMousePosition = Input.mousePosition;
            Vector3 lookVector = _targetFinder.Target.transform.position - transform.position;
            while (true)
            {
                _charge = VectorToGravityState(lookVector, Input.mousePosition - startMousePosition);
                _view.SetCharge(_charge);
                yield return null;
            }
        }

        private GravityState VectorToGravityState(Vector3 lookVector, Vector3 mouseDelta)
        {
            if (mouseDelta.magnitude < zeroChargeBorder) return GravityState.None;
            lookVector.Normalize();
            var down = rotationHandler.Up.normalized;
            var r = Vector3.Cross(lookVector, down);
            Vector3 gravVector;
            if (mouseDelta.magnitude < firstChargeBorder)
            {
                mouseDelta.Normalize();
                gravVector = Vector3.RotateTowards(down, mouseDelta.y * lookVector + mouseDelta.x * r, Mathf.Deg2Rad * 90f, 0);
            }
            else
            {
                mouseDelta.Normalize();
                gravVector = Vector3.RotateTowards(down, mouseDelta.y * lookVector + mouseDelta.x * r, Mathf.Deg2Rad * 180f, 0);
            }

            return GravityUpVectors.ClosestStateForLookingVector(gravVector);
        }

        public void ChargeAlternative1()
        {
            throw new NotImplementedException();
        }

        public void ReleaseAlternative1()
        {
            throw new NotImplementedException();
        }
    }
}