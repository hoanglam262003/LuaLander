using UnityEngine;

public class TowerDetection : MonoBehaviour
{
    private Turret turret;

    private void Awake()
    {
        turret = GetComponentInParent<Turret>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Lander lander))
        {
            turret.SetTarget(lander.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Lander lander))
        {
            turret.ClearTarget(lander.transform);
        }
    }
}