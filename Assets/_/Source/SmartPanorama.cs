using UnityEngine;
using DG.Tweening;

public class SmartPanorama : MonoBehaviour
{
    [Header("References")]
    public DraggableWithBounds draggableSprite;
    
    [Header("Settings")]
    public float activationBorder = 0.1f;
    public float panoramaSpeed = 2f;

    private bool isPanoramaActive = false;
    private int panoramaDirection = 0;
    
    private float spriteWidth;
    public int childCount;
    // private Camera mainCamera;
    public Camera mainCamera;

    public bool _canScoll = true;

    void Start()
    {
        // mainCamera = Camera.main;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
        childCount = transform.childCount;
    }

    void Update()
    {
        CheckDragConditions();
        
        if (isPanoramaActive)
        {
            ScrollPanorama();
            CheckForLoop();
        }
    }
    public void MoveToX(float duration)
    {
        transform.DOMoveX(200f, duration);
    }

    void CheckDragConditions()
    {
        if (!Input.GetMouseButton(0) || draggableSprite == null)
        {
            isPanoramaActive = false;
            panoramaDirection = 0;
            return;
        }
        
        Vector3 mousePos = GetMouseWorldPosition();
        
        float screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        
        float screenWidth = screenRight - screenLeft;
        float leftActivationBorder = screenLeft + screenWidth * activationBorder;
        float rightActivationBorder = screenRight - screenWidth * activationBorder;

        if (_canScoll == false)
        {
            panoramaDirection = 0;
            return;
        }
            
        
        if (mousePos.x < leftActivationBorder)
        {
            isPanoramaActive = true;
            panoramaDirection = 1;
        }
        else if (mousePos.x > rightActivationBorder)
        {
            isPanoramaActive = true;
            panoramaDirection = -1;
        }
        else
        {
            isPanoramaActive = false;
            panoramaDirection = 0;
        }
    }

    public void DisableScrolling()
    {
        _canScoll = false;
    }

    void ScrollPanorama()
    {
        float movement = panoramaDirection * panoramaSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * movement);
    }

    void CheckForLoop()
    {
        float cameraX = mainCamera.transform.position.x;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float childPositionX = child.position.x;
            float distanceToCamera = childPositionX - cameraX;

            if (distanceToCamera < -spriteWidth)
            {
                child.Translate(Vector3.right * (childCount * spriteWidth));
            }
            else if (distanceToCamera > spriteWidth)
            {
                child.Translate(Vector3.left * (childCount * spriteWidth));
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}