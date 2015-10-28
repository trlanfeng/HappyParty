using UnityEngine;
using System.Collections;

public class press : MonoBehaviour {

    Rigidbody2D r2d;

	// Use this for initialization
	void Start () {
        r2d = transform.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            r2d.AddForce(new Vector2(-10, 30));
        }
        if (Input.GetMouseButtonDown(1))
        {
            r2d.AddForce(new Vector2(10, 30));
        }
        if (r2d.velocity.x > 10)
        {
            r2d.velocity = new Vector2(10, r2d.velocity.y);
        }
        if (r2d.velocity.y > 30)
        {
            r2d.velocity = new Vector2(r2d.velocity.x,30);
        }
	}
}
