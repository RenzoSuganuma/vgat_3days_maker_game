using System.Collections;
using UnityEngine;

/// <summary>
/// レーン生成から独立して障害物を生成するクラス<br></br>
/// 同じ場所に何度生成されても問題ないオブジェクトに使用してください
/// </summary>
public class IndependenceObstacleGenerator : MonoBehaviour
{
    [SerializeField] GameObject _obstaclePref;
    [SerializeField, Tooltip("プレイヤーからどれだけ離して生成するか")] float _distance = 20;
    [SerializeField, Tooltip("レイヤー毎の生成間隔")] AnimationCurve[] _duration;
    [SerializeField] float _yOffset;

    public void StartGenerate(Transform playerPos, float height, int layer)
    {
        if (layer < _duration.Length)
        {
            StartCoroutine(Generate(playerPos, height, layer, _duration[layer].Evaluate(Random.value)));
        }
    }

    private IEnumerator Generate(Transform playerPos, float height, int layer, float t)
    {
        yield return new WaitForSeconds(t);
        var generatePos = new Vector3(playerPos.position.x + _distance, height + _yOffset, 0);
        Instantiate(_obstaclePref, generatePos, Quaternion.identity);

        StartCoroutine(Generate(playerPos, height, layer, _duration[layer].Evaluate(Random.value)));
    } 
}
