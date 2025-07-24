using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;
using SFB;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PrefabSaveLoadManager : MonoBehaviour
{
    [Header("配置")]
    public Transform parentTransform;             // 所有实例的父物体
    public List<GameObject> prefabTypes;          // 所有可实例化的预制件类型
    public Button saveButton;
    public Button loadButton;

    [Header("OSC 发送器引用")]
    public GameObject externalObjectWithSender;

    // private string saveFolder => Path.Combine(Application.dataPath, "../SavedData");
    // private string savePath => Path.Combine(saveFolder, "data.json");

    [Serializable]
    public class PrefabData
    {
        public string prefabTypeName;

        public bool hasInputField;
        public string inputText;

        public bool hasIntValue;
        public int intValue;

        public bool hasControl;
        public string controlType;
        public float floatValue;
        public bool boolValue;
    }

    [Serializable]
    public class SaveData
    {
        public List<PrefabData> objects = new List<PrefabData>();
    }

    void Start()
    {
        if (saveButton != null)
            saveButton.onClick.AddListener(SaveDataToFile);
        if (loadButton != null)
            loadButton.onClick.AddListener(LoadDataFromFile);
    }

    /* 旧版保存方法
        public void SaveDataToFile()
        {
            var data = new SaveData();

            foreach (Transform child in parentTransform)
            {
                var comp = child.GetComponent<OscCore.PropertyOutputCombined>();
                if (comp != null)
                {
                    var entry = new PrefabData
                    {
                        prefabTypeName = child.name.Replace("(Clone)", "").Trim()
                    };

                    if (comp.HasInputField())
                    {
                        entry.hasInputField = true;
                        entry.inputText = comp.GetInputText();
                    }

                    if (comp.HasIntValue())
                    {
                        entry.hasIntValue = true;
                        entry.intValue = comp.GetIntValue();
                    }

                    if (comp.HasControl())
                    {
                        entry.hasControl = true;
                        entry.controlType = comp.GetControlType();
                        entry.floatValue = comp.GetSliderValue();
                        entry.boolValue = comp.GetToggleValue();
                    }

                    data.objects.Add(entry);
                }
            }

            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);

            Debug.Log("保存成功至: " + savePath);
    #if UNITY_EDITOR
            AssetDatabase.Refresh();
    #endif
        }
    */

    public void SaveDataToFile()
    {
        var extensions = new[] { new ExtensionFilter("Export As Json File", "json") };

        string path = StandaloneFileBrowser.SaveFilePanel("Saving Hierarchy", "", "data", extensions);
        if (string.IsNullOrEmpty(path)) return;

        var data = new SaveData();

        foreach (Transform child in parentTransform)
        {
            var comp = child.GetComponent<OscCore.PropertyOutputCombined>();
            if (comp != null)
            {
                var entry = new PrefabData
                {
                    prefabTypeName = child.name.Replace("(Clone)", "").Trim()
                };

                if (comp.HasInputField())
                {
                    entry.hasInputField = true;
                    entry.inputText = comp.GetInputText();
                }

                if (comp.HasIntValue())
                {
                    entry.hasIntValue = true;
                    entry.intValue = comp.GetIntValue();
                }

                if (comp.HasControl())
                {
                    entry.hasControl = true;
                    entry.controlType = comp.GetControlType();
                    entry.floatValue = comp.GetSliderValue();
                    entry.boolValue = comp.GetToggleValue();
                }

                data.objects.Add(entry);
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("保存成功至: " + path);
    }

    /* 旧版加载方法
        public void LoadDataFromFile()
        {
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("未找到保存文件：" + savePath);
                return;
            }

            // 清除旧实例
            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
    #if UNITY_EDITOR
                DestroyImmediate(parentTransform.GetChild(i).gameObject);
    #else
                Destroy(parentTransform.GetChild(i).gameObject);
    #endif
            }

            string json = File.ReadAllText(savePath);
            var data = JsonUtility.FromJson<SaveData>(json);

            foreach (var item in data.objects)
            {
                GameObject prefab = prefabTypes.Find(p => p.name == item.prefabTypeName);
                if (prefab == null)
                {
                    Debug.LogWarning($"未找到匹配的预制体类型: {item.prefabTypeName}");
                    continue;
                }

                GameObject instance = Instantiate(prefab, parentTransform);
                var comp = instance.GetComponent<OscCore.PropertyOutputCombined>();
                if (comp != null)
                {
                    if (externalObjectWithSender != null)
                        comp.m_Sender = externalObjectWithSender.GetComponent<OscCore.OscSender>();

                    if (item.hasInputField)
                        comp.SetInputText(item.inputText);

                    if (item.hasIntValue)
                        comp.SetIntValue(item.intValue);

                    if (item.hasControl)
                        comp.ApplyControlValue(item.controlType, item.floatValue, item.boolValue);
                }
            }

            Debug.Log("加载完成！");
        }
    */

    public void LoadDataFromFile()
    {
        var extensions = new[] { new ExtensionFilter("Import Saved Json File", "json") };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Loading Hierarchy", "", extensions, false);
        if (paths.Length == 0 || string.IsNullOrEmpty(paths[0])) return;

        string path = paths[0];
        if (!File.Exists(path))
        {
            Debug.LogWarning("未找到保存文件：" + path);
            return;
        }

        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(parentTransform.GetChild(i).gameObject);
        }

        string json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);

        foreach (var item in data.objects)
        {
            GameObject prefab = prefabTypes.Find(p => p.name == item.prefabTypeName);
            if (prefab == null)
            {
                Debug.LogWarning($"未找到匹配的预制体类型: {item.prefabTypeName}");
                continue;
            }

            GameObject instance = Instantiate(prefab, parentTransform);
            var comp = instance.GetComponent<OscCore.PropertyOutputCombined>();
            if (comp != null)
            {
                if (externalObjectWithSender != null)
                    comp.m_Sender = externalObjectWithSender.GetComponent<OscCore.OscSender>();

                if (item.hasInputField)
                    comp.SetInputText(item.inputText);

                if (item.hasIntValue)
                    comp.SetIntValue(item.intValue);

                if (item.hasControl)
                    comp.ApplyControlValue(item.controlType, item.floatValue, item.boolValue);
            }
        }

        Debug.Log("加载完成！");
    }
}
