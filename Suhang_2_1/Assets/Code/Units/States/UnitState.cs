using Code.Entities;
using Code.FSM;
using UnityEngine;

namespace Code.Units.States
{
    public abstract class UnitState : EntityState
    {
        protected Unit _unit;
        protected UnitMovement _movement;
        
        public UnitState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _unit = entity as Unit;
            _movement = entity.GetCompo<UnitMovement>();
        }
    }
}