using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float Damage = 30f;

    public void Destroy()
    {
        Destroy(gameObject);
    }   
}
