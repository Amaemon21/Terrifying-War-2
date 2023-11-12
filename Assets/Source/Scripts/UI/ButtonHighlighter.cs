
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Класс для "подсвечивания" объекта-кнопки
[RequireComponent(typeof(Image))]
public class ButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite highlightSprite = null;

    private Image m_Image = null;
    private Sprite m_MySprite = null;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_MySprite = m_Image.sprite;
    }

    private void OnDisable()
    {
        //Меняем спрайт на оригинальный при отключении объекта
        m_Image.sprite = m_MySprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Меняем спрайт на подсвеченный когда курсор вошёл в область объекта
        m_Image.sprite = highlightSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Меняем спрайт на оригинальный когда курсор вошёл в область объекта
        m_Image.sprite = m_MySprite;
    }
}
