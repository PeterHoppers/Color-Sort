using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DraggableParent : MonoBehaviour
{
    public int offsetAmount = 5;
    List<Transform> childrenPositions = new List<Transform>();
    VerticalLayoutGroup group;

    public void SetupChildren(List<GameObject> children)
    {
        if (!group) group = GetComponent<VerticalLayoutGroup>();

        childrenPositions = new List<Transform>();
        foreach (GameObject child in children)
        {
            childrenPositions.Add(child.transform);
            child.GetComponent<DraggableObject>().ParentGroup = this;
        }
        ResetGroup();
    }

    public void DestoryAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        childrenPositions.Clear();
    }

    public void ShuffleAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.SetSiblingIndex(UnityEngine.Random.Range(0, transform.childCount));
        }
    }

    public void CheckPosition(DraggableObject draggable, GameObject visable)
    {
        if (draggable == null) return;

        int draggableY = Convert.ToInt32(visable.transform.position.y);

        foreach (Transform optionTransform in childrenPositions)
        {
            if (optionTransform == draggable.transform) continue;

            int targetY = Convert.ToInt32(optionTransform.position.y);
            if (draggableY > targetY - offsetAmount && draggableY < targetY + offsetAmount)
            {
                int draggableIndex = draggable.transform.GetSiblingIndex();
                int newIndex = optionTransform.GetSiblingIndex();
                draggable.transform.SetSiblingIndex(newIndex);
                optionTransform.SetSiblingIndex(draggableIndex);
                ResetGroup();
                return;
            }
        }
    }


    public void ResetGroup() 
    {
        group.enabled = false;
        group.enabled = true;
    }
}
