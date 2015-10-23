using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class com_path : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //_curPos = _pathpoints[0];
        this.GetComponent<LineRenderer>().enabled = false;
    }
    public Vector3 startPos
    {
        get
        {
            return this.transform.localToWorldMatrix.MultiplyPoint(_pathpoints[0]);
        }
    }
    public Vector3 endPos
    {
        get
        {
            return this.transform.localToWorldMatrix.MultiplyPoint(_pathpoints[_pathpoints.Count - 1]);
        }
    }
    public Vector3 curPos
    {
        get
        {
            return this.transform.localToWorldMatrix.MultiplyPoint(_curPos);
        }
    }
    public Vector3 curLocalPos
    {
        get
        {
            return _curPos;
        }
    }

    public int PointCount
    {
        get
        {
            int count = 0;
            foreach (Transform item in transform)
            {
                if (item.gameObject.name.IndexOf('b') != 0)
                {
                    count++;
                }
            }
            return count;
        }
    }

    [SerializeField]
    Vector3 _curPos;
    [SerializeField]
    public int curIndex = 0;
    [SerializeField]
    public int curPoint = 0;
    public Vector3 GetPos(float adddist)
    {
        //Vector3.Distance()
        while (adddist > 0.01f)
        {
            bool bNext = false;
            Vector3 dir = _pathpoints[curIndex] - _curPos;
            float len = dir.magnitude;
            if (len <= 0.01f)
            {
                bNext = true;
            }
            float move = adddist;
            if (move > len)
            {
                move = len;
                bNext = true;
            }
            adddist -= move;
            Vector3 add = dir.normalized * move;
            _curPos += add;
            if (bNext)
            {
                if (pathType == Pathtype.Path_Loop)
                {
                    curIndex++;
                    if (curIndex >= _pathpoints.Count)
                    {
                        curIndex = 0;
                    }
                }
                else if (pathType == Pathtype.Path_Once)
                {
                    curIndex++;
                    if (curIndex >= _pathpoints.Count)
                    {
                        curIndex = _pathpoints.Count - 1;
                        isEnded = true;
                        break;
                    }
                }
                else if (pathType == Pathtype.Path_PingPang)
                {
                    curIndex += pathDir;
                    if (curIndex >= _pathpoints.Count || curIndex < 0)
                    {
                        pathDir *= -1;
                        curIndex += pathDir;
                    }
                }
            }
        }
        return curPos;
    }
    struct cpoint
    {
        public cpoint(Vector3 p, bool c)
        {
            point = p;
            control = c;
        }
        public Vector3 point;
        public bool control;
    }
    public List<Vector3> _pathpoints = new List<Vector3>();
    public enum Pathtype
    {
        Path_Once,
        Path_PingPang,
        Path_Loop,

    }
    public Pathtype pathType;
    [SerializeField]
    public int pathDir = 1;
    //public List<cpoint> _path = new List<cpoint>();
    // Update is called once per frame
    public void Reset()
    {
        _curPos = _pathpoints[0];
        curIndex = 0;
        pathDir = 1;
    }
    public void ResetCustom(Vector3 curPos, int index, int dir)
    {
        _curPos = curPos;
        curIndex = index;
        pathDir = dir;
    }
    void Update()
    {
        return;

    }

    public void ToggleShowCube()
    {
        foreach (Transform p in this.transform)
        {
            p.GetComponent<MeshRenderer>().enabled = !p.GetComponent<MeshRenderer>().enabled;
        }
    }

    public List<com_pathpoint> GetSourcePointList()
    {
        //寻找路径
        SortedList<int, com_pathpoint> points = new SortedList<int, com_pathpoint>();
        foreach (Transform p in this.transform)
        {
            string name = p.gameObject.name;
            if (name.IndexOf("p") == 0)
            {
                int pnum = int.Parse(name.Substring(1));
                points.Add(pnum, p.GetComponent<com_pathpoint>());
            }
        }
        List<com_pathpoint> _path = new List<com_pathpoint>();
        foreach (com_pathpoint p in points.Values)
        {
            _path.Add(p);
        }
        return _path;
    }
    public void UpdatePath()
    {
        //寻找路径
        SortedList<int, com_pathpoint> points = new SortedList<int, com_pathpoint>();
        int iii = 0;
        foreach (Transform p in this.transform)
        {
            points.Add(iii, p.GetComponent<com_pathpoint>());
            iii++;
        }
        List<com_pathpoint> _path = new List<com_pathpoint>();
        foreach (com_pathpoint p in points.Values)
        {
            _path.Add(p);
        }
        //计算线段
        _pathpoints.Clear();
        //List<Vector2> tpoints = new List<Vector2>();
        _pathpoints.Add(_path[0].transform.localPosition);

        for (int i = 1; i < _path.Count; i++)
        {
            if (_path[i].BezierControlPoint == false)//直线
            {
                _pathpoints.Add(_path[i].transform.localPosition);
            }
            else
            {
                if (_path[i + 1].BezierControlPoint == true)
                {
                    Debug.LogError("path 错误");
                }
                Vector3 p0 = _path[i - 1].transform.localPosition;
                Vector3 p1 = _path[i].transform.localPosition;
                Vector3 p2 = _path[i + 1].transform.localPosition;
                for (int e = 1; e <= _path[i].BezierElement; e++)
                {
                    float f = (float)e * (float)(1.0f / (float)_path[i].BezierElement);
                    Debug.Log("f=" + f);
                    _pathpoints.Add(CalculateBezierPoint(f, p0, p1, p2));
                }
                i++;
            }
        }
        //设置LineRenderer
        LineRenderer lrender = this.GetComponent<LineRenderer>();
        lrender.enabled = true;
        lrender.useWorldSpace = false;

        int pathcount = _pathpoints.Count;
        if (pathType == Pathtype.Path_Loop)
        {
            pathcount++;
        }
        lrender.SetVertexCount(pathcount);
        for (int i = 0; i < _pathpoints.Count; i++)
        {
            lrender.SetPosition(i, _pathpoints[i]);
        }
        if (pathType == Pathtype.Path_Loop)
        {
            lrender.SetPosition(pathcount - 1, _pathpoints[0]);
        }

        Reset();
    }
    public int bPointCount = 0;
    public void SetPath(int startIndex, int endIndex)
    {
        bPointCount = 0;
        //寻找路径
        SortedList<int, com_pathpoint> points = new SortedList<int, com_pathpoint>();

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (i >= transform.childCount)
            {
                break;
            }
            if (transform.GetChild(i).gameObject.name.IndexOf('b') == 0)
            {
                bPointCount++;
                endIndex++;
            }
            points.Add(i, transform.GetChild(i).GetComponent<com_pathpoint>());
        }
        List<com_pathpoint> _path = new List<com_pathpoint>();
        foreach (com_pathpoint p in points.Values)
        {
            _path.Add(p);
        }
        //计算线段
        _pathpoints.Clear();
        //List<Vector2> tpoints = new List<Vector2>();
        _pathpoints.Add(_path[0].transform.localPosition);

        for (int i = 1; i < _path.Count; i++)
        {
            if (_path[i].BezierControlPoint == false)//直线
            {
                _pathpoints.Add(_path[i].transform.localPosition);
            }
            else
            {
                if (_path[i + 1].BezierControlPoint == true)
                {
                    Debug.LogError("path 错误");
                }
                Vector3 p0 = _path[i - 1].transform.localPosition;
                Vector3 p1 = _path[i].transform.localPosition;
                Vector3 p2 = _path[i + 1].transform.localPosition;
                for (int e = 1; e <= _path[i].BezierElement; e++)
                {
                    float f = (float)e * (float)(1.0f / (float)_path[i].BezierElement);
                    _pathpoints.Add(CalculateBezierPoint(f, p0, p1, p2));
                }
                i++;
            }
        }
        //设置LineRenderer
        LineRenderer lrender = this.GetComponent<LineRenderer>();
        lrender.enabled = true;
        lrender.useWorldSpace = false;

        int pathcount = _pathpoints.Count;
        if (pathType == Pathtype.Path_Loop)
        {
            pathcount++;
        }
        lrender.SetVertexCount(pathcount);
        for (int i = 0; i < _pathpoints.Count; i++)
        {
            lrender.SetPosition(i, _pathpoints[i]);
        }
        if (pathType == Pathtype.Path_Loop)
        {
            lrender.SetPosition(pathcount - 1, _pathpoints[0]);
        }

        Reset();
    }

    public bool isEnded = false;
    public bool getIsEnded()
    {
        return false;
    }

    static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        p0 = (1 - t) * (1 - t) * p0;
        p1 = 2 * t * (1 - t) * p1;
        p2 = t * t * p2;
        return p0 + p1 + p2;
    }
    public void HidePath()
    {
        this.GetComponent<LineRenderer>().enabled = false;
    }

}
