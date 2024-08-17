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
    [SerializeField] private PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ����

    public GameObject aimImage; // ������ �̹���

    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip shootSound;

    void Update()
    {
        if (playerController == null || playerController.health <= 0)
        {
            return; // �÷��̾ ���ų� ü���� 0�̸� �� �߻縦 ����
        }

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

        // �Ѿ� �߻�
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (playerController == null || playerController.health <= 0)
        {
            return; // �÷��̾ ���ų� ü���� 0�̸� �� �߻縦 ����
        }

        // ���� �Ѿ� ������ ����
        GameObject selectedBulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];

        // �Ѿ� �߻�
        GameObject bullet = Instantiate(selectedBulletPrefab, shootPoint.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // �Ѿ� �߻� ���� �� �ӵ� ����
            Vector3 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position);
            shootDirection.z = 0f; // z�� ���� 0���� �����Ͽ� 2D ��鿡���� ���
            shootDirection.Normalize();

            bulletScript.Initialize(shootDirection, shootForce); // ����� �ӵ��� ����

            // �Ѿ��� ȸ�� ����
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
