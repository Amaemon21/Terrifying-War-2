using UnityEngine;
using UnityEditor;

namespace ScopePro.TutorialWizard
{
    public class DocumentationScopePro : TutorialWizard
    {
        //required//////////////////////////////////////////////////////
        public string FolderPath = "scope-pro/editor/";
        public NetworkImages[] m_ServerImages = new NetworkImages[]
        {
        new NetworkImages{Name = "img-0.png", Image = null},
        new NetworkImages{Name = "img-1.png", Image = null},
        new NetworkImages{Name = "img-2.png", Image = null},
        new NetworkImages{Name = "img-3.png", Image = null},
        new NetworkImages{Name = "img-4.png", Image = null},
        };
        public Steps[] AllSteps = new Steps[] {
    new Steps { Name = "Get Started", StepsLenght = 0 , DrawFunctionName = nameof(GetStartedDoc)},
    new Steps { Name = "Add Red Dot Effect", StepsLenght = 0, DrawFunctionName = nameof(SecondSection) },
    new Steps { Name = "Add Scope Effect", StepsLenght = 0, DrawFunctionName = nameof(AddScopeEffectDoc) },
    new Steps { Name = "Compatible Glass Mesh", StepsLenght = 0, DrawFunctionName = nameof(CompatibleGlassDoc) },
    new Steps { Name = "Code Integration", StepsLenght = 0, DrawFunctionName = nameof(CodeIntegrationDoc) },
    new Steps { Name = "URP Usage", StepsLenght = 0, DrawFunctionName = nameof(URPUsage) },
    new Steps { Name = "HDRP Usage", StepsLenght = 0, DrawFunctionName = nameof(HDRPsage) },
    };
        private readonly GifData[] AnimatedImages = new GifData[]
       {
        new GifData{ Path = "spssirp.gif" },
       };

        public override void OnEnable()
        {
            base.OnEnable();
            base.Initizalized(m_ServerImages, AllSteps, FolderPath, AnimatedImages);
            Style.highlightColor = ("#c9f17c").ToUnityColor();
            allowTextSuggestions = true;
        }

        public override void WindowArea(int window)
        {
            AutoDrawWindows();
        }
        //final required////////////////////////////////////////////////

        void GetStartedDoc()
        {
            DrawText("Scope Pro contains two shaders for two scope/sight effects, which one you use depends first on what you want and second on the type of scope/sight.\n \nThe first effect is the <b>Red Dot reticle effect</b>, a simple object space reticle that simulates the real red dot sights movement effect, this usually is used for simply ads <i>(Aim Dot Sight)</i> like Reflex, Holographic, Cobra, etc... ads that don't have a zoom effect, just the reticle and maybe a glass tint.\n \nThe second effect is a <b>Scope Sight effect</b>, the same object space reticle movement effect but with this shader effect you can add a magnification/zoom and scope barrel depth effect simulating a real scope, you should use this effect for any scope/ads that require a magnification/zoom.\n \nAdding either of the effects to your scope/ads is easy but they have a different integration, in the next sections you will find the tutorial for integrating each of them.");
        }

        void SecondSection()
        {
            DrawText("<i>The <b>Red Dot effect</b> simulates a realistic reticle projection, this effect doesn't add any magnification, its usage is targeted for a specific group of aim dot sights like Reflex, Holographic, Cobra, etc... ads that don't have any magnification/zoom effect.</i>\n \nTo add this effect to your aim dot sight all you need to make sure is that the glass mesh is compatible <i>(check the <b>Compatible Glass Mesh</b> section)</i>, then in the glass mesh material > select the shader in <b>Scope Pro > Red Dot Effect</b>");
            DrawServerImage("img-6.png");
            DrawText("Then use the editor view focusing on the sight glass mesh closely you can modify the material properties to adjust and customize the red dot effect, first assign a reticle texture, the asset come with some that you can use located in the folder <i>Assets ➔ ScopePro ➔ Content ➔ Art ➔ Textures ➔ RedDots➔*</i>, assign one of these or you custom reticle texture then modify all the other properties until the desired result is reached.");
            DrawServerImage("img-7.png");
        }

        void AddScopeEffectDoc()
        {
            DrawText("<i>The <b>Scope effect</b> tries to simulate a realistic scope with magnification and barrel depth effect, you can use it for any scope/sight that requires a magnification/zoom.</i>\n \nAdding this to a new scope/sight requires setting up a few more things than the <i>Red Dot effect</i>, but the custom inspector will automate most of the work so you only have to assign the required references, the only requirement just like the <i>Red Dot effect</i> is that your scope/sight glass mesh have to be compatible <i>(check the <b>Compatible Glass Mesh</b> section)</i>, if this in comply, then you can integrate like this:");

            DrawText("<b>1.</b> Make sure your scope glass mesh is compatible, if it is not or you aren't sure check the <b>Compatible Glass Mesh</b> section and add a compatible glass mesh if required inside the scope model instance.");
            DrawText("<b>2.</b> In your scope/sight object in the <b>Hierarchy</b> window attach the script <b>bl_ScopePro</b>.");
            DrawServerImage("img-8.png");
            DrawText("3. <b>In the inspector of bl_ScopePro</b> you will some instructions on the references that you need to assign:\n \n<b>- Reticle:</b> you will have a list of the asset reticle textures that you can use (click over the reticle to select) or you can assign your custom reticle texture in the Reticle field.\n \n<b>- Default Glass:</b> The default scope/sight glass mesh, will be used in case you only want to show the scope effect in certain moments <i>e.g</i> only when the player is aiming and if not then show the default glass mesh texture, this is a recommended to improve performance. This glass mesh should be inside the scope model instance as well <i>(don't drag the glass mesh from the Project View window).</i>\n \n<b>- Scope Pro Glass:</b> The effect compatible glass mesh, if you don't know what this is, check the <b>Compatible Glass Mesh</b> section. This along with the default glass mesh should be placed inside the scope/sight model instance.");
            DrawServerImage("img-9.png");
            DrawText("Once you assign the required references in their respective field ➔ Click on the <b>Setup Scope</b> button ➔ This will prompt another indication, read it and click on the <b>Continue</b> button.");
            DrawServerImage("img-10.png");
            DrawText("<b>4.</b> For this step, depending on how the scopes/sights works in your game you have two options, view in the scope direction or the player camera direction:\n \n- <b>In case the scope view should look at the scope forward direction</b> <i>(typically for VR games)</i> then you have to move the scope camera position to the end and center of the scope barrel, for this simply select the <b>Scope Camera</b> object in the hierarchy window that is located inside <b>your scope/sight model > Scope Pro Setup > Scope Camera</b>, move that transform and place at the end of the scope barrel ➔ then rotate it to look straight in the scope direction");
            DrawServerImage("img-11.png");
            DrawText("- <b>In case the scope view should look at the player camera forward direction</b>, typically for shooter games where the scope should aim to where the player is looking, then you simply need to assign your <b>Player Camera transform</b> in the <b>View Reference</b> field in the inspector of <b>bl_ScopePro</b> and that's.");
        }
        
