using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fireCooldown = 0.5f;
    
    private Camera _mainCamera;
    private Vector2 _direction;
    private float _fireTimer;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
    }
    
    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
    }

    private void Update()
    {
        Rotate();
        Shoot();
    }

    private void Shoot()
    {
        _fireTimer += Time.deltaTime;
        if (Mouse.current.leftButton.isPressed && _fireTimer >= _fireCooldown)
        {
            OnShoot?.Invoke();
        }
    }

    private void ShootProjectile()
    {
        var newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.FireDirection = _direction.normalized;
        _fireTimer = 0;
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