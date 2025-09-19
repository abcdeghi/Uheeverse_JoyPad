using System.Collections;
using UnityEngine;

public class JoyStickValue : MonoBehaviour
{
    [Header("조이스틱 & UDP 설정")]
    public VariableJoystick joystick;
    public UDPSender udpSender;
    
    [Header("전송 설정")]
    public float sendRate = 0.05f; // 초당 20회 전송 (50ms)
    public float inputThreshold = 0.1f; // 최소 입력값
    
    private float lastSendTime;
    private Vector2 lastSentInput;
    private bool isMoving = false;
    
    void Update()
    {
        // 일정 간격으로만 전송 (네트워크 부하 방지)
        if (Time.time - lastSendTime >= sendRate)
        {
            SendJoystickData();
            lastSendTime = Time.time;
        }
    }
    
    void SendJoystickData()
    {
        if (joystick == null || udpSender == null) return;
        
        
        Vector2 currentInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        
        
        bool hasInput = currentInput.magnitude > inputThreshold;
        
        
        if (ShouldSendData(currentInput, hasInput))
        {
            if (hasInput)
            {
               
                string moveCommand = $"MOVE,{currentInput.x:F3},{currentInput.y:F3}";
                udpSender.SendCommand(moveCommand);
                isMoving = true;
                
                Debug.Log($"조이스틱 전송: H={currentInput.x:F2}, V={currentInput.y:F2}");
            }
            else if (isMoving)
            {
                
                udpSender.SendCommand("STOP");
                isMoving = false;
                Debug.Log("조이스틱 정지");
            }
            
            lastSentInput = currentInput;
        }
    }
    
    bool ShouldSendData(Vector2 currentInput, bool hasInput)
    {
        // 움직임 상태가 바뀌었거나
        if (hasInput != isMoving) return true;
        
        // 입력값이 충분히 변했을 때
        if (hasInput && Vector2.Distance(currentInput, lastSentInput) > 0.05f) return true;
        
        return false;
    }
    
    // 디버그용 - 조이스틱 값 실시간 표시
    void OnGUI()
    {
        if (joystick == null) return;
        
        GUI.Label(new Rect(10, 10, 200, 20), $"Horizontal: {joystick.Horizontal:F2}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Vertical: {joystick.Vertical:F2}");
        GUI.Label(new Rect(10, 50, 200, 20), $"Direction: {joystick.Direction}");
        GUI.Label(new Rect(10, 70, 200, 20), $"IsMoving: {isMoving}");
    }
}