using Code.Entities;
using UnityEngine;

namespace Code.Units.States
{
    public class UnitAttackState : UnitState
    {
        private UniyAttackCompo _attackCompo;
        private float _lastAttackTime = 0f;
        private float _attackCooltime = 1.5f;
        public UnitAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = _unit.GetCompo<UniyAttackCompo>();

        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
        }

        public override void Update()
        {
            base.Update();
            if (_lastAttackTime + _attackCooltime < Time.time)
            {
                _lastAttackTime = Time.time;
                _attackCompo.Attack();
            }
            
        }
    }
}