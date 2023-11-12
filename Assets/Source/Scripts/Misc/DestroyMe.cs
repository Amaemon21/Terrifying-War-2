using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public bool destroyOnlyParentObject;

    public float timeToDestroy = 2.0f;

    public void Destroy()
    {
        //Проверяем, если булевое поле destroyOnlyParentObject равняется true
        if (destroyOnlyParentObject)
            Destroy(gameObject.transform.parent.gameObject, timeToDestroy);
        else //Иначе
            Destroy(gameObject, timeToDestroy);
    }
}
