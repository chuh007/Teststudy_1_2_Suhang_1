using System;
using Code.Entities;
using Code.FSM;
using Code.UI;
using UnityEngine;

namespace Code.Units
{
    public class Unit : Entity
    {
        [SerializeField] private StateDataSO[] stateList;
        [SerializeField] private ActiveCircle activeCircle;
        
        private EntityStateMachine _stateMachine;
        private UnitMovement _movement;
        private UniyAttackCompo _attackCompo;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, stateList);
            _movement = GetCompo<UnitMovement>();
            _attackCompo = GetCompo<UniyAttackCompo>();
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void SetSelected(bool isSelected)
        {
            activeCircle.SetActive(isSelected);
        }

        public void MoveToPosition(Vector3 position)
        {
            _movement.SetDestination(position);
            _stateMachine.ChangeState("MOVE");
        }

        public void Attack(Transform target)
        {
            _attackCompo.SetTarget(target);
            _stateMachine.ChangeState("ATTACK");
        }
        
        public void ChangeState(string newState) => _stateMachine.ChangeState(newState);
    }
}