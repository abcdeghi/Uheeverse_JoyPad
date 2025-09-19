using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UDPSender : MonoBehaviour
{

    public string ServerIP;
    public string PORT;

    public int port;

    [Header("디버그")]
    public bool showDebugLogs = true;
    
    private UdpClient udpClient;
    
    void Start()
    {
        // UDP 클라이언트 초기화
        try
        {
            EnvLoader.Load();

            ServerIP = EnvLoader.Get("SERVER_IP", ServerIP);
            PORT = EnvLoader.Get("PORT", PORT.ToString());
            port = int.Parse(PORT);

            udpClient = new UdpClient();
            Debug.Log($"UDP 전송 준비 완료: {ServerIP}:{PORT}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UDP 초기화 실패: {e.Message}");
        }
    }
    
    public void SendCommand(string command)
    {
        if (udpClient == null)
        {
            Debug.LogWarning("UDP 클라이언트가 초기화되지 않았습니다.");
            return;
        }
        
        try
        {
            // 문자열을 바이트 배열로 변환
            byte[] data = Encoding.UTF8.GetBytes(command);
            
            // UDP로 전송
            udpClient.Send(data, data.Length, ServerIP, port);
            
            if (showDebugLogs)
            {
                Debug.Log($"[UDP 전송] {command} → {ServerIP}:{port}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UDP 전송 실패: {e.Message}");
        }
    }
    
    // IP 주소 설정 (런타임에 변경 가능)
    public void SetServerIP(string newIP)
    {
        ServerIP = newIP;
        Debug.Log($"서버 IP 변경: {ServerIP}");
    }
    
    // 포트 설정
    public void SetPort(int newPort)
    {
        port = newPort;
        Debug.Log($"포트 변경: {port}");
    }
    
    // 연결 테스트
    public void TestConnection()
    {
        SendCommand("TEST_CONNECTION");
    }
    
    void OnDestroy()
    {
        // UDP 클라이언트 정리
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient.Dispose();
        }
    }
    
    void OnApplicationQuit()
    {
        OnDestroy();
    }
}