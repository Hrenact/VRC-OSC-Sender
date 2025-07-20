using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OscCore
{
    [ExecuteInEditMode]
    [AddComponentMenu("OSC/Property Output (Simplified UI)", int.MaxValue)]
    public class PropertyOutput : MonoBehaviour
    {
        [SerializeField] public OscSender m_Sender;
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] string m_Address = "";
        [SerializeField] GameObject m_Object;

        float m_PreviousFloat;
        bool m_PreviousBool;

        void Update()
        {
            m_Address = m_InputField.text;

            if (m_Sender == null || m_Sender.Client == null || string.IsNullOrEmpty(m_Address))
                return;

            if (m_Object == null) return;

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
}
