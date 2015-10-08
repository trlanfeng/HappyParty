using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class DiceManager : MonoBehaviour
{
    public Hero hero;

    public float speed;
    public Arrow FX;
    public float force;

    public bool isPauseToSelect;

    public enum Arrow
    {
        up,
        down,
        left,
        right
    }

    public Arrow arrowType;

    Rigidbody R;

    public Text text;
    public Text textPosition;
    bool isShow = true;

    // Use this for initialization
    void Start()
    {
        isPauseToSelect = false;
        R = this.GetComponent<Rigidbody>();
        arrowType = Arrow.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (R.velocity == Vector3.zero && !isShow)
        {
            showNumber();
        }
        if (R.velocity != Vector3.zero)
        {
            text.text = "等待结果...";
        }
    }

    public void rengShaiZi()
    {
        if (!isShow)
        {
            return;
        }
        R.velocity = Vector3.up;
        R.AddForce(Vector3.up * Random.Range(300, 500) * force);
        R.AddTorque(new Vector3(Random.Range(-360f, 360f) * force * 10, Random.Range(-360f, 360f) * force * 10, Random.Range(-360f, 360f) * force * 10));
        isShow = false;
    }

    void showNumber()
    {
        if (isPauseToSelect)
        {
            return;
        }
        isShow = true;
        Transform T = this.transform.Find("center");
        Vector3 startPos = new Vector3(T.position.x, T.position.y, T.position.z);
        Vector3 endPos = new Vector3(T.position.x, T.position.y + 0.6f, T.position.z);
        RaycastHit hit;
        Debug.DrawLine(startPos, endPos, Color.red);
        if (Physics.Linecast(startPos, endPos, out hit))
        {
            
        }
        else
        {
            return;
        }
        text.text = "你扔出了：" + hit.collider.gameObject.name;
        hero.moveStep = int.Parse(hit.collider.gameObject.name);
    }

    public void chongzhi()
    {
        hero.isMoving = false;
        hero.moveStep = 0;
        hero.transform.localPosition = new Vector3(1.5f, -5.5f, 0f);
    }
}
