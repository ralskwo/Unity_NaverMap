using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class GeocodeManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ���� ���� ������Ƽ
    public static GeocodeManager Instance { get; private set; }

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

    // �ּҸ� �����ڵ��Ͽ� ��ǥ ������ �������� �ڷ�ƾ
    public IEnumerator GetGeocode(string query, System.Action<List<Address>> callback)
    {
        // �����ڵ� API URL ���� (query �Ű������� URL ���ڵ�)
        string url = $"{NaverMapAPI.Instance.geocodeApiUrl}?query={UnityWebRequest.EscapeURL(query)}";

        // UnityWebRequest ��ü�� ����Ͽ� GET ��û ����
        UnityWebRequest request = UnityWebRequest.Get(url);
        // ��û ����� Ŭ���̾�Ʈ ID�� ��ũ�� ����
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", NaverMapAPI.Instance.clientID);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", NaverMapAPI.Instance.clientSecret);

        // ��û ���� �� ���� ���
        yield return request.SendWebRequest();

        // ��Ʈ��ũ ���� �Ǵ� �������� ������ �߻��� ���
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // ���� �޽����� ����ϰ� �ݹ����� null ����
            Debug.LogError(request.error);
            callback(null);
        }
        else
        {
            // ������ �������� ��� ���� ������ ���ڿ��� ������
            string jsonResponse = request.downloadHandler.text;
            // ���� ���ڿ��� �Ľ��Ͽ� �ּ� ����Ʈ�� ��ȯ
            List<Address> addresses = ParseGeocodeResponse(jsonResponse);
            // �ݹ� �Լ� ȣ���Ͽ� �ּ� ����Ʈ ����
            callback(addresses);
        }
    }

    // JSON ���� ���ڿ��� Address ��ü ����Ʈ�� ��ȯ�ϴ� �޼���
    private List<Address> ParseGeocodeResponse(string jsonResponse)
    {
        // JSON ���ڿ��� GeocodeResponse ��ü�� ��ø��������
        GeocodeResponse geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponse>(jsonResponse);
        // GeocodeResponse ��ü���� �ּ� ����Ʈ�� ��ȯ
        return geocodeResponse.addresses;
    }
}

// Geocode API ���� ����ü
public class GeocodeResponse
{
    // JSON ���信�� addresses �ʵ带 ����
    public List<Address> addresses { get; set; }
}

// �ּ� ������ ��� Ŭ����
public class Address
{
    // ���θ� �ּ�
    public string roadAddress { get; set; }
    // ���� �ּ�
    public string jibunAddress { get; set; }
    // ���� �ּ�
    public string englishAddress { get; set; }
    // �浵
    public string x { get; set; }
    // ����
    public string y { get; set; }
}
