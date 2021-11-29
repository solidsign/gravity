using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class RotationHandler : GravityObserver
    {
        [SerializeField] private Transform horizontalRotationBody;
        [Tooltip("Camera. Must be child of horizontalRotationBody")] [SerializeField] private Transform verticalRotationBody;

        [SerializeField] private float minV;
        [SerializeField] private float maxV;
        
        private float _hAngle = 0;
        private float _vRotation = 0;
        private float HAngle
        {
            get => _hAngle;
            set
            {
                _hAngle = value;
                _hAngle %=360;
                _hAngle= _hAngle>180 ? _hAngle-360 : _hAngle;
            }
        }

        private Vector3 _xAxis;

        private Vector3 _yAxis;

        private Vector3 _up;

        private Vector3 Forward => _xAxis * Mathf.Cos(_hAngle) + _yAxis * Mathf.Sin(_hAngle);

        public void RotateHorizontally(float angle)
        {
            HAngle += angle;
            horizontalRotationBody.rotation = Quaternion.LookRotation(Forward, _up);
        }
        public void RotateVertically(float amount)
        {
            _vRotation += amount;
            _vRotation = Mathf.Clamp(_vRotation, minV, maxV);
            verticalRotationBody.localEulerAngles = new Vector3(-_vRotation, 0, 0);
        }
        private void ReverseXAxis()
        {
            HAngle = _hAngle - 180;
        }

        public override void GravityInit(GravityState gravityState)
        {
            UpdateAxisBasis(gravityState);
            horizontalRotationBody.rotation = Quaternion.LookRotation(Forward, _up);
        }

        private void UpdateAxisBasis(GravityState gravityState)
        {
            switch (gravityState)
            {
                case GravityState.White:
                {
                    _xAxis = new Vector3(1, 0, 0);
                    _yAxis = new Vector3(0, 0, -1);
                    _up = new Vector3(0, 1, 0);
                    break;
                }
                case GravityState.Blue:
                {
                    _xAxis = new Vector3(0, 1, 0);
                    _yAxis = new Vector3(0, 0, -1);
                    _up = new Vector3(-1, 0, 0);
                    break;
                }
                case GravityState.Red:
                {
                    _xAxis = new Vector3(1, 0, 0);
                    _yAxis = new Vector3(0, 1, 0);
                    _up = new Vector3(0, 0, 1);
                    break;
                }
                case GravityState.Yellow:
                {
                    _xAxis = new Vector3(-1, 0, 0);
                    _yAxis = new Vector3(0, 0, -1);
                    _up = new Vector3(0, -1, 0);
                    break;
                }
                case GravityState.Green:
                {
                    _xAxis = new Vector3(0, -1, 0);
                    _yAxis = new Vector3(0, 0, -1);
                    _up = new Vector3(1, 0, 0);
                    break;
                }
                case GravityState.Orange:
                {
                    _xAxis = new Vector3(-1, 0, 0);
                    _yAxis = new Vector3(0, 1, 0);
                    _up = new Vector3(0, 0, -1);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(gravityState), gravityState, null);
            }
        }

        public override void GravityChangeStarted(GravityState prevState, GravityState newState, float gravityChangeTime)
        {
            UpdateAxisBasis(newState);
            AdditionalRotation(prevState, newState);
            horizontalRotationBody.DOKill();
            horizontalRotationBody.DORotateQuaternion(Quaternion.LookRotation(Forward, _up), gravityChangeTime);
        }

        // Can be improved
        private void AdditionalRotation(GravityState prevState, GravityState newState)
        {
            switch (prevState)
            {
                case GravityState.White:
                {
                    switch (newState)
                    {
                        case GravityState.Orange:
                            HAngle += 180;
                            return;
                        case GravityState.Yellow:
                            ReverseXAxis();
                            return;
                    }
                    break;
                }
                case GravityState.Blue:
                {
                    switch (newState)
                    {
                        case GravityState.Green:
                            ReverseXAxis();
                            return;
                        case GravityState.Red:
                            HAngle -= 90;
                            return;
                        case GravityState.Orange:
                            HAngle -= 90;
                            return;
                    }
                    break;
                }
                case GravityState.Red:
                {
                    switch (newState)
                    {
                        case GravityState.Blue:
                            HAngle += 90;
                            return;
                        case GravityState.Green:
                            HAngle -= 90;
                            return;
                        case GravityState.Yellow:
                            HAngle += 180;
                            return;
                        case GravityState.Orange:
                            ReverseXAxis();
                            return;
                    }
                    break;
                }
                case GravityState.Yellow:
                {
                    switch (newState)
                    {
                        case GravityState.White:
                            ReverseXAxis();
                            return;
                        case GravityState.Red:
                            HAngle += 180;
                            return;
                    }
                    break;
                }
                case GravityState.Green:
                {
                    switch (newState)
                    {
                        case GravityState.Blue:
                            ReverseXAxis();
                            return;
                        case GravityState.Red:
                            HAngle += 90;
                            return;
                        case GravityState.Orange:
                            HAngle += 90;
                            return;
                    }
                    break;
                }
                case GravityState.Orange:
                {
                    switch (newState)
                    {
                        case GravityState.White:
                            HAngle += 180;
                            return;
                        case GravityState.Blue:
                            HAngle += 90;
                            return;
                        case GravityState.Green:
                            HAngle -= 90;
                            return;
                        case GravityState.Red:
                            ReverseXAxis();
                            return;
                    }
                    break;
                }
            }
        }

        public override void GravityChangeFinished() { }
    }
}