using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Essential script to put on a GameObject of the scene where you want to enable drag and drop system
 */ 
public class DragAndDropManager : MonoBehaviour
{
    public static UnityEvent CanDragEvent = new UnityEvent();
    public static UnityEvent CannotDragEvent = new UnityEvent();
    public static UnityEvent DragEvent = new UnityEvent();
    public static UnityEvent DropEvent = new UnityEvent();

    [Header("INPUT BINDING")]
    [SerializeField] private string m_dragInput;
    [SerializeField] private string m_dropInput;

    [Header("DRAG AND DROP PARAMETERS")]
    [SerializeField] private float m_maximumDistanceFromCameraToObject;

    private Draggable m_draggedObject;

    private void Update()
    {
        if (m_draggedObject == null)
        {
            Draggable target;
            if (CheckIfCanDrag(out target))
            {
                if (Input.GetButtonDown(m_dragInput))
                {
                    DragEvent.Invoke();
                    target.Drag();
                    m_draggedObject = target;
                }
            }
        }

        if (Input.GetButtonUp(m_dropInput) && m_draggedObject != null)
        {
            DropEvent.Invoke();
            m_draggedObject.Drop();
            m_draggedObject = null;
        }
    }

    private bool CheckIfCanDrag(out Draggable p_target)
    {
        p_target = null;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, m_maximumDistanceFromCameraToObject))
        {
            var target = hit.transform.GetComponent<Draggable>();

            if (target != null)
            {
                CanDragEvent.Invoke();
                p_target = target;
                return true;
            }
        }

        return false;
    }
}
