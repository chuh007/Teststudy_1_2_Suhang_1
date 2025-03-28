using System;
using Code.Entities;
using UnityEngine;

namespace Code.Units
{
    public class UnitMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Rigidbody rbCompo;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float stopThreshold = 0.6f;

        private Entity _entity;
        private Vector3 _destination;
        
        private bool _isMoving;
        private Vector3 _moveDirection;

        public bool IsArrived => Vector3.Distance(_entity.transform.position, _destination) < stopThreshold;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void SetDestination(Vector3 destination)
        {
            _isMoving = true;
            _destination = destination;
            _moveDirection = _destination - _entity.transform.position;
            _moveDirection.y = 0;
            _moveDirection.Normalize();

            _entity.transform.forward = _moveDirection;
        }
        
        public void StopImmediately() => _isMoving = false;

        private void FixedUpdate()
        {
            // Debug.Log(Vector3.Distance(_entity.transform.position, _destination));
            if (_isMoving == false) return;
            
            rbCompo.MovePosition(_entity.transform.position + _moveDirection * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}