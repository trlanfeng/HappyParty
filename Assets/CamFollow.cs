using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour
{

    public Transform trans;

    public void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, trans.position, Time.deltaTime *3);
        //this.transform.LookAt(trans.parent.position);
    }
}
