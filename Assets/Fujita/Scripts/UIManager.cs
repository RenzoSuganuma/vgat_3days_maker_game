using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Header("読むテキスト")]
    public List<string> words = new List<string>();

    private int wordCount;



    void Start()
    {
        SetUp();
    }

    void Update()
    {
        if(time>0)
            Timer();
    }

    //初期化
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

    //WordsTextを使えば読むテキストをランダムに表示されます
    public void WordsText()
    {
        wordCount = words.Count;
        int i=Random.Range(0, wordCount);
        nextWordsText.text = words[i];
    }
}
