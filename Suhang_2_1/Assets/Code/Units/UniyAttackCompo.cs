using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.Units
{
    public class UniyAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Bullet bulletPrefab;
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void Attack(Transform target)
        {
            Vector3 direction = target.position - _entity.transform.position;
            direction.y = 0;
            direction.Normalize();
            _entity.transform.forward = direction;
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Fire(target);
        }
    }
}