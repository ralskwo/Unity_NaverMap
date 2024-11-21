using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NaverMapController : MonoBehaviour, IKeyPressButtonHandler
{
    public static NaverMapController Instance { get; private set; }

    public RawImage mapImage;
    public TMP_InputField addressInput;
    public Button searchButton;
    public int zoom = 15;
    public float latitude = 37.5665f;
    public float longitude = 126.9780f;
    private Vector2 pointerDownPosition;
    private Vector2 lastDragPosition;
    private Vector2 dragDeltaTotal;
    private bool isDragging = false;
    private const float clickThreshold = 10f;
    public float dragFactor = 0.000001f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ZoomController.Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetupEventTrigger();
        StartCoroutine(GetMapTile(latitude, longitude, zoom));
        searchButton.onClick.AddListener(OnSearchButtonClick);
    }

    void Update()
    {
        ZoomController.Instance.HandleZoom();

        // Enter 키를 감지하여 검색 버튼을 누름
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnButtonPress();
        }
    }

    public void OnButtonPress()
    {
        searchButton.onClick.Invoke();
        // 포커스를 입력 필드로 돌려줌
        addressInput.ActivateInputField();
    }

    private void SetupEventTrigger()
    {
        EventTrigger trigger = mapImage.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;
        entryPointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(entryPointerDown);

        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;
        entryPointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(entryPointerUp);

        EventTrigger.Entry entryDrag = new EventTrigger.Entry();
        entryDrag.eventID = EventTriggerType.Drag;
        entryDrag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entryDrag);
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        pointerDownPosition = eventData.position;
        lastDragPosition = eventData.position;
        dragDeltaTotal = Vector2.zero;
        isDragging = false;
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging && Vector2.Distance(pointerDownPosition, eventData.position) < clickThreshold)
        {
            // 클릭 이벤트 처리 (필요시 추가)
        }
    }

    private void OnDrag(PointerEventData eventData)
    {
        Vector2 currentDragPosition = eventData.position;
        Vector2 dragDelta = currentDragPosition - lastDragPosition;
        lastDragPosition = currentDragPosition;
        dragDeltaTotal += dragDelta;
        isDragging = true;
        latitude -= dragDelta.y * dragFactor;
        longitude -= dragDelta.x * dragFactor;
        StartCoroutine(GetMapTile(latitude, longitude, zoom));
    }

    private void OnSearchButtonClick()
    {
        string address = addressInput.text;
        if (!string.IsNullOrEmpty(address))
        {
            StartCoroutine(GeocodeManager.Instance.GetGeocode(address, OnGeocodeReceived));
        }
    }

    private void OnGeocodeReceived(List<Address> addresses)
    {
        if (addresses != null && addresses.Count > 0)
        {
            if (float.TryParse(addresses[0].y, out float lat) && float.TryParse(addresses[0].x, out float lon))
            {
                latitude = lat;
                longitude = lon;
                StartCoroutine(GetMapTile(latitude, longitude, zoom));
            }
            else
            {
                Debug.LogError("Invalid coordinates received from the geocode API.");
            }
        }
        else
        {
            Debug.LogError("No coordinates found for the address.");
        }
    }

    public IEnumerator GetMapTile(float latitude, float longitude, int zoom)
    {
        string apiUrl = $"{NaverMapAPI.Instance.mapStaticApiUrl}?w=500&h=500&center={longitude},{latitude}&level={zoom}&pos:{longitude} {latitude}";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(apiUrl);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", NaverMapAPI.Instance.clientID);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", NaverMapAPI.Instance.clientSecret);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            mapImage.texture = texture;
            mapImage.SetNativeSize();
            mapImage.rectTransform.anchoredPosition = Vector2.zero;
            mapImage.transform.localScale = Vector3.one * 3;
        }
    }
}
