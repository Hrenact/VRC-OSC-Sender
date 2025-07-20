using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OscCore
{
    [ExecuteInEditMode]
    [AddComponentMenu("OSC/OSCD Output", int.MaxValue)]
    public class PropertyOutputCombined : MonoBehaviour
    {
        // 获取当前 InputField 的文本内容
        public string GetInputText()
        {
            return m_InputField != null ? m_InputField.text : "";
        }

        // 设置 InputField 的文本内容
        public void SetInputText(string value)
        {
            if (m_InputField != null)
                m_InputField.text = value;
        }

        // 获取整数值
        public int GetIntValue()
        {
            return OSCD_Text_Int;
        }

        // 设置整数值
        public void SetIntValue(int value)
        {
            OSCD_Int = value;
            OSCD_Text_Int = value;
            if (IntText != null)
                IntText.text = value.ToString();
        }

        // ====== 保存字段检查 ======
        public bool HasInputField()
        {
            return m_InputField != null;
        }

        public bool HasIntValue()
        {
            return IntText != null;
        }

        public bool HasControl()
        {
            if (m_Object == null) return false;
            return m_Object.GetComponent<Slider>() != null || m_Object.GetComponent<Toggle>() != null;
        }

        // ====== 控件类型与值处理 ======
        public string GetControlType()
        {
            if (m_Object == null) return "None";
            if (m_Object.GetComponent<Slider>() != null) return "Slider";
            if (m_Object.GetComponent<Toggle>() != null) return "Toggle";
            return "None";
        }

        public float GetSliderValue()
        {
            var slider = m_Object?.GetComponent<Slider>();
            return slider != null ? slider.value : 0f;
        }

        public bool GetToggleValue()
        {
            var toggle = m_Object?.GetComponent<Toggle>();
            return toggle != null && toggle.isOn;
        }

        public void ApplyControlValue(string type, float floatVal, bool boolVal)
        {
            if (m_Object == null) return;

            if (type == "Slider")
            {
                var slider = m_Object.GetComponent<Slider>();
                if (slider != null)
                    slider.value = floatVal;
            }
            else if (type == "Toggle")
            {
                var toggle = m_Object.GetComponent<Toggle>();
                if (toggle != null)
                    toggle.isOn = boolVal;
            }
        }


        [Header("通用设置")]
        [SerializeField] public OscSender m_Sender;
        [SerializeField] public TMP_InputField m_InputField;
        [SerializeField] public string m_Address = "";

        [Header("简化 UI 输出")]
        [SerializeField] public GameObject m_Object;

        float m_PreviousFloat;
        bool m_PreviousBool;

        [Header("整数调试工具")]
        [SerializeField] public Button IntUpButton;
        [SerializeField] public Button IntDownButton;
        [SerializeField] public TMP_Text IntText;
        [SerializeField] int OSCD_Text_Int = 0;
        [HideInInspector] int OSCD_Int;

        void Start()
        {
            // 绑定按钮事件
            if (IntUpButton != null)
                IntUpButton.onClick.AddListener(IntUp);

            if (IntDownButton != null)
                IntDownButton.onClick.AddListener(IntDown);
        }

        void Update()
        {
            if (m_InputField != null)
                m_Address = m_InputField.text;

            if (m_Sender == null || m_Sender.Client == null || string.IsNullOrEmpty(m_Address))
                return;

            // 简化 UI 控件监控逻辑
            if (m_Object != null)
            {
                // 检查 Slider
                var slider = m_Object.GetComponent<Slider>();
                if (slider != null)
                {
                    float value = slider.value;
                    if (!Mathf.Approximately(value, m_PreviousFloat))
                    {
                        m_PreviousFloat = value;
                        m_Sender.Client.Send(m_Address, value);
                    }
                    return;
                }

                // 检查 Toggle
                var toggle = m_Object.GetComponent<Toggle>();
                if (toggle != null)
                {
                    bool isOn = toggle.isOn;
                    if (isOn != m_PreviousBool)
                    {
                        m_PreviousBool = isOn;
                        m_Sender.Client.Send(m_Address, isOn);
                    }
                    return;
                }
            }
        }

        // ----------整数调试逻辑----------
        public void IntUp()
        {
            OSCD_Int += 1;
            OSCD_Text_Int = OSCD_Int;
            if (IntText != null)
                IntText.text = OSCD_Text_Int.ToString();

            if (m_Sender != null && m_Sender.Client != null && !string.IsNullOrEmpty(m_Address))
                m_Sender.Client.Send(m_Address, OSCD_Int);
        }

        public void IntDown()
        {
            OSCD_Int -= 1;
            OSCD_Text_Int = OSCD_Int;
            if (IntText != null)
                IntText.text = OSCD_Text_Int.ToString();

            if (m_Sender != null && m_Sender.Client != null && !string.IsNullOrEmpty(m_Address))
                m_Sender.Client.Send(m_Address, OSCD_Int);
        }
    }
}
