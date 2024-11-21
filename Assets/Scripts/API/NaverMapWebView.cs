using Gpm.WebView; // GPM WebView ���̺귯���� ����ϱ� ���� ���ӽ����̽�
using UnityEngine; // UnityEngine ���� Ŭ���� �� �Լ� ���
using System.Collections.Generic; // List�� ���� �÷��� Ŭ������ ����ϱ� ���� ���ӽ����̽�

/// <summary>
/// Unity���� GPM WebView�� ����Ͽ� ���̹� ������ ǥ���ϴ� Ŭ����
/// </summary>
public class NaverMapWebView : MonoBehaviour
{
    // ���̹� ���� HTML ������ ȣ���õ� URL
    private const string NaverMapURL = "https://your_domain.com/naver_map.html";

    // WebView�� ���� �ִ��� ���θ� ��Ÿ���� �÷���
    private bool isWebViewOpen = false;

    /// <summary>
    /// Unity ���� �� WebView�� ǥ��
    /// </summary>
    void Start()
    {
        ShowWebView(); // WebView�� ǥ���ϴ� �޼��� ȣ��
    }

    /// <summary>
    /// WebView�� ǥ���ϴ� �޼���
    /// </summary>
    public void ShowWebView()
    {
        // �̹� WebView�� ���� ������ �ƹ� �۾��� ���� ����
        if (isWebViewOpen)
        {
            Debug.Log("WebView already open."); // ����� �α� ���
            return;
        }

        // WebView�� ������ ����
        var configuration = new GpmWebViewRequest.Configuration
        {
            style = GpmWebViewStyle.POPUP, // WebView ��Ÿ���� �˾����� ����
            isNavigationBarVisible = false, // �׺���̼� �� ����
            isCloseButtonVisible = true, // �ݱ� ��ư ǥ��
            backgroundColor = "#FFFFFF", // ���� ����
            position = new GpmWebViewRequest.Position
            {
                hasValue = true, // ��ġ ���� ����
                x = 0, // ȭ���� x ��ǥ
                y = 0 // ȭ���� y ��ǥ
            },
            size = new GpmWebViewRequest.Size
            {
                hasValue = true, // ũ�� ���� ����
                width = Screen.width, // ȭ�� �ʺ� ����
                height = Screen.height / 2 // ȭ�� ������ �������� ����
            }
        };

        // WebView���� ����� Ŀ���� URL ��Ŵ ���
        var schemeList = new List<string> { "customscheme" };

        // WebView�� ������ URL�� ����
        GpmWebView.ShowUrl(NaverMapURL, configuration, OnCallback, schemeList);

        // WebView�� ���� �ִٰ� �÷��� ����
        isWebViewOpen = true;

        Debug.Log("WebView opened."); // ����� �α� ���
    }

    /// <summary>
    /// WebView���� �߻��ϴ� �ݹ� ó��
    /// </summary>
    private void OnCallback(GpmWebViewCallback.CallbackType callbackType, string data, GpmWebViewError error)
    {
        // ������ �߻��� ��� ó��
        if (error != null)
        {
            Debug.LogError($"WebView Error: {error.domain}, Code: {error.code}, Message: {error.message}");
            isWebViewOpen = false; // WebView ���� �÷��� �ʱ�ȭ
            return;
        }

        // ��Ŵ �̺�Ʈ ó��
        if (callbackType == GpmWebViewCallback.CallbackType.Scheme)
        {
            // Ŀ���� ��Ŵ���� �����ϴ� ���������� Ȯ��
            if (data.StartsWith("customscheme://"))
            {
                // ��Ŵ ���λ縦 �����Ͽ� �޽��� ����
                string message = data.Substring("customscheme://".Length);
                HandleMessageFromWebView(message); // WebView �޽��� ó�� �޼��� ȣ��
            }
        }
    }

    /// <summary>
    /// WebView���� ���� �޽����� ó���ϴ� �޼���
    /// </summary>
    /// <param name="message">WebView���� ���޵� �޽���</param>
    private void HandleMessageFromWebView(string message)
    {
        Debug.Log($"Received message from WebView: {message}"); // ���� �޽����� ����� �α׷� ���

        // Ư�� ��Ŀ Ŭ�� �̺�Ʈ ó��
        if (message.StartsWith("markerClicked:"))
        {
            string markerName = message.Substring("markerClicked:".Length); // ��Ŀ �̸� ����
            Debug.Log($"Marker clicked: {markerName}"); // ��Ŀ Ŭ�� �α� ���
        }
        // ���� â ���� �̺�Ʈ ó��
        else if (message == "infoWindowOpened")
        {
            Debug.Log("Info window opened."); // ���� â ���� �α� ���
        }
        // ���� â ���� �̺�Ʈ ó��
        else if (message == "infoWindowClosed")
        {
            Debug.Log("Info window closed."); // ���� â ���� �α� ���
        }
        // ��Ÿ ���� �̺�Ʈ ó��
        else if (message.StartsWith("mapEvent:"))
        {
            Debug.Log($"Map Event: {message}"); // ���� �̺�Ʈ �α� ���
        }
    }
}
