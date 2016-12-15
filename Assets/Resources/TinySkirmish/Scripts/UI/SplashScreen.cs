using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public string NextLevel;
    public float Delay = 2;
    private float Timer = 0;

    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= Delay)
        {
            SceneManager.LoadScene(NextLevel);
            Timer = 0;
        }
    }
}
