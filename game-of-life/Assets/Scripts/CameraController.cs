using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public float zoomAmount = 18f; 
    public float minZoom = 30f;
    public float maxZoom = 540f;
    public float panSpeed = 50f;
    
    // Reference to your control panel GameObject
    public GameObject controlPanel;

    private GridManager gridManager;
    private Vector3 dragOrigin;
    private float minX, maxX, minY, maxY;
    private bool isDragging = false;

    private void Start()
    {
        cam = GetComponent<Camera>();
        gridManager = GridManager.Instance;
        cam.orthographic = true;
        CalculateCameraBounds();
    }

    private void Update()
    {
        if (gridManager != null)
        {
            MoveCamera();
            ClampCameraPosition();
        }
    }

    private bool IsPointerOverControlPanel()
    {
        // First check if pointer is over any UI element
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
            return true;
            
        // If you have a specific control panel that's not a UI element
        if (controlPanel != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is the control panel or a child of it
                if (hit.transform.gameObject == controlPanel || 
                    hit.transform.IsChildOf(controlPanel.transform))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    private void MoveCamera()
    {
        // Start drag only if not over control panel
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverControlPanel())
            {
                isDragging = true;
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                isDragging = false;
            }
        }

        // Continue drag only if we started a valid drag
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }

        // Reset dragging state
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Handle zoom with scroll wheel
        HandleZoom();
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput == 0) return;

        // Don't zoom if over control panel
        if (IsPointerOverControlPanel())
            return;

        // Store original zoom and mouse position
        float originalSize = cam.orthographicSize;
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = cam.nearClipPlane;
        Vector3 mouseWorldPosBefore = cam.ScreenToWorldPoint(mouseScreenPos);

        // Apply zoom
        float zoomChange = -scrollInput * 160f;
        cam.orthographicSize = Mathf.Clamp(originalSize + zoomChange, minZoom, maxZoom);

        // Get mouse position after zoom
        Vector3 mouseWorldPosAfter = cam.ScreenToWorldPoint(mouseScreenPos);

        // Adjust camera position to maintain mouse position
        cam.transform.position += mouseWorldPosBefore - mouseWorldPosAfter;

        // Update bounds and clamp
        CalculateCameraBounds();
        ClampCameraPosition();
    }

    private void CalculateCameraBounds()
    {
        float gridWidthTotal = gridManager.gridWidth * gridManager.cellSize - 15;
        float gridHeightTotal = gridManager.gridHeight * gridManager.cellSize - 15;

        float cameraHalfHeight = cam.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * cam.aspect;

        minX = cameraHalfWidth;
        maxX = gridWidthTotal - cameraHalfWidth;
        minY = cameraHalfHeight;
        maxY = gridHeightTotal - cameraHalfHeight;

        if (minX > maxX) minX = maxX = gridWidthTotal / 2;
        if (minY > maxY) minY = maxY = gridHeightTotal / 2;
    }

    private void ClampCameraPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }
}