using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TargetFinder
    {
        private readonly Transform _raycastPoint;
        private readonly float _maxDistance;
        private readonly float _rayRadius;
        private readonly LayerMask _layer;

        public TargetFinder(Transform raycastPoint,float maxDistance,float rayRadius, LayerMask layer)
        {
            _raycastPoint = raycastPoint;
            _maxDistance = maxDistance;
            _rayRadius = rayRadius;
            _layer = layer;
            Target = null;
        }

        public GravityController Target { get; private set; }
        
        public bool UpdateTarget()
        {
            Ray ray = new Ray(_raycastPoint.position, _raycastPoint.forward);
            if (!Physics.SphereCast(ray, _rayRadius, out var hit, _maxDistance, _layer))
            {
                Target = null;
                return false;
            }

            if (!hit.transform.TryGetComponent<GravityController>(out var component))
            {
                Target = null;
                return false;
            }

            Target = component;
            return true;
        }
        
    }
}