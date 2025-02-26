using System.Linq;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI[] _rankingText;

    public void UpdateResultText(int score, int[] ranking)
    {
        _scoreText.text = score.ToString() + "m";

        for (int i = 0; i < _rankingText.Length; i++)
        {
            if (i < ranking.Length)
            {
                _rankingText[i].text = $"{i+1}位 {ranking[i]}m";
                
            }
            else
            {
                _rankingText[i].text = "";
            }
        }
    }
}
