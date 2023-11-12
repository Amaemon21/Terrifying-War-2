using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Variables

    [SerializeField] private GunType m_WeaponType;
    [SerializeField] private Animator m_Animator = null;
    [SerializeField] private float m_FireRate = 600.0f;
    [SerializeField] private float m_MaxSpread = 3.5f;

    [Header("UI Setting's")]
    [SerializeField] private string m_WeaponName = "Name of Weapon";
    [SerializeField] private Sprite m_WeaponIcon = null;

    [Header("Recoil Setting's")]
    [SerializeField] private float m_RecoilX = 1.5f;
    [SerializeField] private float m_RecoilY = 2.0f;
    [SerializeField] private float m_RecoilZ = 1.0f;

    [Space]
    [SerializeField] private float m_Snappiness = 2.0f;
    [SerializeField] private float m_ReturnSpeed = 5.0f;

    [Header("Ammo Setting's")]
    [SerializeField] private int m_Ammo = 30;
    [SerializeField] private int m_AmmoInMag = 30;
    [SerializeField] private int m_ReserveAmmo = 90;
    [SerializeField] private int m_ShotgunSlugsCount = 7;

    [Header("Scope Setting's")]
    [SerializeField] private float m_NormalFOV = 75.0f;
    [SerializeField] private float m_ScopedFOV = 50.0f;
    [SerializeField] private float m_FOVSmoothing = 10.0f;

    [Space(10)]
    [SerializeField] private Vector3 m_NormalPosition;
    [SerializeField] private Vector3 m_AimingPosition;

    [Header("Animation Setting's")]
    [SerializeField] private string m_ShootName = "Fire";
    [SerializeField] private string m_ShootScopeName = "Aim Fire";
    [SerializeField] private string m_ReloadName = "Reload Ammo Left";
    [SerializeField] private string m_ReloadFullName = "Reload Out Of Ammo";
    [SerializeField] private string m_AimInName = "Aim";

    [Header("Audio Setting's")]
    [SerializeField] private AudioClip m_Empty = null;
    [SerializeField] private AudioClip m_Reload = null;
    [SerializeField] private AudioClip m_FullReload = null;
    [SerializeField] private AudioClip m_Equip = null;
    [SerializeField] private AudioClip m_Hit = null;
    [SerializeField] private AudioClip m_OpenShell = null;
    [SerializeField] private AudioClip m_CloseShell = null;

    [Space]
    [SerializeField] private bool m_UseSilencerSounds = false;

    [Space(10)]
    [SerializeField] private AudioClip[] m_Shoots = null;

    [Space(10)]
    [SerializeField] private AudioClip[] m_SilencerShoots = null;

    [Header("Muzzle Flash Setting's")]
    [SerializeField] private bool m_HaveMuzzle = true;
    [SerializeField] private float m_ScaleFactor = 1.0f;
    [SerializeField] private float m_TimeToDestroy = 2.0f;

    [Space()]
    [SerializeField] private GameObject[] m_Muzzle = null;

    [Header("Reload Time Setting's")]
    [SerializeField] private float m_ReloadTime = 2.0f;
    [SerializeField] private float m_ReloadFullTime = 3.0f;

    [Header("Transform's")]
    [SerializeField] private Transform m_ScopeShootPosition = null;
    [SerializeField] private Transform m_MuzzlePosition = null;
    [SerializeField] private Transform m_CameraAnimatorTransform = null;

    [Header("State")]
    [SerializeField] private bool m_CanShoot = true;
    [SerializeField] private bool m_CanReload = true;
    [SerializeField] private bool m_CanScope = true;
    [SerializeField] private bool m_InScope = false;
    [SerializeField] private bool m_IsReload = false;

    [Header("Silencer Setting's")]
    [SerializeField] private bool m_Silencer = false;

    [Space]
    [SerializeField] private GameObject m_SilencerObject = null;

    private float m_NextFire = 0.0f;
    private WeaponManager m_WeaponManager = null;
    private bool m_IsWalking = false;
    private bool m_IsRunning = false;

    public enum GunType { Auto, Semi, Shotgun, Bolt }

    #endregion

    private void Awake()
    {
        InitializeClass();

        SetSilencer();
    }

    private void OnEnable()
    {
        UpdateAmmoInScreen();
        PlayEquip();
    }

    private void Update()
    {
        PlayWeaponAnimations();
        Keycodes();
        Scope();

        if (Input.GetKeyDown(KeyCode.G))
        {
            m_Silencer = !m_Silencer;
            SetSilencer();
        }

        if (Input.GetKey(ButtonsManager.Instance.Inspect))
        {
            m_Animator.SetTrigger("Inspect");
        }

        m_WeaponManager.CameraAnimationTransform.localRotation = m_CameraAnimatorTransform.localRotation;
        m_WeaponManager.WeaponRecoilManager.Recoil(m_ReturnSpeed, m_Snappiness);
    }

    public Sprite GetWeaponIcon()
    {
        return m_WeaponIcon;
    }

    public bool GetState(string stateName)
    {
        if (stateName == "CanShoot") return m_CanShoot;
        else if (stateName == "CanReload") return m_CanReload;
        else if (stateName == "CanScope") return m_CanScope;
        else if (stateName == "InScope") return m_InScope;
        else if (stateName == "IsReload") return m_IsReload;
        else return false;
    }

    private void InitializeClass()
    {
        m_WeaponManager = FindObjectOfType<WeaponManager>();
    }

    private void SetSilencer()
    {
        if (m_SilencerObject != null)
        {
            m_SilencerObject.SetActive(m_Silencer);
            m_HaveMuzzle = !m_Silencer;
            m_UseSilencerSounds = m_Silencer;
        }
    }

    private void Scope()
    {
        if (m_CanScope)
        {
            m_InScope = Input.GetKey(ButtonsManager.Instance.ScopeButton);

            if (m_InScope)
            {
                m_CanReload = !m_InScope;
                Scope(true);
                m_WeaponManager.SwitchWeapon.SetState(false);
                m_WeaponManager.Crosshair.SetActive(!m_InScope);
            }
            else
            {
                m_CanReload = !m_InScope;
                Scope(false);
                m_WeaponManager.SwitchWeapon.SetState(true);
                m_WeaponManager.Crosshair.SetActive(!m_InScope);
            }   
        }

        FOVChange();
    }

    private void FOVChange()
    {
        if (m_InScope)
        {
            m_WeaponManager.MainCamera.fieldOfView = Mathf.Lerp(m_WeaponManager.MainCamera.fieldOfView, m_ScopedFOV, m_FOVSmoothing * Time.deltaTime);
            m_WeaponManager.FirstPersonController.SetSensitivityAim(m_WeaponManager.FirstPersonController._aimSensitivity);

            transform.localPosition = Vector3.Lerp(transform.localPosition, m_AimingPosition, Time.deltaTime * m_FOVSmoothing);
        }
        else
        {
            m_WeaponManager.MainCamera.fieldOfView = Mathf.Lerp(m_WeaponManager.MainCamera.fieldOfView, m_NormalFOV, m_FOVSmoothing * Time.deltaTime);
            m_WeaponManager.FirstPersonController.SetSensitivityMouse(m_WeaponManager.FirstPersonController._mouseSensitivity);

            transform.localPosition = Vector3.Lerp(transform.localPosition, m_NormalPosition, Time.deltaTime * m_FOVSmoothing);
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        if (m_WeaponType != GunType.Shotgun)
        {
            if (m_AmmoInMag <= 0)
            {
                ReloadFull();
                PlayFullReload();

                yield return new WaitForSeconds(m_ReloadFullTime);
            }
            else if (m_AmmoInMag > 0)
            {
                Reload();
                PlayReload();

                yield return new WaitForSeconds(m_ReloadTime);
            }
        }
        else if (m_WeaponType == GunType.Shotgun)
        {
            if (m_AmmoInMag == 4)
            {
                m_Animator.Play("Reload Open 1");

                yield return new WaitForSeconds(2.533f);
            }
            else if (m_AmmoInMag == 3)
            {
                m_Animator.Play("Reload Open 2");

                yield return new WaitForSeconds(3.266f);
            }
            else if (m_AmmoInMag == 2)
            {
                m_Animator.Play("Reload Open 3");

                yield return new WaitForSeconds(4.0f);
            }
            else if (m_AmmoInMag == 1)
            {
                m_Animator.Play("Reload Open 4");

                yield return new WaitForSeconds(4.733f);
            }
            else if (m_AmmoInMag == 0)
            {
                m_Animator.Play("Reload Open 5");

                yield return new WaitForSeconds(5.466f);
            }
        }

        AddAmmo();
        UpdateAmmoInScreen();

        m_IsReload = false;
        m_CanReload = true;
        m_CanScope = true;
        m_CanShoot = true;

        m_WeaponManager.SwitchWeapon.SetState(true);

        m_WeaponManager.ReloadingCircleUI.SetActive(false);

        m_WeaponManager.Crosshair.SetActive(!m_IsReload);
    }

    private void RayCasting(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<Impact>() != null)
            {
                Impact currentImpact = hit.transform.GetComponent<Impact>();

                if (currentImpact.impactPrefab != null)
                {
                    GameObject spawnedObject = Instantiate(currentImpact.impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(spawnedObject, 5.0f);
                }
            }
            if (hit.transform.GetComponent<Target>() != null)
            {
                hit.transform.GetComponent<Target>().Down();

                GameObject hitMarkerObject = Instantiate(m_WeaponManager.HitMarkerPrefab, m_WeaponManager.HudTransform);

                Hit();

                Destroy(hitMarkerObject, 0.25f);
            }

            if(hit.transform.GetComponent<ExplosiveBarrelScript>() != null)
            {
                hit.transform.GetComponent<ExplosiveBarrelScript>().explode = true;

                GameObject hitMarkerObject = Instantiate(m_WeaponManager.HitMarkerPrefab, m_WeaponManager.HudTransform);

                Hit();

                Destroy(hitMarkerObject, 0.25f);
            }
        }
    }

    private void Keycodes()
    {
        if (m_CanShoot == true)
        {
            if (m_AmmoInMag > 0)
            {
                if (Time.time - m_NextFire > 1 / (m_FireRate / 60))
                {
                    if (m_WeaponType == GunType.Auto)
                    {
                        if (Input.GetKey(ButtonsManager.Instance.FireButton))
                        {
                            Shoot();
                        }
                    }
                    else if (m_WeaponType == GunType.Semi || m_WeaponType == GunType.Shotgun)
                    {
                        if (Input.GetKeyDown(ButtonsManager.Instance.FireButton))
                        {
                            Shoot();
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(ButtonsManager.Instance.FireButton))
        {
            if (m_AmmoInMag <= 0)
            {
                if (!m_IsReload)
                {
                    PlayEmptySound();
                }
            }
        }

        if (m_CanReload == true)
        {
            if (m_ReserveAmmo > 0 && m_AmmoInMag < m_Ammo && m_AmmoInMag > 0)
            {
                if (Input.GetKeyDown(ButtonsManager.Instance.ReloadButton))
                {
                    if (m_InScope == false)
                    {
                        Check();
                    }
                }
            }
            else if (m_ReserveAmmo > 0 && m_AmmoInMag < m_Ammo && m_AmmoInMag == 0)
            {
                if (Input.GetKeyDown(ButtonsManager.Instance.ReloadButton))
                {
                    if (m_InScope == false)
                    {
                        Check();
                    }
                }
            }
        }
    }

    private void Check()
    {
        m_CanShoot = false;
        m_CanScope = false;
        m_CanReload = false;
        m_IsReload = true;

        m_WeaponManager.SwitchWeapon.SetState(false);

        m_WeaponManager.ReloadingCircleUI.SetActive(true);

        m_WeaponManager.Crosshair.SetActive(!m_IsReload);

        StartCoroutine(ReloadCoroutine());
    }

    private void Shoot()
    {
        m_NextFire = Time.time;

        SetSilencer();

        if (m_WeaponType == GunType.Shotgun)
        {
            for (int i = 0; i < m_ShotgunSlugsCount; i++)
            {
                Ray ray = new Ray(m_WeaponManager.StartShootPosition.transform.position, GenerateRandomVectorToSpread());
                Instantiate(m_WeaponManager.BulletTrailPrefab, m_MuzzlePosition.position, m_MuzzlePosition.rotation);
                RayCasting(ray);

                m_WeaponManager.StartShootPosition.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (m_InScope == true)
            {
                if (m_ScopeShootPosition != null)
                {
                    Ray ray = new Ray(m_WeaponManager.StartShootPosition.transform.position, m_WeaponManager.StartShootPosition.transform.forward);
                    Instantiate(m_WeaponManager.BulletTrailPrefab, m_MuzzlePosition.position, m_MuzzlePosition.rotation);
                    RayCasting(ray);
                }
                else
                {
                    Ray ray = new Ray(m_WeaponManager.StartShootPosition.transform.position, m_WeaponManager.StartShootPosition.transform.forward);
                    Instantiate(m_WeaponManager.BulletTrailPrefab, m_MuzzlePosition.position, m_MuzzlePosition.rotation);
                    RayCasting(ray);
                }
            }
            else
            {
                Ray ray = new Ray(m_WeaponManager.StartShootPosition.transform.position, GenerateRandomVectorToSpread());
                Instantiate(m_WeaponManager.BulletTrailPrefab, m_MuzzlePosition.position, m_WeaponManager.StartShootPosition.transform.rotation);
                RayCasting(ray);

                m_WeaponManager.StartShootPosition.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        PlayShootSound();
        SpawnMuzzle();

        m_WeaponManager.WeaponRecoilManager.AddRecoil(m_RecoilX, m_RecoilY, m_RecoilZ);

        m_AmmoInMag--;
        UpdateAmmoInScreen();

        if (m_InScope) ScopeShoot();
        else ShootAnimation();
    }

    private Vector3 GenerateRandomVectorToSpread()
    {
        float currentSpread = m_MaxSpread;

        float x = m_WeaponManager.StartShootPosition.transform.localRotation.x + Random.Range(-currentSpread, currentSpread);
        float y = m_WeaponManager.StartShootPosition.transform.localRotation.y + Random.Range(-currentSpread, currentSpread);
        float z = m_WeaponManager.StartShootPosition.transform.localRotation.z + Random.Range(-currentSpread, currentSpread);

        m_WeaponManager.StartShootPosition.transform.localRotation = Quaternion.Euler(x, y, z);

        Vector3 forward = m_WeaponManager.StartShootPosition.transform.TransformDirection(Vector3.forward);

        return forward;
    }

    private void SpawnMuzzle()
    {
        if (m_HaveMuzzle == true)
        {
            GameObject currentMuzzle = m_Muzzle[Random.Range(0, m_Muzzle.Length)];
            GameObject spawnedMuzzle = Instantiate(currentMuzzle, m_MuzzlePosition.position, m_MuzzlePosition.rotation);
            spawnedMuzzle.transform.localScale = new Vector3(m_ScaleFactor, m_ScaleFactor, m_ScaleFactor);
            Destroy(spawnedMuzzle, m_TimeToDestroy);
        }
    }

    private void PlayShootSound()
    {
        AudioClip clip = null;

        if (!m_UseSilencerSounds) clip = m_Shoots[Random.Range(0, m_Shoots.Length)];
        else clip = m_SilencerShoots[Random.Range(0, m_SilencerShoots.Length)];

        m_WeaponManager.AudioSource.clip = clip;
        m_WeaponManager.AudioSource.Play();
    }

    private void PlayEmptySound() => m_WeaponManager.AudioSource.PlayOneShot(m_Empty);
    private void PlayFullReload() => m_WeaponManager.AudioSource.PlayOneShot(m_FullReload);
    private void PlayReload() => m_WeaponManager.AudioSource.PlayOneShot(m_Reload);
    private void PlayOpenShell() => m_WeaponManager.AudioSource.PlayOneShot(m_OpenShell);
    private void PlayCloseShell() => m_WeaponManager.AudioSource.PlayOneShot(m_CloseShell);
    private void PlayEquip() => m_WeaponManager.AudioSource.PlayOneShot(m_Equip);
    private void Hit() => m_WeaponManager.AudioSource.PlayOneShot(m_Hit);

    private void Scope(bool value) => m_Animator.SetBool(m_AimInName, value);
    private void ShootAnimation() => m_Animator.Play(m_ShootName);
    private void ScopeShoot() => m_Animator.Play(m_ShootScopeName);
    private void Reload() => m_Animator.Play(m_ReloadName);
    private void ReloadFull() => m_Animator.Play(m_ReloadFullName);

    protected internal void PlayWeaponAnimations()
    {
        m_IsWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        if (!m_InScope) m_IsRunning = Input.GetKey(ButtonsManager.Instance.RunningButton);

        if (m_IsWalking == true)
        {
            m_Animator.SetBool("Walk", m_IsWalking);

            if (m_IsRunning == true)
            {
                m_Animator.SetBool("Run", m_IsWalking);

                m_CanReload = false;
                m_CanShoot = false;
                m_CanScope = false;
                m_InScope = false;
            }
            else
            {
                m_Animator.SetBool("Run", false);

                if (!m_IsReload)
                {
                    m_CanReload = true;
                    m_CanShoot = true;
                    m_CanScope = true;
                }
            }
        }
        else
        {
            m_Animator.SetBool("Walk", false);
            m_Animator.SetBool("Run", false);
        }
    }

    private void AddAmmo()
    {
        int amountNeeded = m_Ammo - m_AmmoInMag;

        if (amountNeeded >= m_ReserveAmmo)
        {
            m_AmmoInMag += m_ReserveAmmo;
            m_ReserveAmmo -= amountNeeded;
        }
        else
        {
            m_AmmoInMag = m_Ammo;
            m_ReserveAmmo -= amountNeeded;
        }

        UpdateAmmoInScreen();
    }

    private void UpdateAmmoInScreen()
    {
        string reserveAmmo = null;
        string ammoInMag = null;

        if (m_AmmoInMag == 0) ammoInMag = $"<size=\"260\"><color=\"#FF0000\">{m_AmmoInMag}</color></size>";
        else if (m_AmmoInMag > 0) ammoInMag = $"<size=\"260\"><color=\"#FFFFFF\">{m_AmmoInMag}</color></size>";

        if (m_ReserveAmmo == 0) reserveAmmo = $"<color=\"#FF0000\">{m_ReserveAmmo}</color>";
        else if (m_ReserveAmmo > 0) reserveAmmo = $"<color=\"#FFFFFF\">{m_ReserveAmmo}</color>";


        if (m_AmmoInMag <= 0) m_AmmoInMag = 0;
        if (m_ReserveAmmo <= 0) m_ReserveAmmo = 0;
        if (m_WeaponManager.AmmoText != null) m_WeaponManager.AmmoText.text = ammoInMag + " | " + reserveAmmo;
    }

    public void InsertShell() => PlayReload();
    public void Open() => PlayOpenShell();
    public void Close() => PlayCloseShell();
}
