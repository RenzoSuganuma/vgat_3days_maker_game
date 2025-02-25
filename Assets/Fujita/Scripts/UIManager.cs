using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //声の大きさ
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI decibelText;

    //次のセリフText
    [Header("Next wordsText")]
    [SerializeField] private TextMeshProUGUI nextWordsText;

    //スコアText
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI timeText;

    float time = 0f;

    void Start()
    {
        SetUp();
    }

    void Update()
    {
        Timer();
    }

    void SetUp()
    {
        decibelText.text = "0dB";
        nextWordsText.text = "Hello World!";
        distanceText.text = "0m";
        heightText.text = "0m";
        timeText.text = "0";
    }

    void Timer()
    {
        time+=Time.deltaTime;
        timeText.text = "Time:"+Mathf.RoundToInt(time);
    }
}
