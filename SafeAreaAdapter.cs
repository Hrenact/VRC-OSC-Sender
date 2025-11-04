using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaAdapter : MonoBehaviour
{
    private RectTransform panel;
    private Rect lastSafeArea = Rect.zero;
    private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;

    void Start()
    {
        panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        if (Screen.orientation != lastOrientation || Screen.safeArea != lastSafeArea)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;   // 当前安全区域（随旋转动态变化）

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        lastSafeArea = safeArea;
        lastOrientation = Screen.orientation;

        Debug.Log($"[SafeAreaAdapter] Applied safe area: {safeArea}, Orientation: {Screen.orientation}");
    }
}
