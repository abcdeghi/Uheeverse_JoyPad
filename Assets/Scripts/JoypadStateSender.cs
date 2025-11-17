using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class JoypadStateSender : MonoBehaviour   // ← 여기 이름 변경!
{
    public JoypadInputManager input;
    public string targetIp = "127.0.0.1";
    public int targetPort = 9000;
    public float sendInterval = 0.03f;

    private UdpClient client;
    private float timer;

    void Start()
    {
        client = new UdpClient();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= sendInterval)
        {
            timer = 0f;
            SendState();
        }
    }

    void SendState()
    {
        if (input == null || input.state == null) return;

        string json = JsonUtility.ToJson(input.state);
        byte[] data = Encoding.UTF8.GetBytes(json);

        try
        {
            client.Send(data, data.Length, targetIp, targetPort);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[JoypadStateSender] " + e.Message);
        }
    }

    void OnDestroy()
    {
        if (client != null)
        {
            client.Close();
            client = null;
        }
    }
}
