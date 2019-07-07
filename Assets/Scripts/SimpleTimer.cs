using UnityEngine;
using System.Collections;

public class SimpleTimer : MonoBehaviour
{
    public float targetTime;
    public bool finished;

    void Update() {

        if (targetTime > 0.0f)
            targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            Finish(); 
        }
    }

    public void StartTimer(float duration) 
    {
        finished = false;
        targetTime = duration;
    }

    public void Finish()
    {
        finished = true;
        targetTime = -1.0f;
    }

    public float GetCurrentTime()
    {
        return targetTime;
    }

    public bool isFinished()
    {
        return finished;
    }
}
