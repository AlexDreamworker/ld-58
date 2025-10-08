using UnityEngine;

public class DraggableWithBounds : MonoBehaviour
{
    private Vector3 dragOffset;
    private bool isDragging = false;
    
    private float minX, maxX, minY, maxY;
    
    private SpriteRenderer spriteRenderer;
    public Camera mainCamera;

    private bool _canDrag = true;
    
    [Header("Border Offset Settings")]
    public Vector2 horizontalOffset = Vector2.zero; // x = left, y = right
    public Vector2 verticalOffset = Vector2.zero;   // x = bottom, y = top

    [Space] public bool cursorModification = true;
    
    public bool IsDragging => isDragging;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CalculateBounds();
    }

    void CalculateBounds()
    {
        if (spriteRenderer == null || mainCamera == null) return;
        
        // Получаем размеры спрайта
        Bounds spriteBounds = spriteRenderer.bounds;
        float spriteHalfWidth = spriteBounds.extents.x;
        float spriteHalfHeight = spriteBounds.extents.y;
        
        // Получаем границы камеры в мировых координатах
        float cameraBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float cameraTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float cameraLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float cameraRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        
        // Рассчитываем границы с учетом отступов
        // Для горизонтали: левая граница = левый край экрана + половина ширины спрайта + левый отступ
        //                  правая граница = правый край экрана - половина ширины спрайта - правый отступ
        minX = cameraLeft + spriteHalfWidth + horizontalOffset.x;
        maxX = cameraRight - spriteHalfWidth - horizontalOffset.y;
        
        // Для вертикали: нижняя граница = нижний край экрана + половина высоты спрайта + нижний отступ
        //                верхняя граница = верхний край экрана - половина высоты спрайта - верхний отступ
        minY = cameraBottom + spriteHalfHeight + verticalOffset.x;
        maxY = cameraTop - spriteHalfHeight - verticalOffset.y;
        
        // Debug-логи для проверки границ
        Debug.Log($"Camera - Left: {cameraLeft}, Right: {cameraRight}, Bottom: {cameraBottom}, Top: {cameraTop}");
        Debug.Log($"Sprite - HalfWidth: {spriteHalfWidth}, HalfHeight: {spriteHalfHeight}");
        Debug.Log($"Bounds - minX: {minX}, maxX: {maxX}, minY: {minY}, maxY: {maxY}");
        Debug.Log($"Offsets - Horizontal: {horizontalOffset}, Vertical: {verticalOffset}");
    }

    void Update()
    {
        HandleMouseInput();
    }

    public void SetCanDrag(bool canDrag)
    {
        _canDrag = canDrag;
    }

    void HandleMouseInput()
    {
        if (!_canDrag)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            
            if (spriteRenderer.bounds.Contains(mouseWorldPos))
            {
                isDragging = true;
                dragOffset = transform.position - mouseWorldPos;

                if (cursorModification)
                {
                    Cursor.visible = false;
                }
                    
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            
            if (cursorModification)
            {
                Cursor.visible = true;
            }
        }
        
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset;
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            
            //transform.position = targetPosition;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    
    public Bounds GetSpriteBounds()
    {
        return spriteRenderer.bounds;
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
    
    // Добавляем метод для принудительного пересчета границ при изменении offset
    public void UpdateBounds()
    {
        CalculateBounds();
    }
    
    // Метод для визуализации границ в редакторе
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0), 
                           new Vector3(maxX - minX, maxY - minY, 0));
    }
}