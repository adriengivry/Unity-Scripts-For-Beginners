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
        Grabber.CanGrabEvent.AddListener(OnCanInteract);
        Grabber.CannotGrabEvent.AddListener(OnCannotInteract);
        Grabber.GrabEvent.AddListener(OnInteract);

        DoorInteraction.CanInteractWithDoorEvent.AddListener(OnCanInteract);
        DoorInteraction.CannotInteractWithDoorEvent.AddListener(OnCannotInteract);
        DoorInteraction.InteractWithDoorEvent.AddListener(OnInteract);
    }

    private static void OnCanInteract()
    {
        m_crosshairAnimation.SetColor(Color.yellow);
        m_crosshairAnimation.SetRelativeScale(2.0f);
    }

    private static void OnCannotInteract()
    {
        m_crosshairAnimation.ResetColor();
        m_crosshairAnimation.ResetScale();
    }

    private static void OnInteract()
    {
        m_crosshairAnimation.Rotate(180);
    }
}
