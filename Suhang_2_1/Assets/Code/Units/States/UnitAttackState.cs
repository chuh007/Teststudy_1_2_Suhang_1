using Code.Entities;

namespace Code.Units.States
{
    public class UnitAttackState : UnitState
    {
        
        public UnitAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
        }

        public override void Update()
        {
            base.Update();
            
        }
    }
}