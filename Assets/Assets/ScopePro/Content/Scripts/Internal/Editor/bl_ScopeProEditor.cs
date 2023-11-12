using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(bl_ScopePro))]
public class bl_ScopeProEditor : Editor
{
    public bl_ScopePro script;
    public Texture2D[] reticles;
    private readonly Color shadeColor = new Color(0, 0, 0, 0.3f);
    private Vector2 listScroll;

    public Texture2D selectedReticle;
    public GameObject defaultGlassMesh;
    public GameObject uvGlassMesh;

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        script = (bl_ScopePro)target;
        reticles = bl_ScopeProSettings.Instance.scopeReticles;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnInspectorGUI()
    {
        EditorStyles.helpBox.richText = true;
        if (script.scopeCamera == null)
        {
            DrawSetup();
            return;
        }
        if (script._postSetup)
        {
            DrawPostSetup();
            return;
        }

        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void DrawSetup()
    {
        EditorGUILayout.HelpBox("1. To setup this scope, first select or assign a scope reticle texture below", MessageType.Info);
        listScroll = GUILayout.BeginScrollView(listScroll);
        var listRect = EditorGUILayout.BeginHorizontal();
        {
            for (int i = 0; i < reticles.Length; i++)
            {
                var r = GUILayoutUtility.GetRect(75, 75);
                EditorGUI.DrawRect(r, shadeColor);
                GUI.DrawTexture(r, reticles[i], ScaleMode.ScaleToFit);
                if (GUI.Button(r, GUIContent.none, GUIStyle.none))
                {
                    selectedReticle = reticles[i];
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        selectedReticle = EditorGUILayout.ObjectField("Reticle", selectedReticle, typeof(Texture2D), false, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Texture2D;
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("2. Assign the default scope glass mesh below", MessageType.Info);
        defaultGlassMesh = EditorGUILayout.ObjectField("Default Glass", defaultGlassMesh, typeof(GameObject), true) as GameObject;

        GUILayout.Space(10);
        EditorGUILayout.HelpBox("3. Assign the scope glass mesh with the compatible UV <i>(if the default mesh works, then simply duplicate that object and assign the duplicate here)</i>", MessageType.Info);
        uvGlassMesh = EditorGUILayout.ObjectField("Scope Pro Glass", uvGlassMesh, typeof(GameObject), true) as GameObject;

        GUILayout.Space(10);
        GUI.enabled = selectedReticle != null && defaultGlassMesh != null && uvGlassMesh != null;
        if (GUILayout.Button("Setup Scope"))
        {
            RunSetup();
        }
        GUI.enabled = true;
    }

    void DrawPostSetup()
    {
        EditorGUILayout.HelpBox($"The scope setup has been created!\nNow you have to adjust somethings manually.\n\n1. place the <b>Scope Camera</b> at the end and center of the scope model (refer to the documentation for more info).\n2. Adjust the default zoom by adjusting the Field Of View of the Scope Camera.\n3. Adjust the scope shader effect in the <b>{script.proScopeGlass.name}</b> object.", MessageType.Info);
        GUILayout.Space(10);
        if (GUILayout.Button("Understood, Continue >"))
        {
            script._postSetup = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RunSetup()
    {
        var render = uvGlassMesh.GetComponent<Renderer>();
        if(render == null)
        {
            Debug.LogError("Scope Pro Glass object does not have a renderer component");
            return;
        }
        
        string refPath = AssetDatabase.GetAssetPath(bl_ScopeProSettings.Instance);
        refPath = Directory.GetParent(refPath).Parent.FullName;
        refPath = Path.Combine(refPath, @"Art/Material/Customs/");
        refPath += $"{bl_ScopeProSettings.Instance.scopeMaterial.name}.mat";
        refPath = "Assets" + refPath.Substring(Application.dataPath.Length);
        
        Material newMat = new Material(bl_ScopeProSettings.Instance.scopeMaterial);
        newMat.SetTexture("_Reticle", selectedReticle);
        refPath = AssetDatabase.GenerateUniqueAssetPath(refPath);

        AssetDatabase.CreateAsset(newMat, refPath);
        newMat = AssetDatabase.LoadAssetAtPath(refPath, typeof(Material)) as Material;

        render.sharedMaterial = newMat;
        EditorUtility.SetDirty(render);

        var parent = new GameObject("Scope Pro Setup");
        parent.transform.parent = defaultGlassMesh.transform.parent;
        parent.transform.localPosition = Vector3.zero;
        parent.transform.localRotation = Quaternion.identity;

        if (!PrefabUtility.IsPartOfPrefabInstance(defaultGlassMesh))
        {
            defaultGlassMesh.transform.parent = parent.transform;
        }
        if (!PrefabUtility.IsPartOfPrefabInstance(uvGlassMesh))
        {
            uvGlassMesh.transform.parent = parent.transform;
        }
        
        if (!uvGlassMesh.name.Contains("(Pro Glass)"))
        {
            uvGlassMesh.name = uvGlassMesh.name += " (Pro Glass)";
        }

        var scopeCamera = PrefabUtility.InstantiatePrefab(bl_ScopeProSettings.Instance.scopeCamera) as GameObject;
        PrefabUtility.UnpackPrefabInstance(scopeCamera, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        scopeCamera.transform.parent = parent.transform;
        scopeCamera.transform.position = defaultGlassMesh.transform.position;
        scopeCamera.transform.rotation = Quaternion.LookRotation(defaultGlassMesh.transform.forward);

        script.normalScopeGlass = defaultGlassMesh;
        script.proScopeGlass = uvGlassMesh;
        script.scopeCamera = scopeCamera.GetComponent<Camera>();
        script._postSetup = true;
        defaultGlassMesh.SetActive(false);
        uvGlassMesh.SetActive(true);
        
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

}