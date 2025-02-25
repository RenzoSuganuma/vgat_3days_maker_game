using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class GenerateSettings : ScriptableObject
{
    public List<GenerateSetting> settings;

    public GameObject GetObject(int id)
    {
        var max = settings.Max(s => s.probability);
        
        
        return Random.value < prob;
    }
}

[System.Serializable]
public class GenerateSetting
{
    public GameObject gameObject;
    public int probability;
}
