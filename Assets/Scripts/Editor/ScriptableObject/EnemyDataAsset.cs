using UnityEngine;
using UnityEditor;

public class EnemyDataAsset
{
    [MenuItem("Assets/Create/Enemy Data")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<EnemyData>();
    }
}