        void CompatibleGlassDoc()
        {
            DrawText("In order for both effects <i>(Red Dot and Scope Effects)</i> works with your custom scope/sight model you just have to make sure the glass mesh is compatible, how do you verify that?\n \n- The glass should be separated/detached from the main scope/sight mesh and have its own mesh instance inside the scope model.\n \n- The glass mesh should have its own UV and not be shared with any other polygons outside the glass mesh, the UV should not be stretched, you can check if the UV works by assigning one of the Red Dot or Scope effect materials located in <i>Assets ➔ ScopePro ➔ Content ➔ Art ➔ Material➔*</i>, if after assigning the material to the glass mesh you can't see the reticle or its not in the center when looking at the mesh straight then it is not compatible and you will have to use one of the compatible meshes.");
            DrawServerImage("img-12.png");
            DrawText("In case your scope/sight default glass mesh is not compatible you can use one of the glass meshes included with the package like this:\n \nDrag one of the glass prefabs located in the folder <i>Assets ➔ ScopePro ➔ Content ➔ Prefabs ➔ Glasses➔*</i>, there are glass meshes for various scope shapes so use the one that fits better with your scope/sight model, drag and drop it inside your scope/sight model instanced in the hierarchy window > then positioned, rotate, and scale it in the same place as your default glass mesh <i>(just slightly in front so it renders in front of the default glass)</i>, once you positioned it correctly, that's, you have the compatible glass mesh ready to integrate the effect.");
        }

        void CodeIntegrationDoc()
        {
            DrawText("The scope effect has some useful features that you can use like changing the reticle, zooming in runtime, or showing the default scope mesh when the player is not focusing on the scope, this last one is a recommended implementation to save some performance, e.g you can show the scope pro effect only when the player is aiming and show the default glass mesh when is not aiming.");
            DrawAnimatedImage(0);
            DrawText("These features require integrating a few code lines into your game scripts, all the required functions are referenced in the <b>bl_ScopePro.cs</b> script, all you need to do is reference the <b>bl_ScopePro</b> in your custom script and then called one of these functions when required: <b>SetAim(bool aiming), SetScopeZoom(float zoom), SetReticle(Texture2D newReticle), SetViewReference(Transform viewRef)</b>, e.g:");
            DrawCodeText("public class DemoManager : MonoBehaviour\n    {\n        public bl_ScopePro scopeProInstance; //Assing it in the inspector\n \n        void Update()\n        {\n            if (Input.GetMouseButtonDown(1))\n            {\n                scopeProInstance.SetAim(true);\n            }\n            else if (Input.GetMouseButtonUp(1))\n            {\n                scopeProInstance.SetAim(false);\n            }\n        }\n    }");
        }

        void URPUsage()
        {
            DrawText("The package supports the Universal Render Pipeline (URP), the integration is practically the same as in the Built-In render pipeline with the only difference that you have to select the right shader for URP.\n \nFor the Red Dot shader, there's no difference, it works right out of the box in all pipelines, so you don't have to do anything else to integrate it.\n \nFor the scope effect, you just have to select the <b>Scope Effect URP</b> shader instead of <b>Scope Effect</b>:");
            DrawServerImage("img-13.png");
            DrawNote("Adjust the <b>Scope Apperture, Depths, and Fade</b> properties in the material inspectors since by default you may see a black circle in the middle of the scope, adjusting these values fix that.");
        }

        void HDRPsage()
        {
            DrawText("The package supports the High Definition Render Pipeline (HDRP), the integration is practically the same as in the Built-In render pipeline with the only difference that you have to select the right shader for URP.\n \nFor the Red Dot shader, there's no difference, it works right out of the box in all pipelines, so you don't have to do anything else to integrate it.\n \nFor the scope effect, you just have to select the <b>Scope Effect HDRP</b> shader instead of <b>Scope Effect</b>:");
            DrawServerImage("img-14.png");
            DrawNote("Adjust the <b>Scope Apperture, Depths, and Fade</b> properties in the material inspectors since by default you may see a black circle in the middle of the scope, adjusting these values fix that.");
        }

        [MenuItem("Window/Documentation/Scope Pro")]
        static void Open()
        {
            GetWindow<DocumentationScopePro>();
        }
    }
}