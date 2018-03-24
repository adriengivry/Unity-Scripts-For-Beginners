using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Here is a simple first person camera that helps you to move into your level with colliders
 */ 
public class FirstPersonCamera : MonoBehaviour
{
    [Header("CAMERA PARAMETERS")]
    [SerializeField] private float m_cameraHorizontalSpeed = 2.0f;
    [SerializeField] private float m_cameraVerticalSpeed = 2.0f;

    private Rigidbody m_rigidbody;

    private float m_yaw;
    private float m_pitch;
    

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody)
            m_rigidbody.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;

        m_yaw = 0.0f;
        m_pitch = 0.0f;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        m_yaw += m_cameraHorizontalSpeed * Input.GetAxisRaw("Mouse X");
        m_pitch -= m_cameraVerticalSpeed * Input.GetAxisRaw("Mouse Y");
        transform.eulerAngles = new Vector3(m_pitch, m_yaw, 0.0f);
    }
}