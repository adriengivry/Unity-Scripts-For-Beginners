using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * A trigger script to open, close and lock doors
 */
public class DoorTrigger : MonoBehaviour
{
    public static UnityEvent TriggerEnter = new UnityEvent();
    public static UnityEvent TriggerExit = new UnityEvent();

    private enum DoorAction
    {
        OPEN,
        CLOSE,
        NONE
    }

    [Header("LINKED DOORS")]
    [SerializeField] private List<Door> m_linkedDoors;

    [Header("ACTIONS ON TRIGGER")]
    [SerializeField] private DoorAction m_onTriggerEnter;
    [SerializeField] private DoorAction m_onTriggerExit;

    [Header("PROPERTIES")]
    [SerializeField] private bool m_lockOnOpen;
    [SerializeField] private bool m_lockOnClose;
    [SerializeField] private bool m_ignoreLock;

    private Collider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter.Invoke();
        Execute(m_onTriggerEnter, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExit.Invoke();
        Execute(m_onTriggerExit, other.gameObject);
    }

    private void Execute(DoorAction p_action, GameObject p_activator)
    {
        switch (p_action)
        {
            case DoorAction.OPEN:
                OpenDoors(p_activator);
                break;

            case DoorAction.CLOSE:
                CloseDoors(p_activator);
                break;
        }
    }

    private void OpenDoors(GameObject p_activator)
    {
        var doorInteractionScript = p_activator.GetComponent<DoorInteraction>();

        if (doorInteractionScript && doorInteractionScript.CanUseTriggers())
        {
            foreach (var door in m_linkedDoors)
            {
                door.Open(p_activator, m_ignoreLock);

                if (m_lockOnOpen)
                    door.Lock();
            }
        }
    }

    private void CloseDoors(GameObject p_activator)
    {
        var doorInteractionScript = p_activator.GetComponent<DoorInteraction>();

        if (doorInteractionScript.CanUseTriggers())
        {
            foreach (var door in m_linkedDoors)
            {
                door.Close(p_activator, m_ignoreLock);

                if (m_lockOnClose)
                    door.Lock();
            }
        }
    }
}
