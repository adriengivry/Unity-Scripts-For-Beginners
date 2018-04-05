using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * Here is a simple first person movement controller that helps you to move into your level.
 * You must add this script on your first person character
 */
public class FirstPersonMovement : MonoBehaviour
{
    private enum MovementMode
    {
        NORMAL,
        FLYING
    }

    [Header("MOVEMENT PARAMETERS")]
    [SerializeField] private MovementMode m_movementMode;
    [SerializeField] private float m_movementSpeed;
    [SerializeField] private float m_smoothing;

    [Header("INPUT BINDING")]
    [SerializeField] private string m_verticalAxisInput;
    [SerializeField] private string m_horizontalAxisInput;
    [SerializeField] private string m_upAxisInput;

    private Rigidbody m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody)
            m_rigidbody.freezeRotation = true;

        if (m_movementMode == MovementMode.FLYING)
        {
            m_rigidbody.useGravity = false;
        }
    }

    private void Update()
    {
        Vector3 movement = new Vector3();
        movement += Camera.main.transform.forward * Input.GetAxisRaw(m_verticalAxisInput);
        movement += Camera.main.transform.right * Input.GetAxisRaw(m_horizontalAxisInput);

        if (m_movementMode == MovementMode.NORMAL)
            movement.y = m_rigidbody.velocity.y;

        if (m_movementMode == MovementMode.FLYING)
            movement += Camera.main.transform.up * Input.GetAxisRaw(m_upAxisInput);

        movement.Normalize();
        movement *= m_movementSpeed;

        m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, movement, m_smoothing * Time.deltaTime);
    }
}