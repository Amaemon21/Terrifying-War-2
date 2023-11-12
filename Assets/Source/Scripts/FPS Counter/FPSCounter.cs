using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;

    public float frameTime
    {
        get
        {
            return m_FrameTime;
        }
    }
    private float m_FrameTime = 0.0f;

    public float minFrameTime
    {
        get
        {
            return m_MinFrameTime;
        }
    }
    private float m_MinFrameTime = 0.0f;

    public float maxFrameTime
    {
        get
        {
            return m_MaxFrameTime;
        }
    }
    private float m_MaxFrameTime = 0.0f;

    public float frameTimeFlutuation
    {
        get
        {
            return m_FrameTimeFlutuation;
        }
    }
    private float m_FrameTimeFlutuation = 0.0f;

    public float frameRate
    {
        get
        {
            return m_FrameRate;
        }
    }
    private float m_FrameRate = 0.0f;

    public float minFrameRate
    {
        get
        {
            return m_MinFrameRate;
        }
    }
    private float m_MinFrameRate = 0.0f;

    public float maxFrameRate
    {
        get
        {
            return m_MaxFrameRate;
        }
    }
    private float m_MaxFrameRate = 0.0f;

    public float frameRateFlutuation
    {
        get
        {
            return m_FrameRateFlutuation;
        }
    }
    private float m_FrameRateFlutuation = 0.0f;

    private Color m_FPSFieldsColor = ColorHelper.HexStrToColor("#80FF00FF");
    private Color m_FPSMinFieldsColor = ColorHelper.HexStrToColor("#FF8400FF");
    private Color m_FPSMaxFieldsColor = ColorHelper.HexStrToColor("#00A0FFFF");
    private Color m_FPSFluctuationFieldsColor = ColorHelper.HexStrToColor("#DCEC00FF");

    private float m_AccumulatedTime;
    private int m_AccumulatedFrames;
    private float m_LastUpdateTime;
    private string m_DynamicConfigurationFormat;

    public static float UpdateInterval = 0.5f;
    public static float MinTime = 0.000000001f;

    public void Initialize()
    {
        Reset();
        UpdateInternals();
    }

    public void Reset()
    {
        ResetProbingData();

        m_LastUpdateTime = Time.realtimeSinceStartup;
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void UpdateInternals()
    {
        UpdateStaticContentAndData();
    }

    private void UpdateStaticContentAndData()
    {
        m_DynamicConfigurationFormat = string.Format(
            "{0} FPS {1} ms {2}" + Environment.NewLine +
            "{3} FPS {4} ms {5}" + Environment.NewLine +
            "{6} FPS {7} ms {8}" + Environment.NewLine +
            "{9} FPS {10} ms {11}",

            ColorHelper.ColorText("{0}", m_FPSFieldsColor),
            ColorHelper.ColorText("{1}", m_FPSFieldsColor),
            ColorHelper.ColorText("Avg", m_FPSFieldsColor),//avg Σ

            ColorHelper.ColorText("{2}", m_FPSMinFieldsColor),
            ColorHelper.ColorText("{3}", m_FPSMinFieldsColor),
            ColorHelper.ColorText("Min", m_FPSMinFieldsColor),//min ⇓

            ColorHelper.ColorText("{4}", m_FPSMaxFieldsColor),
            ColorHelper.ColorText("{5}", m_FPSMaxFieldsColor),
            ColorHelper.ColorText("Max", m_FPSMaxFieldsColor),// max ⇑

            ColorHelper.ColorText("{6}", m_FPSFluctuationFieldsColor),
            ColorHelper.ColorText("{7}", m_FPSFluctuationFieldsColor),
            ColorHelper.ColorText("∿", m_FPSFluctuationFieldsColor)
        );
    }

    private void UpdateDynamicContent()
    {
        if (!fpsText)
        {
            return;
        }

        fpsText.text = string.Format(
            m_DynamicConfigurationFormat,
            m_FrameRate.ToString("F1"), (m_FrameTime * 1000.0f).ToString("F1"),
            m_MinFrameRate.ToString("F1"), (m_MaxFrameTime * 1000.0f).ToString("F1"),
            m_MaxFrameRate.ToString("F1"), (m_MinFrameTime * 1000.0f).ToString("F1"),
            m_FrameRateFlutuation.ToString("F1"), (m_FrameTimeFlutuation * 1000.0f).ToString("F1")
        );
    }

    private void ResetProbingData()
    {
        m_MinFrameTime = float.MaxValue;
        m_MaxFrameTime = float.MinValue;
        m_AccumulatedTime = 0.0f;
        m_AccumulatedFrames = 0;
    }

    private void UpdateFPS()
    {
        if (!fpsText)
        {
            return;
        }

        float deltaTime = Time.unscaledDeltaTime;

        m_AccumulatedTime += deltaTime;
        m_AccumulatedFrames++;

        if (deltaTime < MinTime)
        {
            deltaTime = MinTime;
        }

        if (deltaTime < m_MinFrameTime)
        {
            m_MinFrameTime = deltaTime;
        }

        if (deltaTime > m_MaxFrameTime)
        {
            m_MaxFrameTime = deltaTime;
        }

        float nowTime = Time.realtimeSinceStartup;
        if (nowTime - m_LastUpdateTime < UpdateInterval)
        {
            return;
        }

        if (m_AccumulatedTime < MinTime)
        {
            m_AccumulatedTime = MinTime;
        }

        if (m_AccumulatedFrames < 1)
        {
            m_AccumulatedFrames = 1;
        }

        m_FrameTime = m_AccumulatedTime / m_AccumulatedFrames;
        m_FrameRate = 1.0f / m_FrameTime;

        m_MinFrameRate = 1.0f / m_MaxFrameTime;
        m_MaxFrameRate = 1.0f / m_MinFrameTime;

        m_FrameTimeFlutuation = Mathf.Abs(m_MaxFrameTime - m_MinFrameTime) / 2.0f;
        m_FrameRateFlutuation = Mathf.Abs(m_MaxFrameRate - m_MinFrameRate) / 2.0f;

        UpdateDynamicContent();

        ResetProbingData();
        m_LastUpdateTime = nowTime;
    }

    private void Update()
    {
        UpdateFPS();
    }
}