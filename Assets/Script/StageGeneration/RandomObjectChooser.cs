using System.Collections.Generic;
using UnityEngine;

public class RandomObjectChooser : MonoBehaviour
{
    [SerializeField] List<RObject> randomObjects;
    List<int> weights; // 重みを加算していったもの。抽選時に使用する
    int weightSum;
    int _seedAdd;

    private void Awake()
    {
        weights = new();
        var probability = 0;

        for (int i = 0; i < randomObjects.Count; i++)
        {
            weights.Add(probability);
            probability += randomObjects[i].weight;
        }
        weights.Add(probability);

        weightSum = probability;
    }

    public RObject Choose()
    {
        Random.InitState(System.DateTime.Now.Millisecond + _seedAdd);
        _seedAdd++;
        var p = Random.value * weightSum;

        for (int i = 0; i < weights.Count - 1; i++)
        {
            // 抽選
            if (weights[i] < p && p <= weights[i + 1])
            {
                return randomObjects[i];
            }
        }

        Debug.Log("生成する障害物を抽選できませんでした");
        return default;
    }
}

[System.Serializable]
public class RObject
{
    public GameObject obj;
    public int weight;
    public bool mid;
}