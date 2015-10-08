using UnityEngine;
using System.Collections;

public class test1 : MonoBehaviour {

    Vector3 pos;
    Quaternion q;
    Rigidbody R;
	// Use this for initialization
	void Start () {
        pos = this.transform.position;
        q = this.transform.rotation;
        R = this.transform.GetComponent<Rigidbody>();
        R.AddTorque(new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f)));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void reset()
    {
        this.transform.position = pos;
        this.transform.rotation = q;
        R.AddTorque(new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f)));
    }
}
