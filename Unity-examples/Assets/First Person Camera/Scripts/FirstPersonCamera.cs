using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A simple first person camera allowing you to look everywhere on your scene
 * You have to place this script on the camera associated to your first person character (The camera must be a child).
 */ 
public class FirstPersonCamera : MonoBehaviour
{
    [Header("CAMERA PARAMETERS")]
    [SerializeField] public float m_mouseSensitivity = 5.0f;
    [SerializeField] public float m_smoothing = 2.0f;

    private Vector2 m_mouseLook;
    private Vector2 m_verticalSmooth;

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    
    /*
     * Some maths to calculate the mouseLook direction
     */ 
    private void UpdateMouseLook()
    {
        Vector2 mouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseMove = Vector2.Scale(mouseMove, new Vector2(m_mouseSensitivity * m_smoothing, m_mouseSensitivity * m_smoothing));

        m_verticalSmooth.x = Mathf.Lerp(m_verticalSmooth.x, mouseMove.x, 1.0f / m_smoothing);
        m_verticalSmooth.y = Mathf.Lerp(m_verticalSmooth.y, mouseMove.y, 1.0f / m_smoothing);

        m_mouseLook += m_verticalSmooth;
        m_mouseLook.y = Mathf.Clamp(m_mouseLook.y, -90.0f, 90.0f);
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UnlockCursor();

        UpdateMouseLook();

        // Set the up/down rotation of the camera in function of the mouseLook variable
        transform.localRotation = Quaternion.AngleAxis(-m_mouseLook.y, Vector3.right);

        /*
         * Set the left/right rotation on the parent of the camera in function of the mouseLook variable.
         * We modify the parent instead of the camera gameObject in order to modify the forward vector of the gameObject.
         * This way, when you'll press the forward key, you'll move in the mouseLook direction
         */
        transform.parent.localRotation = Quaternion.AngleAxis(m_mouseLook.x, transform.parent.up);
    }
}