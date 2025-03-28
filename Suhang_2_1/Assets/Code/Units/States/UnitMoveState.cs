using Code.Entities;
using UnityEngine;

namespace Code.Units.States
{
    public class UnitMoveState : UnitState
    {
        public UnitMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();
            Debug.Log(_movement.IsArrived);
            if (_movement.IsArrived)
            {
                _unit.ChangeState("IDLE");
            }
        }
    }
}