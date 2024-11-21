using UnityEngine;

public class ZoomController
{
    public static ZoomController Instance { get; private set; }
    private float previousDistance;
    private bool isZooming = false;
    private const float zoomSpeed = 0.1f;

    private ZoomController() { }

    public static void Initialize()
    {
        if (Instance == null)
        {
            Instance = new ZoomController();
        }
    }

    public void HandleZoom()
    {
#if UNITY_EDITOR
        HandleMouseZoom();
#endif

#if UNITY_ANDROID
        HandleTouchZoom();
#endif
    }

    private void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            NaverMapController.Instance.zoom = Mathf.Clamp(NaverMapController.Instance.zoom + (scroll > 0 ? 1 : -1), 1, 20);
            NaverMapController.Instance.StartCoroutine(NaverMapController.Instance.GetMapTile(NaverMapController.Instance.latitude, NaverMapController.Instance.longitude, NaverMapController.Instance.zoom));
        }
    }

    private void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                previousDistance = Vector2.Distance(touch1.position, touch2.position);
                isZooming = true;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                if (Mathf.Abs(currentDistance - previousDistance) > 0.01f)
                {
                    NaverMapController.Instance.zoom = Mathf.Clamp(NaverMapController.Instance.zoom + (currentDistance > previousDistance ? 1 : -1), 1, 20);
                    NaverMapController.Instance.StartCoroutine(NaverMapController.Instance.GetMapTile(NaverMapController.Instance.latitude, NaverMapController.Instance.longitude, NaverMapController.Instance.zoom));
                    previousDistance = currentDistance;
                }
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                isZooming = false;
            }
        }
    }
}
