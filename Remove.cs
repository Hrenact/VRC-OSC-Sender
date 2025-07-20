using UnityEngine;

public class Remove : MonoBehaviour
{
    [Tooltip("要从层级中移除的对象")]
    public GameObject targetObject;

    // 可以通过按钮或其他事件调用这个方法
    public void RemoveTarget()
    {
        if (targetObject != null)
        {
            Destroy(targetObject);
            Debug.Log($"对象 {targetObject.name} 已从层级中移除");
        }
        else
        {
            Debug.LogWarning("未设置要移除的对象！");
        }
    }
}
