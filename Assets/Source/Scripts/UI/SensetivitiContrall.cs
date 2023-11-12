using UnityEngine;
using UnityEngine.UI;

public class SensetivitiContrall : MonoBehaviour
{
    [SerializeField] private Slider _sensitivity;
    [SerializeField] private Text _sensitivityText;

    [SerializeField] private Slider _sensitivityAim;
    [SerializeField] private Text _sensitivityAimText;

    private FPSController _controller;

    private void Start()
    {
        _controller = GetComponent<FPSController>();

        GetLoad();
    }

    public void SetSliderMouseValue()
    {
        _controller.SetSensitivityMouse(_sensitivity.value);

        GetLoad();
    }

    public void SetSliderAimValue()
    {
        _controller.SetSensitivityAim(_sensitivityAim.value);

        GetLoad();
    }

    private void GetLoad()
    {
        _sensitivity.value = _controller._mouseSensitivity;
        _sensitivityText.text = _sensitivity.value.ToString("F2");

        _sensitivityAim.value = _controller._aimSensitivity;
        _sensitivityAimText.text = _sensitivityAim.value.ToString("F2");
    }
}