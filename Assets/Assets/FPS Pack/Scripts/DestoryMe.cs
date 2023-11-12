using UnityEngine;

public class DestoryMe : MonoBehaviour
{
    [SerializeField] private float time = 3f;

    void Start()
    {
        Destroy(gameObject, time);
    }
}
