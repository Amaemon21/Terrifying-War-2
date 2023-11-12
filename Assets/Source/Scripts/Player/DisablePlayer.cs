using UnityEngine;

public class DisablePlayer : MonoBehaviour
{
    [SerializeField] private GameObject _weaponHolder;

    private FPSController _fpsController;

    private void Awake()
    {
        _fpsController = GetComponent<FPSController>();
    }

    public void Enable()
    {
        _fpsController.enabled = true;
        _weaponHolder.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Disable()
    {
        _fpsController.enabled = false;
        _weaponHolder.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}