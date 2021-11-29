using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        private CharacterController _character;
        private Vector3 _displacement;

        public bool Grounded => _character.isGrounded;

        private void Awake()
        {
            _character = GetComponent<CharacterController>();
        }

        private void LateUpdate()
        {
            _character.Move(_displacement);
            _displacement = Vector3.zero;
        }

        public void Move(Vector3 displacement)
        {
            _displacement += displacement;
        }
    }
}