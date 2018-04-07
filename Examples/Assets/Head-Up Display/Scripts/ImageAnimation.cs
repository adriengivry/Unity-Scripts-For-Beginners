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

    struct ImageInfo
    {
        public float angle;
        public Color color;
        public float alpha;
        public Vector3 scale;
    }

    [Header("STARTUP PARAMETERS")]
    [SerializeField] private Visibility m_startVisibility;

    [Header("ANIMATION PARAMETERS")]
    [SerializeField] private float m_rotationAnimationSpeed;
    [SerializeField] private float m_alphaAnimationSpeed;
    [SerializeField] private float m_colorAnimationSpeed;
    [SerializeField] private float m_scaleAnimationSpeed;

    private Image m_image;

    private ImageInfo m_defaultInfo;
    private ImageInfo m_toReachInfo;

    private void Awake()
    {
        m_image = GetComponent<Image>();

        if (m_startVisibility == Visibility.SHOW)
            Show();
        else if (m_startVisibility == Visibility.HIDE)
            Hide();

        m_defaultInfo.angle = transform.eulerAngles.z;
        m_defaultInfo.color = m_image.color;
        m_defaultInfo.alpha = m_image.color.a;
        m_defaultInfo.scale = transform.localScale;

        m_toReachInfo = m_defaultInfo;
    }

    private void Update()
    {
        UpdateRotationAnimation();
        UpdateAlphaAnimation();
        UpdateColorAnimation();
        UpdateScaleAnimation();
    }

    private void UpdateRotationAnimation(bool p_end = false)
    {
        var lerpT = p_end == false ? Time.deltaTime * m_rotationAnimationSpeed : 1.0f;
        var current = transform.rotation;
        var toReach = Quaternion.Euler(0.0f, 0.0f, m_toReachInfo.angle);
        transform.rotation = Quaternion.Slerp(current, toReach, lerpT);
    }

    private void UpdateAlphaAnimation(bool p_end = false)
    {
        var lerpT = p_end == false ? Time.deltaTime * m_alphaAnimationSpeed : 1.0f;
        var current = m_image.color.a;
        var toReach = m_toReachInfo.alpha;
        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, Mathf.Lerp(current, toReach, lerpT));
    }

    private void UpdateColorAnimation(bool p_end = false)
    {
        var lerpT = p_end == false ? Time.deltaTime * m_colorAnimationSpeed : 1.0f;
        var current = m_image.color;
        var toReach = m_toReachInfo.color;
        m_image.color = new Color(Mathf.Lerp(current.r, toReach.r, lerpT), Mathf.Lerp(current.g, toReach.g, lerpT), Mathf.Lerp(current.b, toReach.b, lerpT), current.a);
    }

    private void UpdateScaleAnimation(bool p_end = false)
    {
        var lerpT = p_end == false ? Time.deltaTime * m_scaleAnimationSpeed : 1.0f;
        var current = transform.localScale;
        var toReach = m_toReachInfo.scale;
        transform.localScale = Vector3.Slerp(current, toReach, lerpT);
    }

    public void Show()
    {
        m_image.enabled = true;
    }

    public void Hide()
    {
        m_image.enabled = false;
    }

    public void Rotate(float p_angle, bool p_animate = true)
    {
        m_toReachInfo.angle += p_angle;

        if (!p_animate)
            UpdateRotationAnimation(true);
    }

    public void SetRelativeScale(Vector2 p_scale, bool p_animate = true)
    {
        m_toReachInfo.scale.x = m_defaultInfo.scale.x * p_scale.x;
        m_toReachInfo.scale.y = m_defaultInfo.scale.y * p_scale.y;

        if (!p_animate)
            UpdateScaleAnimation(true);
    }

    public void SetRelativeScale(float p_scale, bool p_animate = true)
    {
        SetRelativeScale(new Vector2(p_scale, p_scale), p_animate);
    }

    public void SetScale(Vector2 p_scale, bool p_animate = true)
    {
        m_toReachInfo.scale.x = p_scale.x;
        m_toReachInfo.scale.y = p_scale.y;

        if (!p_animate)
            UpdateScaleAnimation(true);
    }

    public void SetScale(float p_scale, bool p_animate = true)
    {
        SetScale(new Vector2(p_scale, p_scale), p_animate);
    }

    public void SetAlpha(float p_alpha, bool p_animate = true)
    {
        m_toReachInfo.alpha = p_alpha;

        if (!p_animate)
            UpdateAlphaAnimation(true);
    }

    public void SetColor(Color p_color, bool p_animate = true)
    {
        m_toReachInfo.color = p_color;

        if (!p_animate)
            UpdateColorAnimation(true);
    }

    public void Reset()
    {
        ResetRotationAngle();
        ResetColor();
        ResetAlpha();
        ResetScale();
    }

    public void ResetRotationAngle(bool p_animate = true)
    {
        m_toReachInfo.angle = m_defaultInfo.angle;

        if (!p_animate)
            UpdateColorAnimation(true);
    }

    public void ResetColor(bool p_animate = true)
    {
        m_toReachInfo.color = m_defaultInfo.color;

        if (!p_animate)
            UpdateColorAnimation(true);
    }

    public void ResetAlpha(bool p_animate = true)
    {
        m_toReachInfo.alpha = m_defaultInfo.alpha;

        if (!p_animate)
            UpdateAlphaAnimation(true);
    }

    public void ResetScale(bool p_animate = true)
    {
        m_toReachInfo.scale = m_defaultInfo.scale;

        if (!p_animate)
            UpdateColorAnimation(true);
    }
}
