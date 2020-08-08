using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ClickHandler : MonoBehaviour
{
    public UnityEvent DownEvent;
    public UnityEvent UpEvent;

    void OnMouseDown()
    {
        Debug.Log("Down");
        DownEvent?.Invoke();
    }

    void OnMouseUp()
    {
        Debug.Log("Up");
        UpEvent?.Invoke();
    }
}
