using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbleToPickup : MonoBehaviour
{
    private enum InputMode
    {
        UNITY,
        CUSTOM
    }

    [Header("INPUT PARAMETERS")]
    [SerializeField] private InputMode m_inputMode;

    [Header("UNITY INPUTS")]
    [SerializeField] private string m_pickupInput;
    [SerializeField] private string m_dropInput;
    [SerializeField] private string m_throwInput;

    [Header("CUSTOM INPUTS")]
    [SerializeField] private KeyCode m_pickupKey;
    [SerializeField] private KeyCode m_dropKey;
    [SerializeField] private KeyCode m_throwKey;

    [Header("THROW PARAMETERS")]
    [SerializeField] private float m_throwStrength;
    [SerializeField] private float m_distanceToPickup;
    [SerializeField] private float m_carryDistance;
    [SerializeField] private float m_smooth;

    private GameObject m_carriedObject;
    private float m_carryDistanceNaturalOffset;

    private void Update()
    {
        if (m_carriedObject == null)
        {
            if (IsActionTriggered(m_pickupKey, m_pickupInput))
                TryToPickupSomething();
        }
        else
        {
            UpdateCarriedObject();

            if (IsActionTriggered(m_dropKey, m_dropInput))
                DropObject();
            else if (IsActionTriggered(m_throwKey, m_throwInput))
                ThrowObject();
        }            
    }

    private bool IsActionTriggered(KeyCode p_customKey, string p_unityInput)
    {
        switch (m_inputMode)
        {
            case InputMode.CUSTOM:
                return Input.GetKeyDown(p_customKey);

            case InputMode.UNITY:
                return Input.GetButtonDown(p_unityInput);
        }

        return false;
    }

    private void UpdateCarriedObject()
    {
        m_carriedObject.transform.position = Vector3.Lerp(m_carriedObject.transform.position, transform.position + Camera.main.transform.forward * (m_carryDistance + m_carryDistanceNaturalOffset), m_smooth * Time.deltaTime);
        m_carriedObject.transform.rotation = transform.rotation;
    }

    private void TryToPickupSomething()
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        var hits = Physics.RaycastAll(transform.position, cameraForward, m_distanceToPickup);

        Debug.DrawRay(transform.position, cameraForward, Color.red);

        foreach (var hit in hits)
        {
            var target = hit.transform.gameObject;

            if (target.tag == "Carriable")
            {
                m_carriedObject = target;
                m_carriedObject.GetComponent<Rigidbody>().useGravity = false;
                m_carriedObject.GetComponent<Collider>().enabled = false;
                Mesh mesh = m_carriedObject.GetComponent<MeshFilter>().mesh;
                m_carryDistanceNaturalOffset = mesh.bounds.size.z * m_carriedObject.transform.localScale.z;
            }
        }
    }

    private void DropObject()
    {
        m_carriedObject.GetComponent<Rigidbody>().useGravity = true;
        m_carriedObject.GetComponent<Collider>().enabled = true;
        m_carriedObject = null;
    }

    private void ThrowObject()
    {
        m_carriedObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * m_throwStrength);
        DropObject();
    }
}
