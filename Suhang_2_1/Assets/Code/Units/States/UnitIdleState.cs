using Code.Entities;
using UnityEngine;

namespace Code.Units.States
{
    public class UnitIdleState : UnitState
    {
        public UnitIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
        }
    }
}