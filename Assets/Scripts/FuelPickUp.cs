using UnityEngine;

public class FuelPickUp : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
