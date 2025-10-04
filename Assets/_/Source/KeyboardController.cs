using UnityEngine;
using UnityEngine.Events;

public class KeyboardController : MonoBehaviour
{
    public UnityEvent onKeyR;
    public UnityEvent onKeyW;
    public UnityEvent onKeyL;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            onKeyR?.Invoke();
            
        if (Input.GetKeyDown(KeyCode.W))
            onKeyW?.Invoke();
            
        if (Input.GetKeyDown(KeyCode.L))
            onKeyL?.Invoke();
    }
}
