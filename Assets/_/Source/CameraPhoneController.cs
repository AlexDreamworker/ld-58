using System;
using System.Collections;
using UnityEngine;

public class CameraPhoneController : MonoBehaviour
{
    [Header("Object References")]
    public GameObject draggableObject;
    public GameObject cameraPhonePhoto;
    public GameObject cameraPhone;
    public Camera _cameraPhone;
    public Camera _cameraPhonePhoto;
    public float _cameraZoomSize;
    
    [Header("Layer Settings")]
    public LayerMask panoramaLayerMask;
    
    [Header("Raycast Settings")]
    public float rayDistance = 100f;
    
    // Приватные переменные
    private GameObject hitPanorama;
    private Vector2 draggablePosition;
    private RaycastHit2D hit;

    private Coroutine _routine;

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
        draggablePosition = draggableObject.transform.position;
        
        hit = Physics2D.Raycast(
            draggablePosition, 
            Vector2.zero,  
            rayDistance,   
            panoramaLayerMask 
        );
        
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
        
        cameraPhonePhoto.transform.position = new Vector3(draggablePosition.x, draggablePosition.y, cameraPhonePhoto.transform.position.z);
        
        cameraPhonePhoto.transform.SetParent(hitPanorama.transform);
        
        
    }

    [ContextMenu("Show Photo")]
    public void ShowPhoto()
    {
        cameraPhone.SetActive(false);
        cameraPhonePhoto.SetActive(true);

        _routine = StartCoroutine(Routine());
        
        Debug.Log($"Photo shown on {hitPanorama.name}");
    }

    [ContextMenu("Hide Photo")]
    public void HidePhoto()
    {
        //cameraPhonePhoto.transform.SetParent(null); ??? Fixed bug
        cameraPhonePhoto.SetActive(false);
        cameraPhone.SetActive(true);
        
        Debug.Log("Photo hidden");
    }

    public void CamerasState(bool state)
    {
        cameraPhone.SetActive(state);
        cameraPhonePhoto.SetActive(false); //??? Fixed bug
    }

    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(0.01f);
        cameraPhonePhoto.SetActive(false);
    }

    public void ZoomPlus()
    {
        _cameraPhone.orthographicSize = _cameraZoomSize;
        _cameraPhonePhoto.orthographicSize = _cameraZoomSize;
    }

    public void ZoomMinus()
    {
        _cameraPhone.orthographicSize = 5f;
        _cameraPhonePhoto.orthographicSize = 5f;
    }
}