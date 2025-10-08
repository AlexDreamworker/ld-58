using UnityEngine;
using DG.Tweening;

public class SmartPanorama : MonoBehaviour
{
    [Header("References")]
    public DraggableWithBounds draggableSprite;
    
    [Header("Settings")]
    public float activationBorder = 50f;
    public float panoramaSpeed = 2f;

    private bool isPanoramaActive = false;
    private int panoramaDirection = 0;
    
    private float spriteWidth;
    public int childCount;
    public Camera mainCamera;

    public bool _canScoll = true;

    void Start()
    {
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

    void CheckDragConditions()
    {
        if (!Input.GetMouseButton(0) || draggableSprite == null)
        {
            isPanoramaActive = false;
            panoramaDirection = 0;
            return;
        }

        if (draggableSprite.IsDragging == false)
            return;
        
        Vector3 spritePos = draggableSprite.transform.position;
        
        // Получаем границы экрана в мировых координатах
        float screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        
        // Получаем границы спрайта
        Bounds spriteBounds = draggableSprite.GetSpriteBounds();
        float spriteLeft = spritePos.x - spriteBounds.extents.x;
        float spriteRight = spritePos.x + spriteBounds.extents.x;

        if (_canScoll == false)
        {
            panoramaDirection = 0;
            return;
        }
            
        // Проверяем, достиг ли спрайт границ экрана
        if (spriteLeft <= screenLeft)
        {
            isPanoramaActive = true;
            panoramaDirection = 1; // Двигаем панораму вправо
        }
        else if (spriteRight >= screenRight)
        {
            isPanoramaActive = true;
            panoramaDirection = -1; // Двигаем панораму влево
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
}