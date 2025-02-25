using UnityEngine;

public class TestObstacle : MonoBehaviour
{
    [SerializeField] float _speed = 10;

    void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }
}
