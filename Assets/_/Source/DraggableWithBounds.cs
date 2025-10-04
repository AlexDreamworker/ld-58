using UnityEngine;

public class DraggableWithBounds : MonoBehaviour
{
    private Vector3 dragOffset;
    private bool isDragging = false;
    
    private float minX, maxX, minY, maxY;
    
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        CalculateBounds();
    }

    void CalculateBounds()
    {
        if (spriteRenderer == null || mainCamera == null) return;
        
        float spriteWidth = spriteRenderer.bounds.size.x;
        float spriteHeight = spriteRenderer.bounds.size.y;
        
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        minX = mainCamera.transform.position.x - cameraWidth / 2 + spriteWidth / 2;
        maxX = mainCamera.transform.position.x + cameraWidth / 2 - spriteWidth / 2;
        minY = mainCamera.transform.position.y - cameraHeight / 2 + spriteHeight / 2;
        maxY = mainCamera.transform.position.y + cameraHeight / 2 - spriteHeight / 2;
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            
            if (spriteRenderer.bounds.Contains(mouseWorldPos))
            {
                isDragging = true;
                dragOffset = transform.position - mouseWorldPos;
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset;
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            
            transform.position = targetPosition;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
    
    void OnRectTransformDimensionsChange()
    {
        CalculateBounds();
    }
}
