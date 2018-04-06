using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private enum DoorState
    {
        OPEN,
        CLOSE
    }

    private enum DoorOpeningDirection
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE
    }

    [Header("DOOR ACTIVATION PARAMETERS")]
    [SerializeField] private bool m_allowManualInteraction;
    [SerializeField] private DoorState m_startState;

    private Transform m_pivot;
    private GameObject m_player;
    private Vector3 m_initialPivotRotation;
    private DoorState m_doorState;
    private DoorOpeningDirection m_doorOpeningDirection;
    private float m_timer;
    private float m_rotationCoefficient;

    private bool m_locked;

    private void Awake()
    {
        m_pivot = transform.Find("Pivot");
        m_initialPivotRotation = m_pivot.rotation.eulerAngles;
        m_locked = false;

        if (m_startState == DoorState.OPEN)
            Open(Camera.main.gameObject);
        else if (m_startState == DoorState.CLOSE)
            Close(gameObject);
    }

    public void Open(GameObject p_activator = null, bool p_ignoreLock = false)
    {
        if (m_locked && !p_ignoreLock)
            return;

        m_doorState = DoorState.OPEN;

        if (p_activator)
        {
            float angle = Vector3.SignedAngle(transform.right, p_activator.transform.position - transform.position, transform.up);

            if (angle > 0)
                m_doorOpeningDirection = DoorOpeningDirection.COUNTER_CLOCKWISE;
            else
                m_doorOpeningDirection = DoorOpeningDirection.CLOCKWISE;
        }

        m_timer = 0.0f;
    }

    public void Close(GameObject p_activator, bool p_ignoreLock = false)
    {
        if (m_locked && !p_ignoreLock)
            return;

        m_doorState = DoorState.CLOSE;
        m_timer = 0.0f;
    }

    public void Interact(GameObject p_activator, bool p_ignoreLock = false)
    {
        switch (m_doorState)
        {
            case DoorState.OPEN:
                if (m_allowManualInteraction)
                    Close(p_activator, p_ignoreLock);
                break;

            case DoorState.CLOSE:
                if (m_allowManualInteraction)
                    Open(p_activator, p_ignoreLock);
                break;
        }
    }

    private bool IsOpened()
    {
        return Mathf.Abs(m_rotationCoefficient) == 1.0f;
    }

    private bool IsClosed()
    {
        return Mathf.Abs(m_rotationCoefficient) == 0.0f;
    }

    private void Update()
    {
        if (m_rotationCoefficient != GetRotationCoefficientTarget())
        {
            m_timer += Time.deltaTime;
            UpdatePivotRotation();
        }
    }

    private float GetRotationCoefficientTarget()
    {
        float rotationCoefficient = 0.0f;

        switch (m_doorState)
        {
            case DoorState.OPEN:
                rotationCoefficient = 1.0f;
                break;

            case DoorState.CLOSE:
                rotationCoefficient = 0.0f;
                break;
        }

        switch (m_doorOpeningDirection)
        {
            default:
            case DoorOpeningDirection.CLOCKWISE:
                return rotationCoefficient;

            case DoorOpeningDirection.COUNTER_CLOCKWISE:
                return -rotationCoefficient;
        }
    }

    private void UpdatePivotRotation()
    {
        m_rotationCoefficient = Mathf.Lerp(m_rotationCoefficient, GetRotationCoefficientTarget(), m_timer);
        m_pivot.eulerAngles = m_initialPivotRotation + new Vector3(0.0f, 90.0f * m_rotationCoefficient, 0.0f);
    }

    public void Lock()
    {
        m_locked = true;
    }

    public void Unlock()
    {
        m_locked = false;
    }

    public bool CanInteractWith()
    {
        return m_allowManualInteraction;
    }
}
