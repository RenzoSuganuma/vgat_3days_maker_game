using UnityEngine;
using TMPro;

public class HeightText : MonoBehaviour
{
    [SerializeField] private MissionsDisplay _scorebord;
    [SerializeField] private TextMeshProUGUI _displayHeight;
    private float _height = 0f;

    void Start()
    {

    }

    void Update()
    {
        _height = DestinatinCheck._currentLaneIndex * 10 + 10;
        _scorebord.SetHeightScore(_height);
    }
}
