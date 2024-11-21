using Gpm.WebView; // GPM WebView 라이브러리를 사용하기 위한 네임스페이스
using UnityEngine; // UnityEngine 관련 클래스 및 함수 사용
using System.Collections.Generic; // List와 같은 컬렉션 클래스를 사용하기 위한 네임스페이스

/// <summary>
/// Unity에서 GPM WebView를 사용하여 네이버 지도를 표시하는 클래스
/// </summary>
public class NaverMapWebView : MonoBehaviour
{
    // 네이버 지도 HTML 파일이 호스팅된 URL
    private const string NaverMapURL = "https://your_domain.com/naver_map.html";

    // WebView가 열려 있는지 여부를 나타내는 플래그
    private bool isWebViewOpen = false;

    /// <summary>
    /// Unity 시작 시 WebView를 표시
    /// </summary>
    void Start()
    {
        ShowWebView(); // WebView를 표시하는 메서드 호출
    }

    /// <summary>
    /// WebView를 표시하는 메서드
    /// </summary>
    public void ShowWebView()
    {
        // 이미 WebView가 열려 있으면 아무 작업도 하지 않음
        if (isWebViewOpen)
        {
            Debug.Log("WebView already open."); // 디버그 로그 출력
            return;
        }

        // WebView의 설정을 정의
        var configuration = new GpmWebViewRequest.Configuration
        {
            style = GpmWebViewStyle.POPUP, // WebView 스타일을 팝업으로 설정
            isNavigationBarVisible = false, // 네비게이션 바 숨김
            isCloseButtonVisible = true, // 닫기 버튼 표시
            backgroundColor = "#FFFFFF", // 배경색 설정
            position = new GpmWebViewRequest.Position
            {
                hasValue = true, // 위치 값을 설정
                x = 0, // 화면의 x 좌표
                y = 0 // 화면의 y 좌표
            },
            size = new GpmWebViewRequest.Size
            {
                hasValue = true, // 크기 값을 설정
                width = Screen.width, // 화면 너비에 맞춤
                height = Screen.height / 2 // 화면 높이의 절반으로 설정
            }
        };

        // WebView에서 사용할 커스텀 URL 스킴 목록
        var schemeList = new List<string> { "customscheme" };

        // WebView를 지정된 URL로 열기
        GpmWebView.ShowUrl(NaverMapURL, configuration, OnCallback, schemeList);

        // WebView가 열려 있다고 플래그 설정
        isWebViewOpen = true;

        Debug.Log("WebView opened."); // 디버그 로그 출력
    }

    /// <summary>
    /// WebView에서 발생하는 콜백 처리
    /// </summary>
    private void OnCallback(GpmWebViewCallback.CallbackType callbackType, string data, GpmWebViewError error)
    {
        // 에러가 발생한 경우 처리
        if (error != null)
        {
            Debug.LogError($"WebView Error: {error.domain}, Code: {error.code}, Message: {error.message}");
            isWebViewOpen = false; // WebView 상태 플래그 초기화
            return;
        }

        // 스킴 이벤트 처리
        if (callbackType == GpmWebViewCallback.CallbackType.Scheme)
        {
            // 커스텀 스킴으로 시작하는 데이터인지 확인
            if (data.StartsWith("customscheme://"))
            {
                // 스킴 접두사를 제거하여 메시지 추출
                string message = data.Substring("customscheme://".Length);
                HandleMessageFromWebView(message); // WebView 메시지 처리 메서드 호출
            }
        }
    }

    /// <summary>
    /// WebView에서 받은 메시지를 처리하는 메서드
    /// </summary>
    /// <param name="message">WebView에서 전달된 메시지</param>
    private void HandleMessageFromWebView(string message)
    {
        Debug.Log($"Received message from WebView: {message}"); // 받은 메시지를 디버그 로그로 출력

        // 특정 마커 클릭 이벤트 처리
        if (message.StartsWith("markerClicked:"))
        {
            string markerName = message.Substring("markerClicked:".Length); // 마커 이름 추출
            Debug.Log($"Marker clicked: {markerName}"); // 마커 클릭 로그 출력
        }
        // 정보 창 열림 이벤트 처리
        else if (message == "infoWindowOpened")
        {
            Debug.Log("Info window opened."); // 정보 창 열림 로그 출력
        }
        // 정보 창 닫힘 이벤트 처리
        else if (message == "infoWindowClosed")
        {
            Debug.Log("Info window closed."); // 정보 창 닫힘 로그 출력
        }
        // 기타 지도 이벤트 처리
        else if (message.StartsWith("mapEvent:"))
        {
            Debug.Log($"Map Event: {message}"); // 지도 이벤트 로그 출력
        }
    }
}
