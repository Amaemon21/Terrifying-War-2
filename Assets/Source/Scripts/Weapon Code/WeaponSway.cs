using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float _swayAmount = 4.0f;
    [SerializeField] private float _smoothTime = 10.0f;

    private void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * _swayAmount;
        var mouseY = Input.GetAxisRaw("Mouse Y") * _swayAmount;

        var rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        var rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        var targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smoothTime * Time.deltaTime);
    }
}
