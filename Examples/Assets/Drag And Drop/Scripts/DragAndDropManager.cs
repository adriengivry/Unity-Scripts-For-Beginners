using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Essential script to put on a GameObject of the scene where you want to enable drag and drop system
 */ 
public class DragAndDropManager : MonoBehaviour {

    [Header("INPUT BINDING")]
    [SerializeField] private string m_dragInput;
    [SerializeField] private string m_dropInput;

    [Header("DRAG AND DROP PARAMETERS")]
    [SerializeField] private float m_maximumDistanceFromCameraToObject;

    private Draggable m_draggedObject;

    private void Update()
    {
        if (Input.GetButtonDown(m_dragInput) && m_draggedObject == null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, m_maximumDistanceFromCameraToObject))
            {
                var target = hit.transform.GetComponent<Draggable>();

                if (target != null)
                {
                    target.Drag();
                    m_draggedObject = target;
                }
            }
        }

        if (Input.GetButtonUp(m_dropInput) && m_draggedObject != null)
        {
            m_draggedObject.Drop();
            m_draggedObject = null;
        }
    }
}
