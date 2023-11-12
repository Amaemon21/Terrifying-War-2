using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lovatto.Demo.ScopePro
{
    public class DemoMouseRotator : MonoBehaviour
    {
        
        public Vector2 VerticalMinMax;
        public Vector2 HorizontalMinMax;
        public Camera m_Camera;
        private Vector3 defaultRot;

        private void Awake()
        {
            defaultRot = transform.eulerAngles;
        }
        
        private void Update()
        {
            Vector3 viewPoint = m_Camera.ScreenToViewportPoint(Input.mousePosition);
            Vector3 euler = transform.eulerAngles;
            euler.y = -Mathf.Lerp(HorizontalMinMax.x, HorizontalMinMax.y, viewPoint.x);
            euler.x = Mathf.Lerp(VerticalMinMax.x, VerticalMinMax.y, viewPoint.y);
            euler.y += 180;
            transform.eulerAngles = defaultRot + euler;
        }
    }
}