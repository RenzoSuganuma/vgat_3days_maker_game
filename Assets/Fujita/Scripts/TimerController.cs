using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimerController : MonoBehaviour
{
    // 開始前待機時間
    // リミットタイマー

    private float _currentTime;
    private float time;
    void Timer()
    {
        time-=Time.deltaTime;
        // ここのコメントアウトは後で消してくださいMissionsDisplay.SetTimeScore(time);
    }

    private void StartTimer()
    {

    }

    void EndTimer()
    {

    }

    void UpdateTimer()
    {

    }

    void Update()
    {
        Timer();
    }
}
