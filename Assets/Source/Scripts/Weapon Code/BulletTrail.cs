using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletTrail : MonoBehaviour
{
    [SerializeField] private int m_Speed = 100;

    private Rigidbody m_Rigidbody = null;

    private void Start()
    {
        Destroy(gameObject, 20);

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.velocity = transform.TransformDirection(Vector3.forward * m_Speed);
    }
}
