using UnityEngine;
using System.Collections;

public class com_pathpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if(HideInGame)
        {
            this.gameObject.SetActive(false);
        }
	}
    
    public bool BezierControlPoint = false;
    public bool HideInGame = true;
    public int BezierElement = 10;

    public bool isTransfer = false;
    public com_path targetPath;
    public com_pathpoint targetPoint;
    public int targetDir = 1;
}
