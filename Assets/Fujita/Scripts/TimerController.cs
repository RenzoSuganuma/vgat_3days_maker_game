using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private MissionsDisplay _scorebord;
    [SerializeField] private TextMeshProUGUI _displayTime;
    private float _time;

    private void Start()
    {
        StageSettings settings = Resources.Load<GameSettings>("GameSettings").StageSettings;
        if (settings != null)
        {
            _time = settings.TimeLimit;
        }
    }

    void Update()
    {
        _time -= Time.deltaTime;
        _scorebord.SetTimeScore(_time.ToString("c0"));

        if (_time <= 0)
        {
            Foundation.NotifyGameOver();
        }
    }
}
