using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera cam; // 메인 카메라 참조
    public Transform character; // 캐릭터 Transform (부모)

    void Update()
    {
        // 캐릭터의 방향 벡터를 계산합니다.
        Vector3 characterDir = character.right;

        // 마우스 위치를 월드 좌표로 변환합니다.
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // 총의 위치를 설정합니다.
        Vector3 gunPos = transform.position;

        // 총의 방향 벡터를 계산합니다.
        Vector3 direction = mousePos - gunPos;

        // 캐릭터의 방향과 총의 방향 사이의 회전 각도를 계산합니다.
        float angle = Vector3.SignedAngle(characterDir, direction, Vector3.forward);

        // 총의 회전 설정
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // 총을 캐릭터의 자식 객체로 설정
        transform.SetParent(character);
    }
}
