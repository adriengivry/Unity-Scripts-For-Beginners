using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script aims to provide you a very simple implementation of a day and night cycle for your unity projects
 * 
 * In order to get this script working, we need a "Directional Light" gameObject.
 * A directional light is a type of light that emits light strait forward. The sun can be
 * represented in Unity by a directional light.
 * In order to get a day and night cycle, we need to make the direction of the light rotating.
 */
public class DayAndNightCycle : MonoBehaviour
{
    public enum DayTime
    {
        EARLYMORNING,
        MORNING,
        MIDDAY,
        AFTERNOON,
        EVENING,
        EARLYNIGHT,
        NIGHT,
        NIGHTEND
    }

    [Header("SETTING UP LINKS")]
    [SerializeField] private GameObject m_directionalLightObject;

    [Header("TWEAK YOUR DAY AND NIGHT CYCLE SYSTEM")]
    [SerializeField] private DayTime m_startingDayTime;
    [SerializeField] private float m_hoursPerSeconds;

    /* 
     * We'll assume that the time of the day is stored as a "float" (Floating point number with single precision), in hours.
     * Notice that in most of programming languages, including C#, you can handle floating point numbers with an higher precision
     * using the variable type "double" (Floating point number with double precision). If you don't care about really high precision,
     * use a float instead of double, as they needs less memory to get stored on a computer. 
     * For 12AM we'll have 12.0f (The 'f' indicates that we are working with floats)
     */ 
    private float m_daytime;

    private void Start()
    {
        // Here we will set the time of the day in function of the preset entered in inspector
        switch (m_startingDayTime)
        {
            case DayTime.EARLYMORNING:
                m_daytime = 6.0f;
                break;

            case DayTime.MORNING:
                m_daytime = 9.0f;
                break;

            case DayTime.MIDDAY:
                m_daytime = 12.0f;
                break;

            case DayTime.AFTERNOON:
                m_daytime = 15.0f;
                break;

            case DayTime.EVENING:
                m_daytime = 18.0f;
                break;

            case DayTime.EARLYNIGHT:
                m_daytime = 21.0f;
                break;

            case DayTime.NIGHT:
                m_daytime = 0.0f;
                break;

            case DayTime.NIGHTEND:
                m_daytime = 3.0f;
                break;
        }
    }

    /*
     * This method can be used to convert daytime to rotation in degrees (Ex: 12AM = 90�)
     */
    private float ConvertDaytimeToRotation(float p_daytime)
    {
        float rotation = m_daytime / 24.0f;
        rotation *= 360.0f;
        rotation -= 90.0f;
        if (rotation < 0.0f)
        {
            rotation += 360.0f;
        }

        return rotation;
    }

    /*
     * Can be used by any script to get the time of the day
     */ 
    public float GetDaytime()
    {
        return m_daytime;
    }

    /*
     * Here is where the magic happens. First we get the rotation in degrees from the daytime,
     * then we set the rotation of the directional light (the sun) to this freshly calculated rotation.
     * After that we update the time of the day by the increment setup in the inspector (Hours per seconds)
     */
    private void Update()
    {
        float rotation = ConvertDaytimeToRotation(m_daytime);
        m_directionalLightObject.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);

        m_daytime += m_hoursPerSeconds * Time.deltaTime;
    }
}
