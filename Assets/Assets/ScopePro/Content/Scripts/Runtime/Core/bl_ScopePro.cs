using System;
using UnityEngine;

public class bl_ScopePro : MonoBehaviour
{
    [Serializable]
    public enum ScopeProShowMode
    {
        Always,
        OnlyWhenAiming,
    }

    #region Public members
    public ScopeProShowMode scopeProShowMode = ScopeProShowMode.Always;
    public GameObject normalScopeGlass = null;
    public GameObject proScopeGlass = null;
    public Camera scopeCamera = null;
    public Transform viewReference = null;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        // Assign a viewReference in case you want to sync the scope view with any other camera
        // e.g if you want to the scope view exactly the same as the player main camera, then assign the player main camera
        // as the viewReference transform.
        SetViewReference(viewReference);
        
        SetAim(scopeProShowMode == ScopeProShowMode.Always);
    }
    
    /// <summary>
    /// Active/Disable aiming
    /// When is aiming the scope pro glass will be used and when is not
    /// aiming, the normal scope will be used (for better performance)
    /// </summary>
    /// <param name="aiming">Is Aiming</param>
    public void SetAim(bool aiming)
    {
        if(normalScopeGlass != null)
        {
            normalScopeGlass.SetActive(!aiming);
        }
        if (proScopeGlass != null)
        {
            proScopeGlass.SetActive(aiming);
        }
    }

    /// <summary>
    /// Change the scope zoom
    /// Zoom = to the camera field of view
    /// </summary>
    /// <param name="zoom">field of view, less is equal to more zoom</param>
    public void SetScopeZoom(float zoom)
    {
        if (scopeCamera == null) return;

        scopeCamera.fieldOfView = zoom;
    }

    /// <summary>
    /// Change the reticle of the scope
    /// </summary>
    /// <param name="newReticle"></param>
    public void SetReticle(Texture2D newReticle)
    {
        if (proScopeGlass == null) return;

        var render = proScopeGlass.GetComponent<Renderer>();
        if (render != null)
        {
            // use material instead of sharedMaterial so the reticle change just for this scope instance.
            render.material.SetTexture("_Reticle", newReticle);
        }
    }

    /// <summary>
    /// Set a view reference
    /// In case you need sync the scope view with other camera view.
    /// </summary>
    /// <param name="viewRef">Object/Camera that will work as reference of direction to look at.</param>
    public void SetViewReference(Transform viewRef)
    {
        if (viewRef == null || scopeCamera == null) return;

        scopeCamera.transform.position = viewRef.position;
        scopeCamera.transform.rotation = viewRef.rotation;
    }

    #region Editor Required
    [HideInInspector] public bool _postSetup = false;
    #endregion
}