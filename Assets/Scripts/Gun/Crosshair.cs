using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void Awake()
    {
        // ���� ���� �� Ŀ���� ����ϴ�.
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        // ���콺 ��ġ�� ��ũ�� ��ǥ�� �����ɴϴ�.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 2D ���ӿ��� �ʿ� ���� z�� ���� 0���� ����

        // �������� ��ġ�� ���콺 ��ġ�� �����մϴ�.
        transform.position = mousePosition;
    }
}
