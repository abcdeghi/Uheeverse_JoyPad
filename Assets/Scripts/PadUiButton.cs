using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;   // ← 추가

public class PadUiButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string key;                    // "A","B","X","Y","L","R","PLUS","MINUS"
    public System.Action<string,bool> onChanged;

    [Header("비주얼 설정")]
    public Image targetImage;             // 색/스프라이트를 바꿀 이미지
    public Color normalColor = Color.white;
    public Color pressedColor = new Color(0.9f, 0.9f, 0.9f, 1f); // 살짝 어둡게

    public Sprite normalSprite;           // 기본 스프라이트 (선택)
    public Sprite pressedSprite;          // 눌렸을 때 스프라이트 (선택)

    void Awake()
    {
        // targetImage를 안 넣어놨다면, 자기 자신의 Image 사용
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        // 시작할 때 기본 상태 셋업
        if (targetImage != null)
        {
            targetImage.color = normalColor;
            if (normalSprite != null)
                targetImage.sprite = normalSprite;
        }
    }

    public void OnPointerDown(PointerEventData e)
    {
        onChanged?.Invoke(key, true);

        // 눌림 비주얼
        if (targetImage != null)
        {
            targetImage.color = pressedColor;
            if (pressedSprite != null)
                targetImage.sprite = pressedSprite;
        }
    }

    public void OnPointerUp(PointerEventData e)
    {
        onChanged?.Invoke(key, false);

        // 원래 상태로 복귀
        if (targetImage != null)
        {
            targetImage.color = normalColor;
            if (normalSprite != null)
                targetImage.sprite = normalSprite;
        }
    }
}
