using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private DisablePlayer _disablePlayer;
    [SerializeField] private GameObject _sliders;
    [SerializeField] private Button _exitButton;

    private Animator _animator;

    private bool _isPause;

    private void Awake()
    {
        _exitButton.onClick.AddListener(ExitGameButton);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isPause == false) 
        { 
            _isPause = true;
        } 
        else if (Input.GetKeyDown(KeyCode.Escape) && _isPause == true) 
        { 
            _isPause = false;
        }

        if (_isPause == true) 
        {
            _animator.SetBool("isActive", true);
            _disablePlayer.Disable();
            _sliders.SetActive(true);
        }
        else if (_isPause == false)
        {
            _animator.SetBool("isActive", false);
            _disablePlayer.Enable();
            _sliders.SetActive(false);
        }
    }

    private void ExitGameButton()
    {
        SceneManager.LoadScene(0);
    }
}