using UnityEngine;

namespace Code.Combat
{
    public interface IHitable
    {
        public void Hit(Vector3 normal);
    }
}