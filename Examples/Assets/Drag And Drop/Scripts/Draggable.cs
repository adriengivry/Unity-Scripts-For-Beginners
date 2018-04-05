using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Put this script on a GameObject to make it draggable
 */
public class Draggable : MonoBehaviour
{
    private bool m_dragged;

    private void Update()
    {
        if (m_dragged)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
        }
    }

    public void Drag()
    {
        m_dragged = true;
    }

    public void Drop()
    {
        m_dragged = false;
    }
}
