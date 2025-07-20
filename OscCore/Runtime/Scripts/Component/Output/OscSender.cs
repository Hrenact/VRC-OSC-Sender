using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OscCore
{
    [ExecuteInEditMode]
    [AddComponentMenu("OSC/OSC Sender", int.MaxValue - 10)]
    public class OscSender : MonoBehaviour
    {
        [Tooltip("要发送到的IP地址")]
        [SerializeField] private string m_IpAddress = "127.0.0.1";

        [Tooltip("远程IP的端口")]
        [SerializeField] private int m_Port = 7000;


        public string IpAddress => m_IpAddress;
        public int Port => m_Port;

        public OscClient Client { get; private set; }

        // UI相关字段
        [Header("OSC 启用/禁用控制")]
        public bool IsOn = false; // 是否启用OSC发送
        public Color IsOnColor = Color.green; // 启用时的颜色
        public Color IsOffColor = Color.red; // 禁用时的颜色
        public Button ToggleButton; // 切换按钮

        [Header("OSC 地址和端口输入框")]
        public TMP_InputField IpInputField;   // 输入IP地址的TMP输入框
        public TMP_InputField PortInputField; // 输入端口的TMP输入框

        void OnValidate()
        {
            m_Port = m_Port.ClampPort();
        }

        public void ClientToggle()
        {
            IsOn = !IsOn;
            if (ToggleButton != null)
            {
                ToggleButton.image.color = IsOn ? IsOnColor : IsOffColor;
            }
        }

        void Update()
        {
            // 实时读取输入框内容并更新IP和端口
            if (IpInputField != null)
            {
                string ip = IpInputField.text.Trim();
                if (!string.IsNullOrEmpty(ip) && ip != m_IpAddress)
                {
                    m_IpAddress = ip;
                    // 地址变更后需要重建Client
                    if (Client != null)
                    {
                        Client = null;
                    }
                }
            }
            if (PortInputField != null)
            {
                if (int.TryParse(PortInputField.text, out int port) && port != m_Port)
                {
                    m_Port = port.ClampPort();
                    // 端口变更后需要重建Client
                    if (Client != null)
                    {
                        Client = null;
                    }
                }
            }

            if (IsOn)
            {
                // 已经有Client则不重复创建
                if (Client != null)
                {
                    return;
                }

                // 创建新的OscClient
                Client = new OscClient(m_IpAddress, m_Port);
                Debug.Log($"OscSender created client for {m_IpAddress}:{m_Port}");
            }
            else
            {
                // 禁用时释放Client
                if (Client != null)
                {
                    // 如果OscClient实现了IDisposable，可以在这里调用Dispose()
                    // (Client as IDisposable)?.Dispose();
                    Client = null;
                    Debug.Log("OscSender client released.");
                }
            }
        }
    }
}