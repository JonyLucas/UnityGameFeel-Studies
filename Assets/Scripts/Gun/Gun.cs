using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public static ObjectPool<Bullet> BulletPool { get; private set; }

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fireCooldown = 0.5f;
    
    private Camera _mainCamera;
    private Vector2 _direction;
    private Tween _recoilTween;
    private float _fireTimer;

    private void Awake()
    {
        _mainCamera = Camera.main;
        BulletPool = new ObjectPool<Bullet>(
            () => Instantiate(_bulletPrefab), 
            bullet => bullet.gameObject.SetActive(true), 
            bullet => bullet.gameObject.SetActive(false),
            bullet => Destroy(bullet.gameObject));
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ShootAnimation;
    }
    
    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ShootAnimation;
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
        var newBullet = BulletPool.Get();
        newBullet.transform.position = _bulletSpawnPoint.position;
        newBullet.FireDirection = _direction.normalized;
        _fireTimer = 0;
    }

    private void ShootAnimation()
    {
        _recoilTween?.Kill();
        _recoilTween = transform.DOShakePosition(0.1f, 0.2f, 10, 90, false);
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