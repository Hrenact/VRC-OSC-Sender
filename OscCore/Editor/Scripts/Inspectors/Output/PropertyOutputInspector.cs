// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using UnityEditor;
// using UnityEngine;

// namespace OscCore
// {
//     [CustomEditor(typeof(PropertyOutput), true)]
//     public class PropertyOutputInspector : Editor
//     {
//         static readonly GUIContent k_ComponentContent = new GUIContent("Component", "UI 组件，例如 Slider、Toggle、Text");
//         static readonly GUIContent k_PropertyContent = new GUIContent("Property", "字段或属性名称");

//         static readonly HashSet<string> k_SupportedTypes = new HashSet<string>()
//         {
//             "System.Single",   // float (Slider.value)
//             "System.Boolean",  // bool (Toggle.isOn)
//             "System.String",   // string (Text.text)
//         };

//         SerializedProperty m_AddressProp;
//         SerializedProperty m_SenderProp;
//         SerializedProperty m_ObjectProp;
//         SerializedProperty m_SourceComponentProp;
//         SerializedProperty m_PropertyNameProp;
//         SerializedProperty m_PropertyTypeNameProp;

//         PropertyOutput m_Target;
//         Component[] m_CachedComponents;
//         string[] m_ComponentNames;
//         int m_ComponentIndex;

//         MemberInfo[] m_Members;
//         string[] m_MemberNames;
//         int m_MemberIndex;

//         void OnEnable()
//         {
//             m_Target = (PropertyOutput)target;

//             m_AddressProp = serializedObject.FindProperty("m_Address");
//             m_SenderProp = serializedObject.FindProperty("m_Sender");
//             m_ObjectProp = serializedObject.FindProperty("m_Object");
//             m_SourceComponentProp = serializedObject.FindProperty("m_SourceComponent");
//             m_PropertyNameProp = serializedObject.FindProperty("m_PropertyName");
//             m_PropertyTypeNameProp = serializedObject.FindProperty("m_PropertyTypeName");

//             RefreshComponentList();
//         }

//         public override void OnInspectorGUI()
//         {
//             serializedObject.Update();

//             EditorGUILayout.PropertyField(m_SenderProp);
//             EditorGUILayout.PropertyField(m_AddressProp);
//             EditorGUILayout.PropertyField(m_ObjectProp);

//             if (m_ObjectProp.objectReferenceValue != null)
//             {
//                 DrawComponentDropdown();
//                 DrawPropertyDropdown();
//             }

//             serializedObject.ApplyModifiedProperties();
//         }

//         void RefreshComponentList()
//         {
//             if (m_Target == null || m_Target.gameObject == null) return;

//             m_CachedComponents = m_Target.gameObject.GetComponents<Component>();
//             m_ComponentNames = m_CachedComponents.Select(c => c.GetType().Name).ToArray();

//             var currentComp = m_SourceComponentProp.objectReferenceValue as Component;
//             m_ComponentIndex = Array.IndexOf(m_CachedComponents, currentComp);

//             if (m_ComponentIndex >= 0)
//                 RefreshPropertyList(m_CachedComponents[m_ComponentIndex]);
//         }

//         void DrawComponentDropdown()
//         {
//             int newIndex = EditorGUILayout.Popup(k_ComponentContent, m_ComponentIndex, m_ComponentNames);

//             if (newIndex != m_ComponentIndex)
//             {
//                 m_ComponentIndex = newIndex;
//                 m_SourceComponentProp.objectReferenceValue = m_CachedComponents[m_ComponentIndex];
//                 serializedObject.ApplyModifiedProperties();
//                 m_Target.SetPropertyFromSerialized();

//                 RefreshPropertyList(m_CachedComponents[m_ComponentIndex]);
//                 m_MemberIndex = -1;
//             }
//         }

//         void DrawPropertyDropdown()
//         {
//             if (m_MemberNames == null || m_MemberNames.Length == 0) return;

//             int newIndex = EditorGUILayout.Popup(k_PropertyContent, m_MemberIndex, m_MemberNames);
//             if (newIndex != m_MemberIndex)
//             {
//                 m_MemberIndex = newIndex;
//                 var selectedMember = m_Members[m_MemberIndex];
//                 m_PropertyNameProp.stringValue = selectedMember.Name;

//                 Type memberType = null;
//                 if (selectedMember is PropertyInfo propInfo)
//                     memberType = propInfo.PropertyType;
//                 else if (selectedMember is FieldInfo fieldInfo)
//                     memberType = fieldInfo.FieldType;

//                 m_PropertyTypeNameProp.stringValue = memberType?.Name;
//             }

//             if (m_MemberIndex >= 0)
//             {
//                 using (new EditorGUI.IndentLevelScope())
//                 {
//                     EditorGUILayout.LabelField("Type", m_PropertyTypeNameProp.stringValue);
//                 }
//             }
//         }

//         void RefreshPropertyList(Component component)
//         {
//             var type = component.GetType();
//             var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
//                 .Where(p => k_SupportedTypes.Contains(p.PropertyType.FullName) && p.CanRead)
//                 .Cast<MemberInfo>();

//             var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public)
//                 .Where(f => k_SupportedTypes.Contains(f.FieldType.FullName))
//                 .Cast<MemberInfo>();

//             m_Members = props.Concat(fields).OrderBy(m => m.Name).ToArray();
//             m_MemberNames = m_Members.Select(m => m.Name).ToArray();
//         }
//     }
// }
