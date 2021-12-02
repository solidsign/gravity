using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float groundCheckDistance;
        private CharacterController _character;
        private Vector3 _displacement;
        private List<ContactPoint> _contacts;
        private bool _grounded = false;

        public bool Grounded => _grounded;

        // it's shit af that i had to do this, i hope i will learn how to change ground for character controller and rewrite this 
        public bool IsGrounded(Vector3 down)
        {
            return Physics.Raycast(transform.position, transform.position + down, groundCheckDistance, 1 << 8);
        }

        private void Awake()
        {
            _character = GetComponent<CharacterController>();
        }

        private void LateUpdate()
        {
            var v = _character.Move(_displacement);
            _grounded = _character.isGrounded;
            //Debug.Log(v);
            _displacement = Vector3.zero;
        }

        public void Move(Vector3 displacement)
        {
            _displacement += displacement;
        }

        private void OnCollisionEnter(Collision other)
        {
            //other.GetContacts(_contacts);
            Debug.Log("stay");
        }
    }
}