using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Text m_AmmoText = null;
    [SerializeField] private GameObject m_StartShootPosition = null;
    [SerializeField] private GameObject m_Crosshair = null;
    [SerializeField] private Camera m_MainCamera = null;
    [SerializeField] private FPSController m_FirstPersonController = null;
    [SerializeField] private SwitchWeapon m_SwitchWeapon = null;
    [SerializeField] private Transform m_HudTransform = null;
    [SerializeField] private GameObject m_BulletTrailPrefab;
    [SerializeField] private GameObject m_HitMarkerPrefab;
    [SerializeField] private LayerMask m_PlayerMask = new LayerMask();
    [SerializeField] private WeaponRecoilManager m_WeaponRecoilManager = null;
    [SerializeField] private AudioSource m_AudioSource = null;
    [SerializeField] private GameObject m_ReloadingCircleUI = null;
    [SerializeField] private Transform m_CameraAnimationTransform = null;

    public Text AmmoText { get { return m_AmmoText; } set { } }
    public GameObject StartShootPosition { get { return m_StartShootPosition; } set { } }
    public GameObject Crosshair { get { return m_Crosshair; } set { } }
    public Camera MainCamera { get { return m_MainCamera; } set { } }
    public FPSController FirstPersonController { get { return m_FirstPersonController; } set { } }
    public SwitchWeapon SwitchWeapon { get { return m_SwitchWeapon; } set { } }
    public Transform HudTransform { get { return m_HudTransform; } set { } }
    public GameObject BulletTrailPrefab { get { return m_BulletTrailPrefab; } set { } }
    public GameObject HitMarkerPrefab { get { return m_HitMarkerPrefab; } set { } }
    public LayerMask PlayerMask { get { return m_PlayerMask; } set { } }
    public WeaponRecoilManager WeaponRecoilManager { get { return m_WeaponRecoilManager; } set { } }
    public AudioSource AudioSource { get { return m_AudioSource; } set { } }
    public GameObject ReloadingCircleUI { get { return m_ReloadingCircleUI; } set { } }
    public Transform CameraAnimationTransform { get { return m_CameraAnimationTransform; } set { } }
}
