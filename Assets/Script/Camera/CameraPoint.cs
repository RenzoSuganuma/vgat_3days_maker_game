using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    [SerializeField] Transform _player;

    void Update()
    {
        transform.position = _player.position + new Vector3(5, -2, 0);
    }
}
