using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager Instance = null;

    public KeyCode SwitchWeapon = KeyCode.Tab;
    public KeyCode FireButton = KeyCode.Mouse0;
    public KeyCode ScopeButton = KeyCode.Mouse1;
    public KeyCode ReloadButton = KeyCode.R;
    public KeyCode JumpButton = KeyCode.Space;
    public KeyCode RunningButton = KeyCode.LeftShift;
    public KeyCode TakeItemButton = KeyCode.F;
    public KeyCode PauseMenu = KeyCode.Escape;
    public KeyCode Inspect = KeyCode.V;

    private void Awake()
    {
        Instance = this;
    }
}
