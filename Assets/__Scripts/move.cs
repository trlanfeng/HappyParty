using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {
    
    public float speed;

    public enum Arrow
    {
        up,
        down,
        left,
        right
    }

    public Arrow arrowType;

    void Update()
    {
        switch (arrowType)
        {
            case Arrow.up:
                this.transform.eulerAngles = new Vector3(0, -90, 0);
                this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
                break;
            case Arrow.down:
                this.transform.eulerAngles = new Vector3(0, 90, 0);
                this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -speed);
                break;
            case Arrow.left:
                this.transform.eulerAngles = new Vector3(0, -180, 0);
                this.transform.GetComponent<Rigidbody>().velocity = new Vector3(-speed, 0, 0);
                break;
            case Arrow.right:
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                this.transform.GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0);
                break;
        }
    }
}
