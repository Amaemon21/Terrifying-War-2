using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Crosshair : MonoBehaviour
{
    public float idleSize = 50.0f;
    public float maxSize = 100.0f;
    public float speed = 5.0f;
    public float walkingSize = 150.0f;
    public float runningSize = 250.0f;

    public bool IsWalking { get; set; } = false;
    public bool IsRunning { get; set; } = false;

    private float m_CurrentSize = 0.0f;
    private RectTransform m_Crosshair = null;

    private void Start() 
    { 
        m_Crosshair = GetComponent<RectTransform>();
    }

    private void Update()
    {
        CheckButtonsState();
        CheckMoveToResize(IsWalking);

        m_Crosshair.sizeDelta = new Vector2(m_CurrentSize, m_CurrentSize);
    }
    //Отслеживаем передвижение и задаём плавное изменение размера
    private void CheckMoveToResize(bool isMoving)
    {
        if (isMoving)
        {
            m_CurrentSize = Mathf.Lerp(m_CurrentSize, maxSize, Time.deltaTime * speed);
        }
        else
        {
            m_CurrentSize = Mathf.Lerp(m_CurrentSize, idleSize, Time.deltaTime * speed);
        }

        m_CurrentSize = Mathf.Lerp(m_CurrentSize, idleSize, Time.deltaTime * speed);
    }
    //Проверяем, нажаты ли кнопки W, A, S, D - и задаём размеры
    private void CheckButtonsState()
    {
        IsWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (IsWalking)
        {
            IsRunning = Input.GetKey(ButtonsManager.Instance.RunningButton);

            maxSize = walkingSize;

            if (IsRunning)
            {
                maxSize = runningSize;
            }
            else
            {
                maxSize = walkingSize;
            }
        }
        else
        {
            maxSize = walkingSize;
        }
    }
}
