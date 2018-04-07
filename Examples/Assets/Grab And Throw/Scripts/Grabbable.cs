using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script to place on any Grabbable object
 */ 
public class Grabbable : MonoBehaviour
{
    [Header("PARAMATERS TO CONSIDER WHEN GRABBED")]
    [SerializeField] private bool m_throwable = true;
    [SerializeField] private bool m_kinematic = true;
    [SerializeField] private bool m_colliderEnabled = true;

    private bool m_wasKinematic;
    private bool m_wasColliderEnabled;
    private bool m_wasUsingGravity;

    private bool m_linkedToGrabber;

    private Rigidbody m_rigidbody;
    private Collider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(GameObject p_grabber)
    {
        m_linkedToGrabber = true;
        m_wasKinematic = m_rigidbody.isKinematic;
        m_wasColliderEnabled = m_collider.enabled;
        m_wasUsingGravity = m_rigidbody.useGravity;

        m_rigidbody.isKinematic = m_kinematic;
        m_rigidbody.useGravity = false;
        m_collider.enabled = m_colliderEnabled;

        Physics.IgnoreCollision(m_collider, p_grabber.GetComponent<Collider>(), true);
    }

    public void Drop(GameObject p_dropper)
    {
        m_rigidbody.isKinematic = m_wasKinematic;
        m_collider.enabled = m_wasColliderEnabled;
        m_rigidbody.useGravity = m_wasUsingGravity;

        Physics.IgnoreCollision(m_collider, p_dropper.GetComponent<Collider>(), false);
    }

    public void Throw(float p_strength)
    {
        if (m_throwable)
            m_rigidbody.AddForce(Camera.main.transform.forward * p_strength);
    }

    public float CalculateDistanceToCameraOffset()
    {
        return GetComponent<MeshFilter>().mesh.bounds.size.z * transform.localScale.z;
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_linkedToGrabber = false;
    }

    public bool IsLinkedLost()
    {
        return !m_linkedToGrabber;
    }
}
