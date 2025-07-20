using UnityEngine;
using OscCore;

public class PrefabSpawner : MonoBehaviour
{
    [Header("要生成的预制件")]
    public GameObject prefab;

    [Header("生成后的父物体")]
    public Transform parent;

    [Header("外部引用的物体")]
    public GameObject externalObject;

    public void SpawnPrefab()
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.SetParent(parent, false);

        PropertyOutputCombined prefabPropertyOutput = instance.GetComponent<PropertyOutputCombined>();
        if (prefabPropertyOutput != null)
        {
            prefabPropertyOutput.m_Sender = externalObject.GetComponent<OscSender>();  // 手动赋值外部引用
        }
    }
}
