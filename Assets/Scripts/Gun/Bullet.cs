using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    public Vector2 FireDirection { get; set; }
    private bool _isReleased;

    private Rigidbody2D _rigidBody;

    private void OnEnable()
    {
        _isReleased = false;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = FireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        
        if (!_isReleased)
        {
            _isReleased = true;
            Gun.BulletPool.Release(this);
        }
    }
}