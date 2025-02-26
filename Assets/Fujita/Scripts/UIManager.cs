using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    //次のセリフText
    [Header("Next wordsText")]
    [SerializeField] private TextMeshProUGUI nextWordsText;

    //スコアText
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("制限時間")]
    [SerializeField]float time = 50f;


    void Start()
    {
        SetUp();
    }

    void Update()
    {
        if(time>0)
            Timer();
    }

    void SetUp()
    {
        nextWordsText.text = " ";
        heightText.text = "0m";
        timeText.text = "0";
    }

    void Timer()
    {
        time-=Time.deltaTime;
        timeText.text = "Time:"+Mathf.RoundToInt(time);
    }

    public float GetTime()
    {
        return time;
    }
}
