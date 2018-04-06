using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * A script to put on a Player to allow him to grab and throw objects (First person needed)
 */
public class Grabber : MonoBehaviour
{
    public static UnityEvent CanGrabEvent = new UnityEvent();
    public static UnityEvent GrabEvent = new UnityEvent();
    public static UnityEvent DropEvent = new UnityEvent();
    public static UnityEvent ThrowEvent = new UnityEvent();

    [Header("INPUT BINDING")]
    [SerializeField] private string m_grabInput;
    [SerializeField] private string m_dropInput;
    [SerializeField] private string m_throwInput;

    [Header("GLOBAL PARAMETERS")]
    [SerializeField] private float m_minimumDistanceToGrab;
    [SerializeField] private float m_objectPositionSmoothing;
    [SerializeField] private float m_objectRotationSmoothing;

    [Header("THROW PARAMETERS")]
    [SerializeField] private float m_throwStrength;

    private GameObject m_grabbedObject;
    private Grabbable m_grabbedObjectScript;

    private float m_distanceBetweenObjectAndCamera = 0.6f;
    private float m_distanceBetweenObjectAndCameraDueToMeshSize;

    private Vector3 m_objectVelocity;
    private Vector3 m_objectAngularVelocity;

    private void Update()
    {
        if (m_grabbedObject == null)
        {
            GameObject toGrab;
            if (CheckIfCanGrab(out toGrab))
                if (Input.GetButtonDown(m_grabInput))
                    GrabObject(toGrab);
        }
        else
        {
            UpdateGrabbedObject();

            if (Input.GetButtonDown(m_dropInput))
                ThrowObject(0);
            else if (Input.GetButtonDown(m_throwInput))
                ThrowObject(m_throwStrength);
        }            
    }

    private void UpdateGrabbedObject()
    {
        var currentPosition = m_grabbedObject.transform.position;
        var targetedPosition = Camera.main.transform.position + Camera.main.transform.forward * (m_distanceBetweenObjectAndCamera + m_distanceBetweenObjectAndCameraDueToMeshSize);
        m_grabbedObject.transform.position = Vector3.Lerp(currentPosition, targetedPosition, m_objectPositionSmoothing);

        var currentRotation = m_grabbedObject.transform.rotation;
        var targetedRotation = transform.rotation;
        m_grabbedObject.transform.rotation = Quaternion.Slerp(currentRotation, targetedRotation, m_objectRotationSmoothing);
    }

    private void GrabObject(GameObject p_toGrab)
    {
        GrabEvent.Invoke();
        m_grabbedObject = p_toGrab;
        m_grabbedObjectScript = m_grabbedObject.GetComponent<Grabbable>();

        m_grabbedObjectScript.Grab(gameObject);
        m_distanceBetweenObjectAndCameraDueToMeshSize = m_grabbedObjectScript.CalculateDistanceToCameraOffset();
    }

    private bool CheckIfCanGrab(out GameObject p_grabbableFound)
    {
        p_grabbableFound = null;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameObjectHit = hit.transform.gameObject;
            Grabbable grabbedObjectScript = gameObjectHit.GetComponent<Grabbable>();
            if (grabbedObjectScript != null && hit.distance <= m_minimumDistanceToGrab)
            {
                p_grabbableFound = gameObjectHit;
            }
        }

        if (p_grabbableFound != null)
        {
            CanGrabEvent.Invoke();
            return true;
        }
        return false;
    }

    private void ThrowObject(float p_strength)
    {
        if (p_strength > 0)
            ThrowEvent.Invoke();
        else
            DropEvent.Invoke();

        m_grabbedObjectScript.Drop(gameObject);
        m_grabbedObjectScript.Throw(p_strength);

        m_grabbedObject = null;
        m_grabbedObjectScript = null;
    }
}
