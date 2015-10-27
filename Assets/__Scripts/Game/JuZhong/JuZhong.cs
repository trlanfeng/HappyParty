using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JuZhong : MonoBehaviour
{

    public enum State
    {
        Ready,
        Running,
        LeftSlider,
        TopSlider,
        Result,
        End
    }

    public State state;

    GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        state = State.Ready;
    }

    Slider leftSlider;
    RectTransform leftSliderForegroundRT;

    float reduceSpeed;
    float percentOnClick;
    float needPower;
    float leftNeedStayTime;
    float leftTimer;

    Slider topSlider;
    float topLenght;
    float topSpeed;
    float topNeedPoint;
    float topCurPoint;
    float topFloatInterval;
    float topSliderWidth;
    RectTransform topPointerRT;

    void readyForGame()
    {
        leftSlider = canvas.transform.Find("leftSlider").GetComponent<Slider>();
        topSlider = canvas.transform.Find("topSlider").GetComponent<Slider>();
        leftSlider.value = 0;
        topSlider.value = 0;
        reduceSpeed = 5f;
        percentOnClick = 5f;
        needPower = 0.6f;
        leftSliderForegroundRT = leftSlider.transform.Find("Foreground").GetComponent<RectTransform>();
        leftSliderForegroundRT.offsetMin = new Vector2(leftSliderForegroundRT.offsetMin.x, leftSliderForegroundRT.rect.height * 0.6f);
        leftNeedStayTime = 3;
        leftTimer = 0;
        //topSlider相关部分初始化
        countDownTimer = 3;
        countDownText = topSlider.transform.Find("Timer").GetComponent<Text>();
        topSliderForegroundRT = topSlider.transform.Find("Foreground").GetComponent<RectTransform>();
        topPointerRT = topSlider.transform.Find("Pointer").GetComponent<RectTransform>();
        topSliderWidth = topSlider.transform.GetComponent<RectTransform>().rect.width;
        topLenght = 1f / 11f * topSliderWidth;
        topFloatInterval = topLenght / 2f;
        topSpeed = 1f;
        topNeedPoint = 0.5f;
        topCurPoint = -1;
        topSliderForegroundRT.offsetMin = new Vector2(topSliderWidth * topNeedPoint - topFloatInterval, topSliderForegroundRT.offsetMin.y);
        topSliderForegroundRT.offsetMax = new Vector2(-(topSliderWidth * (1 - topNeedPoint) - topFloatInterval), topSliderForegroundRT.offsetMax.y);
        //
        textResult = canvas.transform.Find("Text_Result").GetComponent<Text>();
    }

    void onLeftSlider()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (leftSlider.value >= 0 && leftSlider.value < 0.5f)
            {
                leftSlider.value += percentOnClick / 100f;
            }
            else if (leftSlider.value >= 0.5f && leftSlider.value < 0.8f)
            {
                leftSlider.value += percentOnClick / 120;
            }
            else if (leftSlider.value >= 0.8f)
            {
                leftSlider.value += percentOnClick / 150;
            }
        }
        leftSlider.value -= Time.deltaTime / reduceSpeed;
        if (leftSlider.value > needPower)
        {
            leftTimer += Time.deltaTime;
        }
        else
        {
            leftTimer = 0;
        }
        if (leftTimer > leftNeedStayTime)
        {
            state = State.TopSlider;
        }
    }

    float countDownTimer;
    float topSliderTimer;
    RectTransform topSliderForegroundRT;
    Text countDownText;
    void onTopSlider()
    {
        topSliderTimer += Time.deltaTime;
        if (topSliderTimer >= 0 && topSliderTimer < 1 / topSpeed)
        {
            topSlider.value += Time.deltaTime * topSpeed;
        }
        else if (topSliderTimer >= 1 / topSpeed && topSliderTimer < 2 / topSpeed)
        {
            topSlider.value -= Time.deltaTime * topSpeed;
        }
        else
        {
            topSliderTimer = 0;
        }
        if (countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            countDownText.text = countDownTimer.ToString();
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            topCurPoint = topSlider.value;
        }
        if (topCurPoint >= topNeedPoint - 1/22f && topCurPoint <= topNeedPoint + 1/22f)
        {
            resultDes = "成功！";
            state = State.Result;
        }
        else if (topCurPoint < 1/11f || topCurPoint > 10/11f)
        {
            if (topCurPoint != -1)
            {
                resultDes = "失败！";
                state = State.Result;
            }
        }
        else
        {
            if (topCurPoint > 0)
            {
                topPointerRT.anchoredPosition = new Vector2(topSliderWidth * topCurPoint, topPointerRT.anchoredPosition.y);
                topNeedPoint = topNeedPoint * 2 - topCurPoint;
                if (topNeedPoint < 1 / 11f || topNeedPoint > 10 / 11f)
                {
                    resultDes = "失败！";
                    state = State.Result;
                    return;
                }
                topSliderForegroundRT.offsetMin = new Vector2(topSliderWidth * topNeedPoint - topFloatInterval, topSliderForegroundRT.offsetMin.y);
                topSliderForegroundRT.offsetMax = new Vector2(-(topSliderWidth * (1 - topNeedPoint) - topFloatInterval), topSliderForegroundRT.offsetMax.y);
                topCurPoint = -1;
            }
        }
    }

    Text textResult;
    string resultDes;
    void onResult()
    {
        textResult.text = resultDes;
    }

    void Update()
    {
        switch (state)
        {
            case State.Ready:
                readyForGame();
                state = State.Running;
                break;
            case State.Running:
                state = State.LeftSlider;
                break;
            case State.LeftSlider:
                onLeftSlider();
                break;
            case State.TopSlider:
                onTopSlider();
                break;
            case State.Result:
                onResult();
                break;
            case State.End:
                break;
            default:
                break;
        }
    }
}
