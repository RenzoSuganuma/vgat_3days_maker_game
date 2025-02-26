using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] GameObject player;

    private void Start()
    {
        distanceText.text = "0m";
    }

    private void Update()
    {
        if (FindAnyObjectByType<PlayerMove>() == null)
        {
            return;
        }

        float dis = Vector2.Distance(this.transform.position, player.transform.position);
        distanceText.text = "distance: " + (Mathf.RoundToInt(dis / 2f)) + "m";
    }
}
