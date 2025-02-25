using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomObjectChooser : MonoBehaviour
{
    [SerializeField] List<RObject> randomObjects;
    List<int> probabilities;
    int probSum;
    int _seedAdd;

    private void Awake()
    {
        probabilities = new();
        var probability = 0;

        for (int i = 0; i < randomObjects.Count; i++)
        {
            probabilities.Add(probability);
            probability += randomObjects[i].probability;
        }
        probabilities.Add(probability);

        probSum = probability;
    }

    public GameObject Choose()
    {
        Random.InitState(System.DateTime.Now.Millisecond + _seedAdd);
        _seedAdd++;
        var p = Random.value * probSum;

        for (int i = 0; i < probabilities.Count - 1; i++)
        {
            if (probabilities[i] < p && p <= probabilities[i + 1])
            {
                return randomObjects[i].prefab;
            }
        }

        Debug.Log("¶¬‚·‚éáŠQ•¨‚ð’Š‘I‚Å‚«‚Ü‚¹‚ñ‚Å‚µ‚½");
        return null;
    }
}

[System.Serializable]
public struct RObject
{
    public GameObject prefab;
    public int probability;
}
