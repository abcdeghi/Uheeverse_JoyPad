using UnityEngine;

public class JoypadInputManager : MonoBehaviour
{
    public PadState state = new PadState();

    [Header("Joystick")]
    public VariableJoystick joystick;    // 왼쪽 조이스틱 (이미 씬에 있죠!)

    [Header("Buttons")]
    public PadUiButton[] buttons;        // ABXY, L/R, PLUS/MINUS 전부

    [Header("Page")]
    public int currentPage = 0;          // 0: 기본 / 필요하면 나중에 1,2,...로 변경

    void Awake()
    {
        foreach (var b in buttons)
        {
            b.onChanged += OnButtonChanged;
        }
    }

    void Update()
    {
        // 조이스틱 값 업데이트
        state.lx = joystick.Horizontal;
        state.ly = joystick.Vertical;

        // 현재 페이지 번호
        state.page = currentPage;
    }

    void OnButtonChanged(string key, bool down)
    {
        switch (key)
        {
            case "A":      state.a = down; break;
            case "B":      state.b = down; break;
            case "X":      state.x = down; break;
            case "Y":      state.y = down; break;
            case "L":      state.l = down; break;
            case "R":      state.r = down; break;
            case "PLUS":   state.plus = down; break;
            case "MINUS":  state.minus = down; break;
        }
    }
}
