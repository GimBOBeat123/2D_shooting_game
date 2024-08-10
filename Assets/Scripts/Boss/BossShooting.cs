using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public Transform target; // ĳ������ Transform
    public GameObject bulletPrefab; // �Ѿ� �������� ������ ����
    public int bulletCount = 20; // �߻��� �Ѿ��� ����
    public float bulletSpeed = 5f; // �Ѿ��� �ӵ�
    public float fireRate = 2f; // �Ѿ� �߻� �ֱ�
    public float rotationDuration = 2f; // ȸ�� ź�� ������ ���� �ð�
    public float rotationSpeed = 180f; // ������ ȸ�� �ӵ� (��/��)

    private bool isShooting = false;

    public IEnumerator Shoot360()
    {
        isShooting = true;

        float angleStep = 360f / bulletCount; // �� �Ѿ� ������ ����
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // ������ ���� �Ѿ��� ���� ���
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            // �Ѿ� ���� �� �߻�
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

            angle += angleStep; // ���� �Ѿ��� ���� ����
        }

        yield return null;
        isShooting = false;
    }

    public IEnumerator FanPattern()
    {
        isShooting = true;

        // ��ä�� ����� ������ ���� ����
        float startAngle = -45f; // ��ä���� ���� ���� (����)
        float endAngle = 45f;    // ��ä���� �� ���� (����)

        // Ÿ�ٰ� ������ ���� ���
        Vector3 targetDirection = (target.position - transform.position).normalized;
        float angleStep = (endAngle - startAngle) / (bulletCount - 1); // �Ѿ� ������ ����
        float angle = startAngle;

        for (int i = 0; i < 10; i++) // 10���� �߻�
        {
            // �Ѿ� ���� �� �߻�
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // ��ä�� ���� ��� (Ÿ�� ����� ��ä�� ���� �߰�)
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 bulletDirection = rotation * targetDirection;

            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

            angle += angleStep; // ���� �Ѿ��� ���� ����
        }

        yield return null; // ��� �Ѿ��� �߻�� ��
        isShooting = false;
    }

    public IEnumerator WavePattern()
    {
        isShooting = true;

        float waveAmplitude = 1f; // �ĵ��� ����
        float waveFrequency = 2f; // �ĵ��� �ֱ�

        for (int i = 0; i < bulletCount; i++)
        {
            // �Ѿ� ����
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // �ĵ� ����
            float angle = i * (360f / bulletCount);
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            // �Ѿ��� �ӵ� �� �ĵ� ȿ�� ����
            rb.velocity = bulletMoveDirection * bulletSpeed;
            rb.angularVelocity = waveFrequency;

            yield return new WaitForSeconds(fireRate); // �߻� ���� ����
        }

        isShooting = false;
    }

    public IEnumerator HorizontalSpreadPattern()
    {
        isShooting = true;

        float startAngle = -45f; // ���ʿ��� ����
        float endAngle = 45f;    // ���������� ��
        float angleStep = (endAngle - startAngle) / (bulletCount - 1); // �� �Ѿ� ������ ����
        float angle = startAngle;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

            angle += angleStep;
            yield return new WaitForSeconds(fireRate); // �߻� ���� ����
        }

        isShooting = false;
    }

    public IEnumerator RandomSpreadPattern()
    {
        isShooting = true;

        float angleStep = 360f / bulletCount; // �� �Ѿ� ������ ����
        for (int i = 0; i < bulletCount; i++)
        {
            // ������ ���� ����
            float randomAngle = Random.Range(0f, 360f);
            float bulletDirX = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            // �Ѿ� ���� �� �߻�
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

            yield return new WaitForSeconds(fireRate); // �߻� ���� ����
        }

        isShooting = false;
    }


}
