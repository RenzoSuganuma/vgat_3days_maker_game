using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private MissionsDisplay _scorebord;
    [SerializeField] private TextMeshProUGUI _displayTime;
    private float _time = 120f;

    void Update()
    {
        _time -= Time.deltaTime;
        _scorebord.SetTimeScore(_time.ToString("c0"));
    }
}
