using UnityEngine;
using System.Collections;

public class test2 : MonoBehaviour {

    public Transform target;
    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
