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

    private Door m_toInteractWith;

    private void Update()
    {
        if (CheckIfCanInteract(out m_toInteractWith))
        {
            if (Input.GetButtonDown(m_interactInput))
            {
                InteractWithDoorEvent.Invoke();
                m_toInteractWith.Interact(gameObject);
            }
        }
    }

    private bool CheckIfCanInteract(out Door p_door)
    {
        p_door = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);

        foreach (var hit in hits)
        {
            GameObject gameObjectHit = hit.transform.gameObject;
            Collider gameObjectCollider = gameObjectHit.GetComponent<Collider>();
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 gameObjectHitClosestPoint = gameObjectCollider.ClosestPointOnBounds(cameraPosition);
            float distanceBetweenCameraAndObject = Vector3.Distance(cameraPosition, gameObjectHitClosestPoint);

            if (distanceBetweenCameraAndObject <= m_minimumDistanceToOpen)
            {
                Door attachedDoor = gameObjectHit.GetComponentInParent<Door>();

                if (attachedDoor && attachedDoor.CanInteractWith())
                {
                    p_door = attachedDoor;
                    break;
                }
            }
        }

        if (p_door != null)
        {
            CanInteractWithDoorEvent.Invoke();
            return true;
        }

        CannotInteractWithDoorEvent.Invoke();
            
        return false;
    }

    public bool CanUseTriggers()
    {
        return m_canUseTriggers;
    }
}
