using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    public Image img;

    float splashTimer = 0;
    void Update()
    {
        if (splashTimer < 1)
        {
            img.sprite = Resources.Load<Sprite>("Images/Splash/logo1");
            img.color = new Color(1, 1, 1, splashTimer);
        }
        else if (splashTimer > 1 && splashTimer < 3)
        {
            img.color = new Color(1, 1, 1, 3 - splashTimer);
        }
        else if (splashTimer > 3 && splashTimer < 4)
        {
            img.sprite = Resources.Load<Sprite>("Images/Splash/logo2");
            img.color = new Color(1, 1, 1, splashTimer - 3);
        }
        else if (splashTimer > 4 && splashTimer < 6)
        {
            img.color = new Color(1, 1, 1, 6 - splashTimer);
        }
        else
        {
            Application.LoadLevel("Hall");
        }
        splashTimer += Time.deltaTime;
    }
}
