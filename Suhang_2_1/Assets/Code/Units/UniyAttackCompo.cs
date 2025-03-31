using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.Units
{
    public class UniyAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzleTrm;
        private Transform _target;
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        public void Attack()
        {
            Vector3 direction = _target.position - _entity.transform.position;
            direction.y = 0;
            direction.Normalize();
            _entity.transform.forward = direction;
            Bullet bullet = Instantiate(bulletPrefab, muzzleTrm.position, Quaternion.identity);
            bullet.Fire(_target);
        }
    }
}