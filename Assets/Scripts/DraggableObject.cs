using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IDragHandler, IEndDragHandler
{
    DraggableParent parentGroup;

    //visuals for the dragging
    GameObject draggableClone;

    public DraggableParent ParentGroup { get => parentGroup; set => parentGroup = value; }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggableClone) 
        {
            draggableClone = Object.Instantiate(this.gameObject, transform.parent.parent);
        }
        
        parentGroup.CheckPosition(this, draggableClone);
        draggableClone.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        parentGroup.ResetGroup();
        ResetDraggable();
    }

    void ResetDraggable() 
    {
        if (draggableClone)
        {
            Destroy(draggableClone);
        }
    }
}
