using UnityEngine; // UnityEngine ���� Ŭ���� �� �Լ��� ����ϱ� ���� ���ӽ����̽�
using UnityEngine.UI; // UI ���� Ŭ���� ���
using TMPro; // TextMeshPro ���� Ŭ���� ���
using Gpm.WebView; // GPM WebView ���� Ŭ���� ���

/// <summary>
/// Unity���� ����ڰ� �Է��� ���� ������ �������� ������ ��Ŀ�� �߰��ϴ� �Ŵ��� Ŭ����
/// </summary>
public class NaverDynamicMapMarker : MonoBehaviour
{
    // ���� �̸��� �Է¹ޱ� ���� TextMeshPro UI �ʵ�
    public TMP_InputField locationInput;

    // ������ ���� ������ �Է¹ޱ� ���� TextMeshPro UI �ʵ�
    public TMP_InputField descriptionInput;

    // ��Ŀ�� �߰��ϴ� ��ư
    public Button addMarkerButton;

    // ��Ŀ�� ��� �����ϴ� ��ư
    public Button removeMarkersButton;

    /// <summary>
    /// �ʱ�ȭ �޼����, Unity�� Start �̺�Ʈ���� ȣ���
    /// </summary>
    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� OnAddMarkerButtonClicked �޼��带 ����
        addMarkerButton.onClick.AddListener(OnAddMarkerButtonClicked);

        // ��ư Ŭ�� �̺�Ʈ�� RemoveAllMarkers �޼��带 ����
        removeMarkersButton.onClick.AddListener(RemoveAllMarkers);
    }

    /// <summary>
    /// ��Ŀ �߰� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    /// </summary>
    private void OnAddMarkerButtonClicked()
    {
        // ����ڰ� �Է��� ���� �̸��� ������
        string location = locationInput.text;

        // ����ڰ� �Է��� ������ ������
        string description = descriptionInput.text;

        // ���� �̸��� ��� �ִ� ��� ���� �α� ��� �� ����
        if (string.IsNullOrEmpty(location))
        {
            Debug.LogError("���� �̸��� �Է����ּ���."); // ����� �α� ���
            return;
        }

        // Geocode API�� ȣ���Ͽ� ���� �̸��� �ش��ϴ� ��ǥ�� �˻�
        StartCoroutine(GeocodeManager.Instance.GetGeocode(location, (addresses) =>
        {
            // �˻� ����� ������ ���
            if (addresses != null && addresses.Count > 0)
            {
                // ù ��° �˻� ����� ���
                Address firstResult = addresses[0];

                // ��ǥ�� ���ڿ����� float�� ��ȯ
                float latitude = float.Parse(firstResult.y);
                float longitude = float.Parse(firstResult.x);

                // ������ ��� ���� ��� ���� �̸��� �������� ���
                if (string.IsNullOrEmpty(description))
                {
                    description = location;
                }

                // ������ ��Ŀ �߰�
                AddMarkerToMap(latitude, longitude, location, description);
            }
            else
            {
                // �˻� ����� ���� ��� ���� �α� ���
                Debug.LogError("�ش� ������ ã�� �� �����ϴ�.");
            }
        }));
    }

    /// <summary>
    /// ������ ��Ŀ�� �߰��ϴ� �޼���
    /// </summary>
    /// <param name="latitude">��Ŀ�� ����</param>
    /// <param name="longitude">��Ŀ�� �浵</param>
    /// <param name="title">��Ŀ�� ����</param>
    /// <param name="description">��Ŀ�� ����</param>
    private void AddMarkerToMap(float latitude, float longitude, string title, string description)
    {
        // JavaScript �Լ� ȣ���� ���� �޽��� ����
        string message = $"{{\"type\":\"addMarker\",\"latitude\":{latitude},\"longitude\":{longitude},\"title\":\"{title}\",\"description\":\"{description}\"}}";

        // WebView���� JavaScript �Լ� ����
        GpmWebView.ExecuteJavaScript($"receiveMessageFromUnity('{message}');");

        // ����� �α׷� ��Ŀ �߰� ������ ���
        Debug.Log($"Marker added: {title} at ({latitude}, {longitude})");
    }

    /// <summary>
    /// �������� ��� ��Ŀ�� �����ϴ� ��û�� JavaScript�� �����մϴ�.
    /// </summary>
    public void RemoveAllMarkers()
    {
        // GpmWebView.ExecuteJavaScript�� ���� Unity���� WebView�� JavaScript ����� �����մϴ�.
        // "removeAllMarkers();" ��ɾ�� JavaScript ȯ�濡�� �ش� �Լ��� ȣ���մϴ�.
        GpmWebView.ExecuteJavaScript("removeAllMarkers();");

        // Unity ����� �ֿܼ� �α� �޽����� ����Ͽ� �Լ� ȣ�� ���θ� Ȯ���մϴ�.
        Debug.Log("Remove all markers request sent");
    }
}
