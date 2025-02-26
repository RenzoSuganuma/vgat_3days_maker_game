using TMPro;
using UnityEngine;

public class MissionsText : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMeshProUGUI;

    public TMP_Text MeshProUGUI
    {
        get => _textMeshProUGUI;
        set => _textMeshProUGUI = value;
    }
}
