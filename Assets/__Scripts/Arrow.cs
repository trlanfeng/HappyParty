using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{

    public List<Hero.Direction> arrowList;
    public List<Hero.Direction> tempList;
    public DiceManager sz;

    // Use this for initialization
    void Start()
    {
        sz = GameObject.Find("Dice").GetComponent<DiceManager>();
        isTriggered = false;
        GameObject panel = GameObject.Find("Canvas").transform.Find("Panel (1)").gameObject;
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(false);
        }
        tempList = new List<Hero.Direction>();
    }

    bool isTriggered;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hero" && !isTriggered)
        {
            tempList.Clear();
            for (int i = 0; i < arrowList.Count; i++)
            {
                tempList.Add(arrowList[i]);
            }
            isTriggered = true;
            sz.isPauseToSelect = true;
            switch (sz.hero.direction)
            {
                case Hero.Direction.up:
                    tempList.Remove(Hero.Direction.down);
                    break;
                case Hero.Direction.down:
                    tempList.Remove(Hero.Direction.up);
                    break;
                case Hero.Direction.left:
                    tempList.Remove(Hero.Direction.right);
                    break;
                case Hero.Direction.right:
                    tempList.Remove(Hero.Direction.left);
                    break;
            }
            if (sz.isPauseToSelect && tempList.Count > 1)
            {
                GameObject panel = GameObject.Find("Canvas").transform.Find("Panel (1)").gameObject;
                for (int i = 0; i < panel.transform.childCount; i++)
                {
                    panel.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < tempList.Count; i++)
                {
                    GameObject t = panel.transform.GetChild(i).gameObject;
                    t.SetActive(true);
                    t.transform.Find("Text").GetComponent<Text>().text = tempList[i].ToString();
                    int index = i;
                    t.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        sz.hero.direction = tempList[index];
                        sz.isPauseToSelect = false;
                        GameObject ttt = GameObject.Find("Canvas").transform.Find("Panel (1)").gameObject;
                        for (int j = 0; j < panel.transform.childCount; j++)
                        {
                            ttt.transform.GetChild(j).gameObject.SetActive(false);
                        }
                    });
                }
            }
            else if (sz.isPauseToSelect && tempList.Count == 1)
            {
                sz.hero.direction = tempList[0];
                sz.isPauseToSelect = false;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject panel = GameObject.Find("Canvas").transform.Find("Panel (1)").gameObject;
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(false);
        }
        tempList.Clear();
        for (int i = 0; i < arrowList.Count; i++)
        {
            tempList.Add(arrowList[i]);
        }
        isTriggered = false;
    }
}
