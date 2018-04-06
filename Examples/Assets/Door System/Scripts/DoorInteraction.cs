using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * A script to place on any gameObject able to open doors
 */
public class DoorInteraction : MonoBehaviour
{
    public static UnityEvent CanInteractWithDoorEvent = new UnityEvent();
    public static UnityEvent CannotInteractWithDoorEvent = new UnityEvent();
    public static UnityEvent InteractWithDoorEvent = new UnityEvent();

    [Header("DOOR INTERACTION PARAMETERS")]
    [SerializeField] private bool m_canUseTriggers;
    [SerializeField] private string m_interactInput;
    [SerializeField] private float m_minimumDistanceToOpen;

    private void Awake()
    {
        GetComponent<Detector>().DetectionEvent.AddListener(OnDetection);
    }

    private void OnDetection(GameObject p_detected)
    {
        Door attachedDoor = p_detected.GetComponentInParent<Door>();

        if (attachedDoor != null)
        {
            Collider gameObjectCollider = p_detected.GetComponent<Collider>();
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 gameObjectHitClosestPoint = gameObjectCollider.ClosestPointOnBounds(cameraPosition);

            if (Vector3.Distance(cameraPosition, gameObjectHitClosestPoint) <= m_minimumDistanceToOpen)
            {
                if (attachedDoor.CanInteractWith())
                {
                    CanInteractWithDoorEvent.Invoke();
                    if (Input.GetButtonDown(m_interactInput))
                    {
                        InteractWithDoorEvent.Invoke();
                        attachedDoor.Interact(gameObject);
                    }
                }
                else
                {
                    CannotInteractWithDoorEvent.Invoke();
                }
            }
        }
    }

    public bool CanUseTriggers()
    {
        return m_canUseTriggers;
    }
}
