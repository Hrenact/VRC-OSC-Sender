using UnityEngine;
using UnityEngine.UI;

public class HierarchyMover : MonoBehaviour
{
    public Image BackgroundImage;
    public Color evenColor = Color.white;
    public Color oddColor = Color.gray;

    void Update()
    {
        int index = transform.GetSiblingIndex();

        if (index % 2 == 0)
        {
            BackgroundImage.color = evenColor;
        }
        else
        {
            BackgroundImage.color = oddColor;
        }
    }

    public void MoveUp()
    {
        int index = transform.GetSiblingIndex();
        if (index > 0)
        {
            transform.SetSiblingIndex(index - 1);
        }
    }

    public void MoveDown()
    {
        int index = transform.GetSiblingIndex();
        int siblingCount = transform.parent.childCount;

        if (index < siblingCount - 1)
        {
            transform.SetSiblingIndex(index + 1);
        }
    }
}
