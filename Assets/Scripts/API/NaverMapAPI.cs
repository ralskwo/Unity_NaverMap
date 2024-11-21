using UnityEngine;

public class NaverMapAPI : MonoBehaviour
{
    // 싱글톤 인스턴스를 위한 정적 프로퍼티
    public static NaverMapAPI Instance { get; private set; }

    // 네이버 지도 API의 지오코드 요청 URL    
    [HideInInspector] public string geocodeApiUrl = "https://naveropenapi.apigw.ntruss.com/map-geocode/v2/geocode";
    // 네이버 지도 API의 정적 지도 요청 URL
    [HideInInspector] public string mapStaticApiUrl = "https://naveropenapi.apigw.ntruss.com/map-static/v2/raster";
    // 네이버 클라우드 플랫폼에서 발급받은 클라이언트 아이디
    [HideInInspector] public string clientID = "YOUR_CLIENT_ID";
    // 네이버 클라우드 플랫폼에서 발급받은 클라이언트 시크릿
    [HideInInspector] public string clientSecret = "YOUR_CLIENT_SECRET";

    // Awake 메서드는 유니티 라이프 사이클 중 객체가 처음 생성될 때 호출됨
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
}
