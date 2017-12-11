using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public float Damage = 100f;

    public void Destroy()
    {
        Destroy(gameObject);
    }   
}
