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
    public static UnityEvent IsGrabbingEvent = new UnityEvent();

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

    private bool m_grabbedDuringThisFrame;

    private void Awake()
    {
        GetComponent<Detector>().DetectionEvent.AddListener(OnDetection);
    }

    private void Update()
    {
        if (m_grabbedObject != null)
        {
            IsGrabbingEvent.Invoke();

            UpdateGrabbedObject();

            if (!m_grabbedDuringThisFrame)
            {
                if (Input.GetButtonDown(m_dropInput))
                    DropObject();
                else if (Input.GetButtonDown(m_throwInput))
                    ThrowObject(m_throwStrength);
            }
        }

        m_grabbedDuringThisFrame = false;
    }

    private void OnDetection(GameObject p_detected)
    {
        if (m_grabbedObject == null)
        {
            Grabbable p_grabbable = p_detected.GetComponent<Grabbable>();

            if (p_grabbable != null)
            {
                Collider gameObjectCollider = p_detected.GetComponent<Collider>();
                Vector3 cameraPosition = Camera.main.transform.position;
                Vector3 gameObjectHitClosestPoint = gameObjectCollider.ClosestPointOnBounds(cameraPosition);

                if (Vector3.Distance(cameraPosition, gameObjectHitClosestPoint) <= m_minimumDistanceToGrab)
                {
                    CanGrabEvent.Invoke();

                    if (Input.GetButtonDown(m_grabInput))
                    {
                        GrabObject(p_detected);
                    }
                }
            }
        }
    }

    private void UpdateGrabbedObject()
    {
        var currentPosition = m_grabbedObject.transform.position;
        var targetedPosition = Camera.main.transform.position + Camera.main.transform.forward * (m_distanceBetweenObjectAndCamera + m_distanceBetweenObjectAndCameraDueToMeshSize);
        m_grabbedObject.transform.position = Vector3.MoveTowards(currentPosition, targetedPosition, m_objectPositionSmoothing);

        var currentRotation = m_grabbedObject.transform.rotation;
        var targetedRotation = transform.rotation;
        m_grabbedObject.transform.rotation = Quaternion.Slerp(currentRotation, targetedRotation, m_objectRotationSmoothing);

        if (m_grabbedObjectScript.IsLinkedLost())
            DropObject();
    }

    private void GrabObject(GameObject p_toGrab)
    {
        GrabEvent.Invoke();
        m_grabbedObject = p_toGrab;
        m_grabbedObjectScript = m_grabbedObject.GetComponent<Grabbable>();

        m_grabbedObjectScript.Grab(gameObject);
        m_distanceBetweenObjectAndCameraDueToMeshSize = m_grabbedObjectScript.CalculateDistanceToCameraOffset();
        m_grabbedDuringThisFrame = true;
    }

    private void DropObject()
    {
        ThrowObject(0);
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
