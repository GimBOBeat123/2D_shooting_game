using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera cam; // ���� ī�޶� ����
    public Transform character; // ĳ���� Transform (�θ�)

    void Update()
    {
        // ĳ������ ���� ���͸� ����մϴ�.
        Vector3 characterDir = character.right;

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ�մϴ�.
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // ���� ��ġ�� �����մϴ�.
        Vector3 gunPos = transform.position;

        // ���� ���� ���͸� ����մϴ�.
        Vector3 direction = mousePos - gunPos;

        // ĳ������ ����� ���� ���� ������ ȸ�� ������ ����մϴ�.
        float angle = Vector3.SignedAngle(characterDir, direction, Vector3.forward);

        // ���� ȸ�� ����
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // ���� ĳ������ �ڽ� ��ü�� ����
        transform.SetParent(character);
    }
}
