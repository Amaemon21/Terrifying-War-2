using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponRecoilManager : MonoBehaviour
{
    private Vector3 m_TargetRotation;
    private Vector3 m_CurrentRotation;

    public void Recoil(float returnSpeed, float snappiness)
    {
        m_TargetRotation = Vector3.Lerp(m_TargetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        m_CurrentRotation = Vector3.Slerp(m_CurrentRotation, m_TargetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(m_CurrentRotation);
    }

    public void AddRecoil(float recoilX, float recoilY, float recoilZ) 
    {
        m_TargetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
