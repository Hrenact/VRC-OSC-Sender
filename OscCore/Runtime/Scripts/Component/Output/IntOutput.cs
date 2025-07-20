using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OscCore
{
    [ExecuteInEditMode]
    [AddComponentMenu("OSC/Property Output (Int)", int.MaxValue)]
    public class PropertyOutputInspector : MonoBehaviour
    {
        [SerializeField] public OscSender m_Sender;
        [SerializeField] TMP_InputField m_InputField;
        [SerializeField] string m_Address = "";

        // OSC Debugger
        [Header("Int Only")]
        [SerializeField] public Button IntUpButton;
        [SerializeField] public Button IntDownButton;
        [SerializeField] public Text IntText;
        [SerializeField] int OSCD_Text_Int = 0;
        [HideInInspector] int OSCD_Int;

        void Update()
        {
            m_Address = "/avatar/parameters/" + m_InputField.text;
        }

        // ---------OSC Debugger---------

        public void IntUp()
        {
            OSCD_Int += 1;
            OSCD_Text_Int = OSCD_Int;
            IntText.text = OSCD_Text_Int.ToString();

            m_Sender.Client.Send(m_Address, OSCD_Int);
        }

        public void IntDown()
        {
            OSCD_Int -= 1;
            OSCD_Text_Int = OSCD_Int;
            IntText.text = OSCD_Text_Int.ToString();

            m_Sender.Client.Send(m_Address, OSCD_Int);
        }
    }
}
