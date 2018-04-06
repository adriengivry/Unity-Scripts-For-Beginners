using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> {}

    public GameObjectEvent DetectionEvent = new GameObjectEvent();

    private void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, 100);

        foreach (var hit in hits)
        {
            DetectionEvent.Invoke(hit.transform.gameObject);
        }
        
        /*
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            DetectionEvent.Invoke(hit.transform.gameObject);
        */
    }
}
