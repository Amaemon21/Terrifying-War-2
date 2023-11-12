using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, IPointerClickHandler
{
    [SerializeField] private float normal = 1.0f;
    [SerializeField] private float scale = 1.1f;
    //Возвращаем локальный размер при отключении объекта
    public void OnDisable()
    {
        gameObject.transform.localScale = new Vector3(normal, normal, normal);
    }
    //Меняем локальный размер
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }
    //Возвращаем локальный размер
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(normal, normal, normal);
    }
}
