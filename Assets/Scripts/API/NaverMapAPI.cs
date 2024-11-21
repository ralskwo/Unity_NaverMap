using UnityEngine;

public class NaverMapAPI : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ���� ���� ������Ƽ
    public static NaverMapAPI Instance { get; private set; }

    // ���̹� ���� API�� �����ڵ� ��û URL    
    [HideInInspector] public string geocodeApiUrl = "https://naveropenapi.apigw.ntruss.com/map-geocode/v2/geocode";
    // ���̹� ���� API�� ���� ���� ��û URL
    [HideInInspector] public string mapStaticApiUrl = "https://naveropenapi.apigw.ntruss.com/map-static/v2/raster";
    // ���̹� Ŭ���� �÷������� �߱޹��� Ŭ���̾�Ʈ ���̵�
    [HideInInspector] public string clientID = "YOUR_CLIENT_ID";
    // ���̹� Ŭ���� �÷������� �߱޹��� Ŭ���̾�Ʈ ��ũ��
    [HideInInspector] public string clientSecret = "YOUR_CLIENT_SECRET";

    // Awake �޼���� ����Ƽ ������ ����Ŭ �� ��ü�� ó�� ������ �� ȣ���
    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            // ���� �ν��Ͻ��� �����ϰ� �ٸ� �� �ε� �� �ı����� �ʵ��� ����
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü�� �ı�
            Destroy(gameObject);
        }
    }
}
