using UnityEngine; // UnityEngine 관련 클래스 및 함수를 사용하기 위한 네임스페이스
using UnityEngine.UI; // UI 관련 클래스 사용
using TMPro; // TextMeshPro 관련 클래스 사용
using Gpm.WebView; // GPM WebView 관련 클래스 사용

/// <summary>
/// Unity에서 사용자가 입력한 지역 정보를 바탕으로 지도에 마커를 추가하는 매니저 클래스
/// </summary>
public class NaverDynamicMapMarker : MonoBehaviour
{
    // 지역 이름을 입력받기 위한 TextMeshPro UI 필드
    public TMP_InputField locationInput;

    // 지역에 대한 설명을 입력받기 위한 TextMeshPro UI 필드
    public TMP_InputField descriptionInput;

    // 마커를 추가하는 버튼
    public Button addMarkerButton;

    // 마커를 모두 삭제하는 버튼
    public Button removeMarkersButton;

    /// <summary>
    /// 초기화 메서드로, Unity의 Start 이벤트에서 호출됨
    /// </summary>
    private void Start()
    {
        // 버튼 클릭 이벤트에 OnAddMarkerButtonClicked 메서드를 연결
        addMarkerButton.onClick.AddListener(OnAddMarkerButtonClicked);

        // 버튼 클릭 이벤트에 RemoveAllMarkers 메서드를 연결
        removeMarkersButton.onClick.AddListener(RemoveAllMarkers);
    }

    /// <summary>
    /// 마커 추가 버튼 클릭 시 호출되는 메서드
    /// </summary>
    private void OnAddMarkerButtonClicked()
    {
        // 사용자가 입력한 지역 이름을 가져옴
        string location = locationInput.text;

        // 사용자가 입력한 설명을 가져옴
        string description = descriptionInput.text;

        // 지역 이름이 비어 있는 경우 에러 로그 출력 후 종료
        if (string.IsNullOrEmpty(location))
        {
            Debug.LogError("지역 이름을 입력해주세요."); // 디버그 로그 출력
            return;
        }

        // Geocode API를 호출하여 지역 이름에 해당하는 좌표를 검색
        StartCoroutine(GeocodeManager.Instance.GetGeocode(location, (addresses) =>
        {
            // 검색 결과가 존재할 경우
            if (addresses != null && addresses.Count > 0)
            {
                // 첫 번째 검색 결과를 사용
                Address firstResult = addresses[0];

                // 좌표를 문자열에서 float로 변환
                float latitude = float.Parse(firstResult.y);
                float longitude = float.Parse(firstResult.x);

                // 설명이 비어 있을 경우 지역 이름을 설명으로 사용
                if (string.IsNullOrEmpty(description))
                {
                    description = location;
                }

                // 지도에 마커 추가
                AddMarkerToMap(latitude, longitude, location, description);
            }
            else
            {
                // 검색 결과가 없을 경우 에러 로그 출력
                Debug.LogError("해당 지역을 찾을 수 없습니다.");
            }
        }));
    }

    /// <summary>
    /// 지도에 마커를 추가하는 메서드
    /// </summary>
    /// <param name="latitude">마커의 위도</param>
    /// <param name="longitude">마커의 경도</param>
    /// <param name="title">마커의 제목</param>
    /// <param name="description">마커의 설명</param>
    private void AddMarkerToMap(float latitude, float longitude, string title, string description)
    {
        // JavaScript 함수 호출을 위한 메시지 생성
        string message = $"{{\"type\":\"addMarker\",\"latitude\":{latitude},\"longitude\":{longitude},\"title\":\"{title}\",\"description\":\"{description}\"}}";

        // WebView에서 JavaScript 함수 실행
        GpmWebView.ExecuteJavaScript($"receiveMessageFromUnity('{message}');");

        // 디버그 로그로 마커 추가 정보를 출력
        Debug.Log($"Marker added: {title} at ({latitude}, {longitude})");
    }

    /// <summary>
    /// 지도에서 모든 마커를 제거하는 요청을 JavaScript로 전달합니다.
    /// </summary>
    public void RemoveAllMarkers()
    {
        // GpmWebView.ExecuteJavaScript를 통해 Unity에서 WebView로 JavaScript 명령을 실행합니다.
        // "removeAllMarkers();" 명령어는 JavaScript 환경에서 해당 함수를 호출합니다.
        GpmWebView.ExecuteJavaScript("removeAllMarkers();");

        // Unity 디버그 콘솔에 로그 메시지를 출력하여 함수 호출 여부를 확인합니다.
        Debug.Log("Remove all markers request sent");
    }
}
