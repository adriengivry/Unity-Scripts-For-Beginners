using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Provide some animations to a Unity UI.Image
 */ 
public class ImageAnimation : MonoBehaviour
{
    private enum Visibility
    {
        SHOW,
        HIDE
    }

    [Header("STARTUP PARAMETERS")]
    [SerializeField] private Visibility m_startVisibility;

    [Header("ANIMATION PARAMETERS")]
    [SerializeField] private float m_showAndHideSpeed;
    [SerializeField] private float m_rotationSpeed;
    

    private Image m_image;
    private float m_angle;
    private float m_newAlpha;
    
    private void Awake()
    {
        m_image = GetComponent<Image>();

        if (m_startVisibility == Visibility.SHOW)
            Show(false);
        else if (m_startVisibility == Visibility.HIDE)
            Hide(false);

        m_angle = transform.eulerAngles.z;
    }

    private void Update()
    {
        UpdateRotationAnimation();
        UpdateShowHideAnimation();
    }

    private void UpdateRotationAnimation(bool p_end = false)
    {
        var lerpT = p_end == false ? Time.deltaTime * m_rotationSpeed : 1.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, m_angle), lerpT);
    }

    private void UpdateShowHideAnimation(bool p_end = false)
    {
        var lerpT = p_end == false ? Time.deltaTime * m_showAndHideSpeed : 1.0f;
        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, Mathf.Lerp(m_image.color.a, m_newAlpha, lerpT));
    }

    public void Rotate()
    {
        m_angle += 180.0f;
    }

    public void Hide(bool p_animate = false)
    {
        ModifyAlpha(0.0f, p_animate);
    }

    public void Show(bool p_animate = false)
    {
        ModifyAlpha(1.0f, p_animate);
    }

    public void ModifyAlpha(float p_newValue, bool p_animate = false)
    {
        m_newAlpha = p_newValue;

        if (!p_animate)
            UpdateShowHideAnimation(true);
    }

    public void ModifyRotation(float p_angle, bool p_animate = false)
    {
        m_angle = p_angle;

        if (!p_animate)
            UpdateRotationAnimation(true);
    }
}
