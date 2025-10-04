using System;
using System.Collections;
using UnityEngine;

public class CameraPhoneController : MonoBehaviour
{
    [Header("Object References")]
    public GameObject draggableObject; // Используем Transform для 2D
    public GameObject cameraPhonePhoto;
    public GameObject cameraPhone;
    
    [Header("Layer Settings")]
    public LayerMask panoramaLayerMask; // Маска слоя для панорам
    
    [Header("Raycast Settings")]
    public float rayDistance = 100f; // Дальность луча
    
    // Приватные переменные
    private GameObject hitPanorama;
    private Vector2 draggablePosition;
    private RaycastHit2D hit; // Используем RaycastHit2D для 2D

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            TakePhoto();
            
        if (Input.GetKeyDown(KeyCode.X))
            ShowPhoto();
            
        if (Input.GetKeyDown(KeyCode.C))
            HidePhoto();
    }

    [ContextMenu("Take Photo")]
    public void TakePhoto()
    {
        // Сохраняем позицию Draggable
        draggablePosition = draggableObject.transform.position;
        
        // Создаем луч ИСПОЛЬЗУЯ Physics2D.Raycast
        hit = Physics2D.Raycast(
            draggablePosition,    // Точка начала луча
            Vector2.zero,         // Направление (можно изменить на нужное)
            rayDistance,          // Дальность
            panoramaLayerMask     // Маска слоя
        );
        
        // Визуализация луча в Scene для отладки
        Debug.DrawRay(draggablePosition, Vector2.zero * rayDistance, Color.red, 2f);
        
        if (hit.collider != null)
        {
            hitPanorama = hit.collider.gameObject;
            Debug.Log($"Photo taken! Hit: {hitPanorama.name}");
        }
        else
        {
            Debug.Log("TakePhoto: Ray didn't hit any panorama objects.");
            // Для отладки: покажи луч, даже если нет попадания
            Debug.DrawRay(draggablePosition, Vector2.zero * rayDistance, Color.blue, 2f);
        }
        
        if (hitPanorama == null)
        {
            Debug.LogWarning("ShowPhoto: No panorama object saved. Take a photo first.");
            return;
        }

        // Перемещаем фото в позицию Draggable на момент съемки
        cameraPhonePhoto.transform.position = new Vector3(draggablePosition.x, draggablePosition.y, cameraPhonePhoto.transform.position.z);
        
        // Делаем фото дочерним объектом целевой панорамы
        cameraPhonePhoto.transform.SetParent(hitPanorama.transform);
        
        
    }

    [ContextMenu("Show Photo")]
    public void ShowPhoto()
    {
        // Переключаем видимость
        cameraPhone.SetActive(false);
        cameraPhonePhoto.SetActive(true);
        
        Debug.Log($"Photo shown on {hitPanorama.name}");
    }

    private IEnumerator CameraRoutine()
    {
        yield return null;
    }

    [ContextMenu("Hide Photo")]
    public void HidePhoto()
    {
        cameraPhonePhoto.transform.SetParent(null);
        cameraPhonePhoto.SetActive(false);
        cameraPhone.SetActive(true);
        
        Debug.Log("Photo hidden");
    }
}