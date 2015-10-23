using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{

    public enum MapState
    {
        IDLE,
        DICE,
        MOVE,
        CHANGE,
        GAME
    }

    public MapState mapState;

    List<Charactor> ListCharactor;
    int currentActorIndex;

    public com_path path;

    public int moveStep = 0;
    public int endPathIndex = 0;
    public int curPathIndex = 0;

    public int moveSpeed = 10;

    public GameObject camera;

    // Use this for initialization
    void Start()
    {
        currentActorIndex = 0;
        ListCharactor = new List<Charactor>();
        ListCharactor.Add(new Charactor());
        ListCharactor[0].GameObject = GameObject.Find("Capsule");

        Vector3 pos = path.transform.GetChild(0).position;
        ListCharactor[currentActorIndex].GameObject.transform.position = new Vector3(pos.x, ListCharactor[currentActorIndex].GameObject.transform.position.y, pos.z);
        doWorldCamera = true;
        doPlayerCamera = false;
    }

    float runTime = 0;

    // Update is called once per frame
    void Update()
    {
        switch (mapState)
        {
            case MapState.IDLE:
                mapState = MapState.DICE;
                break;
            case MapState.DICE:
                //见GUI方法
                break;
            case MapState.MOVE:
                moveCheck();
                break;
            case MapState.CHANGE:
                break;
            case MapState.GAME:
                break;
        }
        if (doPlayerCamera)
        {
            CameraToPlayer();
        }
        if (doWorldCamera)
        {
            CameraToWorld();
        }
    }

    private void moveCheck()
    {
        if (moveStep > 0 && isBegin && !isEnd)
        {
            Vector3 pathpos = path.GetPos(0.011f);
            Vector3 newpos = new Vector3(pathpos.x, ListCharactor[currentActorIndex].GameObject.transform.position.y, pathpos.z);
            ListCharactor[currentActorIndex].GameObject.transform.position = Vector3.Lerp(ListCharactor[currentActorIndex].GameObject.transform.position, newpos, Time.deltaTime * moveSpeed);
            //this.transform.position = newpos;
            Vector3 pathpos2 = path.GetPos(Time.deltaTime * 8);
            Vector3 lookatpos = new Vector3(pathpos2.x, ListCharactor[currentActorIndex].GameObject.transform.position.y, pathpos2.z);
            ListCharactor[currentActorIndex].GameObject.transform.LookAt(lookatpos);
        }
        checkEnd();
    }

    bool isBegin = false;
    bool isEnd = true;

    void OnGUI()
    {
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
        if (mapState != MapState.DICE)
        {
            return;
        }
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
            endPathIndex = path.transform.childCount - 1;
            checkLastStep(curPathIndex, endPathIndex, moveStep);
        }
        isBegin = true;
        path.SetPath(curPathIndex, endPathIndex);
        endpos = path.endPos;
        endpos = new Vector3(endpos.x, ListCharactor[currentActorIndex].GameObject.transform.position.y, endpos.z);
        mapState = MapState.MOVE;
    }

    void checkLastStep(int b, int e, int m)
    {
        int mCount = 0;
        for (int i = b + 1; i <= e; i++)
        {
            if (path.transform.GetChild(i).gameObject.name.IndexOf('b') != 0)
            {
                mCount++;
            }
        }
        lastStep = m - mCount;
    }

    Vector3 endpos;
    void checkEnd()
    {
        if (path.curLocalPos == path._pathpoints[path._pathpoints.Count - 1])
        {
            if ((Mathf.Abs(ListCharactor[currentActorIndex].GameObject.transform.position.x - endpos.x) < 0.01f && Mathf.Abs(ListCharactor[currentActorIndex].GameObject.transform.position.z - endpos.z) < 0.01f))
            {
                //Debug.Log("已到达终点");
                isEnd = true;
                isBegin = false;
                onMoveComplete();
            }
            else
            {
                ListCharactor[currentActorIndex].GameObject.transform.position = Vector3.Lerp(ListCharactor[currentActorIndex].GameObject.transform.position, endpos, Time.deltaTime * 5);
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
        camera.transform.position = Vector3.Lerp(camera.transform.position, ListCharactor[currentActorIndex].GameObject.transform.Find("LookPosition").position, Time.deltaTime * 3);
        camera.transform.LookAt(ListCharactor[currentActorIndex].GameObject.transform.Find("Cube").position);
    }

    bool isWorldCameraReady = false;
    void CameraToWorld()
    {
        Vector3 newpos = new Vector3(ListCharactor[currentActorIndex].GameObject.transform.position.x, 25, ListCharactor[currentActorIndex].GameObject.transform.position.z + 16);
        if (isWorldCameraReady)
        {
            camera.transform.position = newpos;
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, newpos, Time.deltaTime * 3);
            if (Mathf.Abs(ListCharactor[currentActorIndex].GameObject.transform.position.x - camera.transform.position.x) < 0.1f && Mathf.Abs(ListCharactor[currentActorIndex].GameObject.transform.position.z + 16 - camera.transform.position.z) < 0.1f)
            {
                isWorldCameraReady = true;
            }
        }
        camera.transform.LookAt(ListCharactor[currentActorIndex].GameObject.transform.Find("Cube").position);
    }

    bool getTransfer = false;
    com_path transferPath;
    com_pathpoint transferPoint;
    int transferDir;
    void checkTransfer()
    {
        Debug.Log("执行了");
        Debug.Log("getTransfer:::"+ getTransfer);
        Debug.Log("lastStep:::" + lastStep);
        if (lastStep > 0)
        {
            com_pathpoint cp = path.transform.GetChild(endPathIndex).GetComponent<com_pathpoint>();
            if (cp.isTransfer)
            {
                getTransfer = true;
                transferPath = cp.targetPath;
                transferPoint = cp.targetPoint;
                transferDir = cp.targetDir;
                curPathIndex = transferPoint.gameObject.transform.GetSiblingIndex();
            }
            if (getTransfer)
            {
                path = transferPath;
                curPathIndex = transferPoint.transform.GetSiblingIndex();
                moveStep = lastStep;
                Debug.Log("剩余步数：" + moveStep);
                commonFunction(moveStep);
                getTransfer = false;
                lastStep = 0;
            }
            else
            {
                Debug.Log("已到达终点，没有切换点。");
            }
        }
        else
        {
            mapState = MapState.IDLE;
        }
    }

}
