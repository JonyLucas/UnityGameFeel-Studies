using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;
    private Vector2 _direction;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Rotate();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            newBullet.FireDirection = _direction.normalized;
        }
    }

    private void Rotate()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var worldMousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        _direction = worldMousePosition - transform.position;
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _spriteRenderer.flipY = angle > 90 || angle < -90;
    }

}