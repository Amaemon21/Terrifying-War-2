using System.Collections.Generic;
using UnityEngine;

public class bl_ScopeProSettings : ScriptableObject
{
    [Header("References")]
    public Material scopeMaterial;
    public GameObject scopeCamera;
    public Texture2D[] scopeReticles;
    
    private static bl_ScopeProSettings m_Data;
    public static bl_ScopeProSettings Instance
    {
        get
        {
            if (m_Data == null)
            {
                m_Data = Resources.Load("ScopeProSettings", typeof(bl_ScopeProSettings)) as bl_ScopeProSettings;
            }
            return m_Data;
        }
    }
}