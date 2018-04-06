using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    public static UnityEvent TriggerEnter = new UnityEvent();

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);

        foreach (var hit in hits)
        {
            GameObject gameObjectHit = hit.transform.gameObject;
            Collider gameObjectCollider = gameObjectHit.GetComponent<Collider>();
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 gameObjectHitClosestPoint = gameObjectCollider.ClosestPointOnBounds(cameraPosition);
            float distanceBetweenCameraAndObject = Vector3.Distance(cameraPosition, gameObjectHitClosestPoint);

        }
    }
}
