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
    [SerializeField] private float m_movementSpeed;
    [SerializeField] private MovementMode m_movementMode;

    [Header("MOVEMENT KEYS")]
    [SerializeField] private bool m_useCustomInputs;
    [SerializeField] private KeyCode m_customForwardKey;
    [SerializeField] private KeyCode m_customBackwardKey;
    [SerializeField] private KeyCode m_customRightKey;
    [SerializeField] private KeyCode m_customLeftKey;

    [Header("MOVEMENT KEYS (FLYING MODE ONLY)")]
    [SerializeField] private bool m_useFlyingModeCustomInputs;
    [SerializeField] private KeyCode m_customUpKey;
    [SerializeField] private KeyCode m_customDownKey;

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

    private float GetVerticalMove()
    {
        float vertical = 0.0f;

        if (m_useCustomInputs)
        {
            vertical += System.Convert.ToSingle(Input.GetKey(m_customForwardKey));
            vertical -= System.Convert.ToSingle(Input.GetKey(m_customBackwardKey));
        }
        else
        {
            vertical = Input.GetAxisRaw("Vertical");
        }

        return vertical;
    }

    private float GetHorizontalMove()
    {
        float horizontal = 0.0f;

        if (m_useCustomInputs)
        {
            horizontal += System.Convert.ToSingle(Input.GetKey(m_customRightKey));
            horizontal -= System.Convert.ToSingle(Input.GetKey(m_customLeftKey));
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        return horizontal;
    }

    private float GetUpMode()
    {
        float up = 0.0f;

        if (m_useFlyingModeCustomInputs)
        {
            up += System.Convert.ToSingle(Input.GetKey(m_customUpKey));
            up -= System.Convert.ToSingle(Input.GetKey(m_customDownKey));
        }
        else
        {
            try
            {
                up = Input.GetAxisRaw("Up");
            }
            catch
            {
                string error = "\"Up\" axis not found. Use custom mode or setup Unity Inputs";
                Debug.LogError(error);
            }
        }

        return up;
    }

    private void Update()
    {
        Vector3 movement = new Vector3();
        movement += Camera.main.transform.forward * GetVerticalMove();
        movement += Camera.main.transform.right * GetHorizontalMove();

        if (m_movementMode == MovementMode.NORMAL)
        {
            movement.y = m_rigidbody.velocity.y;
        }

        if (m_movementMode == MovementMode.FLYING)
        {
            movement += Camera.main.transform.up * GetUpMode();
        }

        movement.Normalize();
        movement *= m_movementSpeed;

        m_rigidbody.velocity = movement;
    }
}