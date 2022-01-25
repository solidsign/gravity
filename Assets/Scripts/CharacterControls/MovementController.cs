using System.Collections.Generic;
using MenteBacata.ScivoloCharacterController;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CharacterMover))]
    public class MovementController : MonoBehaviour
    {
        private CharacterMover _mover;
        private Vector3 _displacement;
        private MoveContact[] _contacts;
        private int _contactsAmount;

        public bool HasContacts() => _contactsAmount > 0;
        
        public bool OnWall(Vector3 down)
        {
            for (int i = 0; i < _contactsAmount; i++)
            {
                if (Mathf.Abs(Vector3.Dot(down, _contacts[i].normal)) < 0.3f) return true;
            }

            return false;
        }

        public void Grounded(Vector3 down, out bool onFloor, out bool onWall)
        {
            onFloor = false;
            onWall = false;
            
            for (int i = 0; i < _contactsAmount || (onWall && onFloor); i++)
            {
                var dot = Vector3.Dot(down, _contacts[i].normal);
                if (!onWall && Mathf.Abs(dot) < 0.3f)
                {
                    onWall = true;
                }
                else if (!onFloor && dot > 0.3f)
                {
                    onFloor = true;
                }
            }
        }

        public void Grounded(Vector3 down, out Vector3 sumNormal, out bool onFloor, out bool onWall)
        {
            var s = new Vector3();
            onFloor = false;
            onWall = false;
            
            for (int i = 0; i < _contactsAmount; i++)
            {
                var dot = Vector3.Dot(down, _contacts[i].normal);
                if (Mathf.Abs(dot) < 0.3f)
                {
                    s += _contacts[i].normal;
                    onWall = true;
                }
                else if (dot > 0.3f) onFloor = true;
            }

            sumNormal = onWall ? Vector3.zero : s.normalized;
        }
        
        private void Awake()
        {
            _mover = GetComponent<CharacterMover>();
            _contacts = new MoveContact[6];
        }

        private void LateUpdate()
        {
            _mover.Move(_displacement, _contacts, out _contactsAmount);
            _displacement = Vector3.zero;
        }
        
        public void Move(Vector3 displacement)
        {
            _displacement += displacement;
        }
    }
}