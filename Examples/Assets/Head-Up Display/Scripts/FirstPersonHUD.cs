using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script that handle In-game user interface. It will listen to events sent by MonoBehaviours and will start animate, rotate, the crosshair
 */ 
public class FirstPersonHUD : MonoBehaviour
{
    [Header("LINKED GAMEOBJECTS")]
    [SerializeField] private GameObject m_crosshairPrefab;

    private static ImageAnimation m_crosshairAnimation;

    private void Awake()
    {
        ListenToEvents();

        if (m_crosshairPrefab != null)
            m_crosshairAnimation = m_crosshairPrefab.GetComponent<ImageAnimation>();
    }

    private void ListenToEvents()
    {
        Grabber.CanGrabEvent.AddListener(OnCanGrab);
        Grabber.CannotGrabEvent.AddListener(OnCannotGrab);

        DoorInteraction.CanInteractWithDoorEvent.AddListener(OnCanInteractWithDoor);
        DoorInteraction.CannotInteractWithDoorEvent.AddListener(OnCannotInteractWithDoor);
    }

    private static void OnCanGrab()
    {
        m_crosshairAnimation.Show(true);
    }

    private static void OnCannotGrab()
    {
        m_crosshairAnimation.Hide(true);
    }

    private static void OnCanInteractWithDoor()
    {
        m_crosshairAnimation.Show(true);
    }

    private static void OnCannotInteractWithDoor()
    {
        m_crosshairAnimation.Hide(true);
    }
}
