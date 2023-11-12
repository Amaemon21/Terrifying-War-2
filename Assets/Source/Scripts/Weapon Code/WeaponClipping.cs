using UnityEngine;

public class WeaponClipping : MonoBehaviour
{
    [SerializeField] private float m_Speed = 10.0f;
    [SerializeField] private float m_ClippingRange = 1.0f;

    [Space]
    [SerializeField] private Vector3 m_ClippingOffset = Vector3.zero;
    [SerializeField] private Vector3 m_DefaultOffset = Vector3.zero;

    [Space]
    [SerializeField] private LayerMask m_PlayerMask;

    private Transform m_MainCameraTransform = null;

    private void Awake()
    {
        m_MainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Ray ray = new Ray(m_MainCameraTransform.position, m_MainCameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, m_ClippingRange, m_PlayerMask))
        {
            if (hit.transform.gameObject)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, m_ClippingOffset, m_Speed * Time.deltaTime);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, m_DefaultOffset, m_Speed * Time.deltaTime);
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_DefaultOffset, m_Speed * Time.deltaTime);
        }
    }
}
