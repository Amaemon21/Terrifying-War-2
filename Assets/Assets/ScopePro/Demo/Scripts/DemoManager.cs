using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.Demo.ScopePro
{
    public class DemoManager : MonoBehaviour
    {
        public List<DemoState> demoStates = new List<DemoState>();
        public Vector2 cameraXLimits;
        public Vector2 cameraYLimits = new Vector2(-0.05f, 0.05f);
        public Camera m_camera;
        public CanvasGroup blackAlpha;
        public Text exampleText;
        [SerializeField] private Animator transitionAnimator = null;
        public AnimationCurve movementCurve;
        public MeshRenderer customizableCobra;

        private bool canMoveCamera = false;
        private int currentDemo = -1;
        private Vector3 defaultPosition;
        private void Awake()
        {
            defaultPosition = transform.position;
            Change(true);
        }

        public void Change(bool forward)
        {
            canMoveCamera = false;
            if (forward) { currentDemo = (currentDemo + 1) % demoStates.Count; }
            else { if (currentDemo > 0) { currentDemo--; } else { currentDemo = demoStates.Count - 1; } }

            StopAllCoroutines();
            StartCoroutine(DoTransition());
        }

        IEnumerator DoTransition()
        {
            exampleText.text = demoStates[currentDemo].Name.ToUpper();

            transitionAnimator.Play("Transition", 0, 0);
            yield return new WaitForSeconds(0.5f);
            transform.position = defaultPosition;
            foreach (var item in demoStates)
            {
                if (item.enableOnStart != null) item.enableOnStart.SetActive(false);
                if (item.enableOnFinish != null) item.enableOnFinish.SetActive(false);
            }
            if (demoStates[currentDemo].enableOnStart != null) { demoStates[currentDemo].enableOnStart.SetActive(true); }

            canMoveCamera = demoStates[currentDemo].freeCameraMovement;
            if(demoStates[currentDemo].enableOnFinish != null) { demoStates[currentDemo].enableOnFinish.SetActive(true); }
        }

        private void Update()
        {
            if (canMoveCamera)
            {
                Vector3 viewPoint = m_camera.ScreenToViewportPoint(Input.mousePosition);
                Vector3 offset = Vector3.zero;
                offset.x = Mathf.Lerp(cameraXLimits.x, cameraXLimits.y, viewPoint.x);
                offset.y = Mathf.Lerp(cameraYLimits.x, cameraYLimits.y, viewPoint.y);
                transform.position = Vector3.Lerp(transform.position, demoStates[currentDemo].CameraPosition - offset, Time.deltaTime * 5);
            }
        }

        public void SetCobraColor(Image img)
        {
            customizableCobra.sharedMaterial.SetColor("_Color", img.color);
        }

        [System.Serializable]
        public class DemoState
        {
            public string Name;
            public Vector3 CameraPosition;
            public Vector3 CameraRotation;
            public bool freeCameraMovement = false;
            public GameObject enableOnStart;
            public GameObject enableOnFinish;
        }
    }
}