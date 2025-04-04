﻿using System;
using UnityEngine;

namespace Code.Combat
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Fire(Transform target)
        {
            _rb.linearVelocity = (target.position - transform.position).normalized * 25f;
        }

        private void OnCollisionEnter(Collision other)
        {
            ContactPoint contact = other.contacts[0];
            Vector3 pos = contact.point;
            Vector3 normal = contact.normal;
            if (other.collider.TryGetComponent(out IHitable hitable))
            {
                hitable.Hit(normal);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
                Instantiate(particle, pos, rotation);
                Destroy(gameObject);
            }
        }

    }
}