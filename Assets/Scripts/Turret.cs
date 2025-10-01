using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cannon;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float fireDelay = 4f;
    [SerializeField] private float aimThreshold = 5f;

    private Transform target;
    private float fireCooldown;
    private float startDelayTimer = 0;

    private void Update()
    {
        if (target != null)
        {
            AimAtTarget();
            HandleShooting();
        }
    }

    private void AimAtTarget()
    {
        Vector2 dir = target.position - cannon.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        cannon.rotation = Quaternion.RotateTowards(cannon.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleShooting()
    {
        if (target != null && target.TryGetComponent(out Lander lander))
        {
            if (lander.GetCurrentState() == Lander.State.WaitingToStart)
            {
                startDelayTimer = 0f;
                return;
            }

            startDelayTimer += Time.deltaTime;
            if (startDelayTimer < 2f) return;
        }
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f && IsAimedAtTarget())
        {
            Shoot();
            fireCooldown = fireDelay;
        }
    }

    private bool IsAimedAtTarget()
    {
        if (target == null) return false;

        Vector2 cannonDir = cannon.up;
        Vector2 toTarget = (target.position - cannon.position).normalized;

        float angleDiff = Vector2.Angle(cannonDir, toTarget);
        return angleDiff <= aimThreshold;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Collider2D[] turretCols = GetComponentsInChildren<Collider2D>();
        foreach (var col in turretCols)
        {
            Physics2D.IgnoreCollision(bulletCol, col);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget(Transform oldTarget)
    {
        if (target == oldTarget) target = null;
    }
}
