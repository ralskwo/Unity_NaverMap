using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class GeocodeManager : MonoBehaviour
{
    // 싱글톤 인스턴스를 위한 정적 프로퍼티
    public static GeocodeManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            // 현재 인스턴스를 설정하고 다른 씬 로드 시 파괴되지 않도록 설정
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 현재 객체를 파괴
            Destroy(gameObject);
        }
    }

    // 주소를 지오코딩하여 좌표 정보를 가져오는 코루틴
    public IEnumerator GetGeocode(string query, System.Action<List<Address>> callback)
    {
        // 지오코드 API URL 생성 (query 매개변수를 URL 인코딩)
        string url = $"{NaverMapAPI.Instance.geocodeApiUrl}?query={UnityWebRequest.EscapeURL(query)}";

        // UnityWebRequest 객체를 사용하여 GET 요청 생성
        UnityWebRequest request = UnityWebRequest.Get(url);
        // 요청 헤더에 클라이언트 ID와 시크릿 설정
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", NaverMapAPI.Instance.clientID);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", NaverMapAPI.Instance.clientSecret);

        // 요청 전송 및 응답 대기
        yield return request.SendWebRequest();

        // 네트워크 오류 또는 프로토콜 오류가 발생한 경우
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // 오류 메시지를 출력하고 콜백으로 null 전달
            Debug.LogError(request.error);
            callback(null);
        }
        else
        {
            // 응답이 성공적인 경우 응답 본문을 문자열로 가져옴
            string jsonResponse = request.downloadHandler.text;
            // 응답 문자열을 파싱하여 주소 리스트로 변환
            List<Address> addresses = ParseGeocodeResponse(jsonResponse);
            // 콜백 함수 호출하여 주소 리스트 전달
            callback(addresses);
        }
    }

    // JSON 응답 문자열을 Address 객체 리스트로 변환하는 메서드
    private List<Address> ParseGeocodeResponse(string jsonResponse)
    {
        // JSON 문자열을 GeocodeResponse 객체로 디시리얼라이즈
        GeocodeResponse geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponse>(jsonResponse);
        // GeocodeResponse 객체에서 주소 리스트를 반환
        return geocodeResponse.addresses;
    }
}

// Geocode API 응답 구조체
public class GeocodeResponse
{
    // JSON 응답에서 addresses 필드를 매핑
    public List<Address> addresses { get; set; }
}

// 주소 정보를 담는 클래스
public class Address
{
    // 도로명 주소
    public string roadAddress { get; set; }
    // 지번 주소
    public string jibunAddress { get; set; }
    // 영어 주소
    public string englishAddress { get; set; }
    // 경도
    public string x { get; set; }
    // 위도
    public string y { get; set; }
}
