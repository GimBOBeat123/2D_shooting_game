using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void Awake()
    {
        // 게임 시작 시 커서를 숨깁니다.
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        // 마우스 위치를 스크린 좌표로 가져옵니다.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 2D 게임에서 필요 없는 z축 값을 0으로 설정

        // 조준점의 위치를 마우스 위치로 설정합니다.
        transform.position = mousePosition;
    }
}
