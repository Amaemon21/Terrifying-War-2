using UnityEngine;
using UnityEngine.UI;

public class WeaponTaker : MonoBehaviour
{
    [SerializeField] private RectTransform infoObject = null;
    [SerializeField] private Text infoText = null;
    [SerializeField] private Transform weaponContainer = null;
    [SerializeField] private float takeRange = 2.5f;
    [SerializeField] private LayerMask playerMask = new LayerMask();

    private Transform m_MainCameraTransform = null;
    private string m_TakeButton = null;

    private void Start()
    {
        m_MainCameraTransform = Camera.main.transform;
        m_TakeButton = $"<color=\"#FFAA00\">{ButtonsManager.Instance.TakeItemButton}</color>";
    }

    private void Update()
    {
        if (Physics.Raycast(m_MainCameraTransform.position, m_MainCameraTransform.forward, out RaycastHit hit, takeRange, playerMask))
        {
            if (hit.transform.tag == "WeaponItem")
            {
                if (hit.transform.GetComponent<WeaponItem>() != null)
                {
                    WeaponItem currentWeaponItem = hit.transform.GetComponent<WeaponItem>();

                    DisplayInfoText(currentWeaponItem);
                    Take(currentWeaponItem);
                }
            }
            else
            {
                infoObject.GetComponent<Image>().enabled = false;
                infoText.text = null;
            }
        }
        else
        {
            infoObject.GetComponent<Image>().enabled = false;
            infoText.text = null;
        }
    }

    private void Take(WeaponItem currentWeapon)
    {
        if (Input.GetKeyDown(ButtonsManager.Instance.TakeItemButton))
        {
            Instantiate(currentWeapon.Prefab, weaponContainer);

            SwitchWeapon switchWeapon = weaponContainer.GetComponent<SwitchWeapon>();
            switchWeapon.SelectWeapon();

            Destroy(currentWeapon.gameObject);
        }
    }

    private void DisplayInfoText(WeaponItem currentWeapon)
    {
        string weaponName = $"<color=\"#FFAA00\">{currentWeapon.Name}</color>";

        infoObject.GetComponent<Image>().enabled = true;
        infoText.text = "[" + m_TakeButton + "] " + weaponName;
    }
}
