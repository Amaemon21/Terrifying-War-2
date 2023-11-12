using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    [Range(1, 60)]
    [SerializeField] private int m_TimeToChangeTargetState = 60;

    [Space]
    [SerializeField] private AudioClip m_TargetDownSound = null;
    [SerializeField] private AudioClip m_TargetUpSound = null;

    [SerializeField] private ScoreCounter m_ScoreCounter;

    private Animator m_TargetAnimator = new Animator();
    private AudioSource m_AudioSource = new AudioSource();

    public bool CurrentState { get; protected set; } = false;
    public bool CanChangeState { get; protected set; } = true;

    private void Awake()
    {
        m_TargetAnimator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void Down()
    {
        if (CanChangeState == true)
        {
            CurrentState = true;

            m_AudioSource.PlayOneShot(m_TargetDownSound);

            m_TargetAnimator.SetBool("TargetDown", CurrentState);

            StartCoroutine(IE_ChangeState());

            m_ScoreCounter.AddScore(1);

            CanChangeState = false;
        }
    }

    private IEnumerator IE_ChangeState()
    {
        yield return new WaitForSeconds(m_TimeToChangeTargetState);

        Up();
    }

    private void Up()
    {
        if (CanChangeState == false)
        {
            CurrentState = false;

            m_AudioSource.PlayOneShot(m_TargetUpSound);

            m_TargetAnimator.SetBool("TargetDown", CurrentState);

            CanChangeState = true;
        }
    }
}
