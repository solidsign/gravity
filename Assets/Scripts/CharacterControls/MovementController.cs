using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        private CharacterController _character;
        private Vector3 _displacement;
        private List<ContactPoint> _contacts;
        private bool _grounded = false;

        public bool Grounded => _grounded;
        
        private void Awake()
        {
            _character = GetComponent<CharacterController>();
        }

        private void LateUpdate()
        {
            _grounded = _character.Move(_displacement) != CollisionFlags.None;
            _displacement = Vector3.zero;
        }
        
        public void Move(Vector3 displacement)
        {
            _displacement += displacement;
        }
    }
}