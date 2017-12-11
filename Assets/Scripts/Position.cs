using UnityEngine;

public class Position : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.75f);
    }
}
