using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weapon; // ���� Transform
    [SerializeField] private Transform character; // ĳ���� Transform
    [SerializeField] private Transform playerHand; // ĳ������ �� ��ġ
    [SerializeField] private float radius = 1.5f; // ���Ⱑ �̵��� ���� ������
    [SerializeField] private Transform shootPoint; // �߻� ��ġ
    [SerializeField] private GameObject[] bulletPrefabs; // �Ѿ� ������ �迭
    [SerializeField] private float shootForce = 20f; // �߻� �ӵ�

    public GameObject aimImage; // ������ �̹���

    void Update()
    {
        // ���콺 ��ġ ���
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 2D ���ӿ��� �ʿ� ���� z�� ���� 0���� ����

        // �տ��� ���콺 ��ġ������ ���� ���� ���
        Vector3 direction = mousePosition - playerHand.position;
        direction.z = 0f; // z���� 0���� �����Ͽ� 2D ��鿡���� ���

        // ������ ȸ�� ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������ ��ġ�� ���� �������� ���� ���� �̵���Ŵ
        Vector3 weaponPosition = playerHand.position + direction.normalized * radius;
        weapon.position = weaponPosition;

        // ������ ȸ�� ����
        weapon.rotation = Quaternion.Euler(0f, 0f, angle);

        // ������ ��ġ ����
        if (aimImage != null)
        {
            aimImage.transform.position = mousePosition; // ���콺 ��ġ�� ������ �̵�
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // �Ѿ��� �߻��� ���� ��� (���콺 ��ġ ����)
        Vector3 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position).normalized;

        // ���� �Ѿ� ������ ����
        GameObject selectedBulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];

        // �Ѿ� �߻�
        GameObject bullet = Instantiate(selectedBulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDirection * shootForce; // �Ѿ˿� �߻� �ӵ� ����
        }
    }
}
