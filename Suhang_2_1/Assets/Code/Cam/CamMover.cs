using System;
using UnityEngine;

namespace Code.Cam
{
    public class CamMover : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInpiut;
        [SerializeField] private float _speed = 10f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 movement = Quaternion.Euler(0, -45f, 0) *
                               new Vector3(playerInpiut.MovementKey.x, 0, playerInpiut.MovementKey.y);
                _rb.linearVelocity = movement * _speed;
        }
    }
}
