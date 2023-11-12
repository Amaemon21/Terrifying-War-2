using System.Collections;
using UnityEngine;

public class TutorialInfo : MonoBehaviour
{
    [SerializeField] private int _wasdTimeStart = 10;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(_wasdTimeStart);
        _animator.SetBool("isActiv", true);
    }

    public void DestroyMe()
    {
        Destroy(gameObject, 1);
    }
}