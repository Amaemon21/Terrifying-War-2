using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : MonoBehaviour
{
    [SerializeField] private Image m_WeaponIconImage;

    private Weapon m_CurrentWeaponInHand;
    private bool m_CanSwitchWeapon;
    private int m_SelectedWeapon;
    private int m_PreviousSelectedWeapon;

    private void Start()
    {
        SetState(true);

        SelectWeapon();
    }

    private void Update()
    {
        m_PreviousSelectedWeapon = m_SelectedWeapon;

        CheckInput();

        if (m_PreviousSelectedWeapon != m_SelectedWeapon)
        {
            SelectWeapon();
        }
    }

    public Weapon GetCurrentWeapon()
    {
        if (m_CurrentWeaponInHand != null) return m_CurrentWeaponInHand;
        else return null;
    }

    public void SetState(bool value) => m_CanSwitchWeapon = value;

    private void CheckInput()
    {
        if (Input.GetKeyDown(ButtonsManager.Instance.SwitchWeapon))
        {
            if (m_CanSwitchWeapon)
            {
                if (m_SelectedWeapon >= transform.childCount - 1)
                {
                    m_SelectedWeapon = 0;
                }
                else
                {
                    m_SelectedWeapon++;
                }
            }
        }
    }

    public void SelectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == m_SelectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            m_CurrentWeaponInHand = transform.GetChild(m_SelectedWeapon).GetComponent<Weapon>();
            m_WeaponIconImage.enabled = true;
            m_WeaponIconImage.sprite = m_CurrentWeaponInHand.GetWeaponIcon();

            i++;
        }
    }
}
