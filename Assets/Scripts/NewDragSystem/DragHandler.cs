using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  {
    public static GameObject draggableObject;
    private bool overTarget = false;
    private Collider2D targetObject;

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggableObject = gameObject;
        draggableObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggableObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }

    public void OnMouseDrag()
    {
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggableObject = null;
        if (overTarget)
        {
            overTarget = false;
            Word targetWord = targetObject.GetComponent<Word>();
            GetComponent<Word>().WordUpdate(targetWord);
        }
    }

    protected virtual void OnMouseUp()
    {
        if (overTarget)
        {
            overTarget = false;
            GetComponent<Word>().WordUpdate(targetObject.GetComponent<Word>());
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "target")
        {
            overTarget = true;
            targetObject = other;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "target")
        {
            overTarget = false;
            targetObject = null;
        }
    }
}
