using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Here is a simple first person movement controller that helps you to move into your level.
 * You must add this script on your first person character
 */ 
public class FirstPersonMovement : MonoBehaviour
{
    private struct Inputs
    {
        public bool moveForward;
        public bool moveBackward;
        public bool moveRight;
        public bool moveLeft;
    }

    [Header("MOVEMENT PARAMETERS")]
    [SerializeField] private float m_movementSpeed;

    [Header("MOVEMENT KEYS")]
    [SerializeField] private KeyCode m_forwardKey;
    [SerializeField] private KeyCode m_backwardKey;
    [SerializeField] private KeyCode m_rightKey;
    [SerializeField] private KeyCode m_leftKey;

    private Rigidbody m_rigidbody;
    private Inputs m_inputs;
    private float m_currentSpeed;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody)
            m_rigidbody.freezeRotation = true;

        m_inputs.moveForward = false;
        m_inputs.moveBackward = false;
        m_inputs.moveRight = false;
        m_inputs.moveLeft = false;

        m_currentSpeed = m_movementSpeed;
    }

    private bool AnyKeyInput()
    {
        return m_inputs.moveForward || m_inputs.moveBackward || m_inputs.moveRight || m_inputs.moveLeft;
    }

    private void FixedUpdate()
    {
        float frontAndBack = 0.0f;
        float rightAndLeft = 0.0f;

        frontAndBack += System.Convert.ToSingle(Input.GetKey(m_forwardKey));
        frontAndBack -= System.Convert.ToSingle(Input.GetKey(m_backwardKey));

        rightAndLeft += System.Convert.ToSingle(Input.GetKey(m_rightKey));
        rightAndLeft -= System.Convert.ToSingle(Input.GetKey(m_leftKey));

        frontAndBack *= m_currentSpeed * Time.deltaTime;
        rightAndLeft *= m_currentSpeed * Time.deltaTime;

        transform.Translate(rightAndLeft, 0, frontAndBack);
    }
}