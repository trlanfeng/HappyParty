using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.forward * 10;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.GetComponent<Rigidbody>().velocity = (Vector3.back * 10);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.GetComponent<Rigidbody>().velocity = (Vector3.left * 10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.GetComponent<Rigidbody>().velocity = (Vector3.right * 10);
        }
    }
}
