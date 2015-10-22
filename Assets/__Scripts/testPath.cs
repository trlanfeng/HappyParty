using UnityEngine;
using System.Collections;

public class testPath : MonoBehaviour
{

    public com_path path;

    public int moveStep = 0;
    public int endPathIndex = 0;
    public int curPathIndex = 0;

    public int moveSpeed = 10;

    public GameObject camera;

    // Use this for initialization
    void Start()
    {
        Vector3 pos = path.transform.GetChild(0).position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        doWorldCamera = true;
        doPlayerCamera = false;
    }

    float runTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (moveStep > 0 && isBegin && !isEnd)
        {
            Vector3 pathpos = path.GetPos(0.011f);
            Vector3 newpos = new Vector3(pathpos.x, transform.position.y, pathpos.z);
            this.transform.position = Vector3.Lerp(transform.position, newpos, Time.deltaTime * moveSpeed);
            //this.transform.position = newpos;
            Vector3 pathpos2 = path.GetPos(Time.deltaTime * 8);
            Vector3 lookatpos = new Vector3(pathpos2.x, transform.position.y, pathpos2.z);
            this.transform.LookAt(lookatpos);
        }
        checkEnd();
        if (doPlayerCamera)
        {
            CameraToPlayer();
        }
        if (doWorldCamera)
        {
            CameraToWorld();
        }
    }

    bool isBegin = false;
    bool isEnd = true;

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "走一步"))
        {
            moveStep = 1;
            commonFunction(moveStep);
        }
        if (GUI.Button(new Rect(0, 60, 100, 50), "走二步"))
        {
            moveStep = 2;
            commonFunction(moveStep);
        }
        if (GUI.Button(new Rect(0, 120, 100, 50), "走三步"))
        {
            moveStep = 3;
            commonFunction(moveStep);
        }
        if (GUI.Button(new Rect(0, 180, 100, 50), "走四步"))
        {
            moveStep = 4;
            commonFunction(moveStep);
        }
        if (GUI.Button(new Rect(0, 240, 100, 50), "走五步"))
        {
            moveStep = 5;
            commonFunction(moveStep);
        }
        if (GUI.Button(new Rect(0, 300, 100, 50), "走六步"))
        {
            moveStep = 6;
            commonFunction(moveStep);
        }
        if (GUI.Button(new Rect(120, 0, 100, 50), "看玩家"))
        {
            doPlayerCamera = true;
            doWorldCamera = false;
        }
        if (GUI.Button(new Rect(120, 60, 100, 50), "看地图"))
        {
            doWorldCamera = true;
            doPlayerCamera = false;
        }
    }
    int lastStep = 0;
    void commonFunction(int moveStep)
    {
        if (!isEnd)
        {
            return;
        }
        endPathIndex = curPathIndex + moveStep;
        if (endPathIndex > path.transform.childCount - 1)
        {
            lastStep = endPathIndex - path.transform.childCount - 1;
            endPathIndex = path.transform.childCount - 1;
        }
        isBegin = true;
        path.SetPath(curPathIndex, endPathIndex);
        endpos = path.endPos;
        endpos = new Vector3(endpos.x, transform.position.y, endpos.z);
    }

    Vector3 endpos;
    void checkEnd()
    {
        if (path.curLocalPos == path._pathpoints[path._pathpoints.Count - 1])
        {
            if ((Mathf.Abs(transform.position.x - endpos.x) < 0.01f && Mathf.Abs(transform.position.z - endpos.z) < 0.01f))
            {
                //Debug.Log("已到达终点");
                isEnd = true;
                isBegin = false;
                onMoveComplete();
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, endpos, Time.deltaTime * 5);
            }
        }
        else
        {
            if (isBegin)
            {
                isEnd = false;
            }
        }
    }

    void onMoveComplete()
    {
        curPathIndex = endPathIndex + path.bPointCount;
        moveStep = 0;
        checkTransfer();
    }

    bool doPlayerCamera = false;
    bool doWorldCamera = false;
    void CameraToPlayer()
    {
        isWorldCameraReady = false;
        camera.transform.position = Vector3.Lerp(camera.transform.position, this.transform.Find("LookPosition").position, Time.deltaTime * 3);
        camera.transform.LookAt(this.transform.Find("Cube").position);
    }

    bool isWorldCameraReady = false;
    void CameraToWorld()
    {
        Vector3 newpos = new Vector3(transform.position.x, 25, transform.position.z + 16);
        if (isWorldCameraReady)
        {
            camera.transform.position = newpos;
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, newpos, Time.deltaTime * 3);
            if (Mathf.Abs(transform.position.x - camera.transform.position.x) < 0.1f && Mathf.Abs(transform.position.z + 16 - camera.transform.position.z) < 0.1f)
            {
                isWorldCameraReady = true;
            }
        }
        camera.transform.LookAt(this.transform.Find("Cube").position);
    }

    bool getTransfer = false;
    com_path transferPath;
    com_pathpoint transferPoint;
    int transferDir;
    void checkTransfer()
    {
        if (lastStep > 0 && getTransfer)
        {
            path = transferPath;
            curPathIndex = transferPoint.transform.GetSiblingIndex();
            moveStep = lastStep;
            commonFunction(moveStep);
            getTransfer = false;
            lastStep = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger in");
        if (other.tag == "Transfer" && other.gameObject.GetComponent<com_pathpoint>().isTransfer)
        {
            Debug.Log("trigger on");
            com_pathpoint cp = other.gameObject.GetComponent<com_pathpoint>();
            getTransfer = true;
            transferPath = cp.targetPath;
            transferPoint = cp.targetPoint;
            transferDir = cp.targetDir;
            curPathIndex = transferPoint.gameObject.transform.GetSiblingIndex();
        }
    }
}
