using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script that handle In-game user interface. It will listen to events sent by MonoBehaviours and will start animate, rotate, the crosshair
 */ 
public class FirstPersonHUD : MonoBehaviour
{
    private enum InteractionState
    {
        CAN,
        CANNOT,
        IS,
        NEUTRAL
    }

    [Header("LINKED GAMEOBJECTS")]
    [SerializeField] private GameObject m_crosshairPrefab;

    private static ImageAnimation m_crosshairAnimation;

    private static InteractionState m_interactionState;

    private void Awake()
    {
        ListenToEvents();

        if (m_crosshairPrefab != null)
            m_crosshairAnimation = m_crosshairPrefab.GetComponent<ImageAnimation>();
    }

    private void ListenToEvents()
    {
        Grabber.CanGrabEvent.AddListener(OnCanInteract);
        Grabber.GrabEvent.AddListener(OnInteract);
        Grabber.DropEvent.AddListener(OnInteract);
        Grabber.ThrowEvent.AddListener(OnInteract);
        Grabber.IsGrabbingEvent.AddListener(OnIsInteracting);

        DoorInteraction.CanInteractWithDoorEvent.AddListener(OnCanInteract);
        DoorInteraction.CannotInteractWithDoorEvent.AddListener(OnCannotInteract);
        DoorInteraction.InteractWithDoorEvent.AddListener(OnInteract);
    }

    private static void OnCanInteract()
    {
        m_interactionState = InteractionState.CAN;
    }

    private static void OnCannotInteract()
    {
        m_interactionState = InteractionState.CANNOT;
    }

    private static void OnIsInteracting()
    {
        m_interactionState = InteractionState.IS;
    }

    private static void OnInteract()
    {
        m_crosshairAnimation.Rotate(180);
    }

    private void LateUpdate()
    {
        switch (m_interactionState)
        {
            case InteractionState.CAN:
                m_crosshairAnimation.ResetAlpha();
                m_crosshairAnimation.SetColor(Color.yellow);
                m_crosshairAnimation.SetRelativeScale(2.0f);
                break;

            case InteractionState.CANNOT:
                m_crosshairAnimation.ResetAlpha();
                m_crosshairAnimation.SetColor(Color.red);
                m_crosshairAnimation.ResetScale();
                break;

            case InteractionState.IS:
                m_crosshairAnimation.SetAlpha(0.0f);
                break;

            case InteractionState.NEUTRAL:
                m_crosshairAnimation.ResetAlpha();
                m_crosshairAnimation.ResetColor();
                m_crosshairAnimation.ResetScale();
                break;
        }

        m_interactionState = InteractionState.NEUTRAL;
    }
}
