using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform

    // �Ѿ� ������
    public GameObject bulletPrefab360;
    public GameObject bulletPrefabFan;
    public GameObject bulletPrefabWave;
    public GameObject bulletPrefabHorizontal;
    public GameObject bulletPrefabRandom;

    // �߻� ���� ����
    public int bulletCount = 20;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    public float rotationDuration = 2f;
    public float rotationSpeed = 180f;

    // ���Ϻ� ����
    public AudioClip shootSound360;
    public AudioClip shootSoundFan;
    public AudioClip shootSoundWave;
    public AudioClip shootSoundHorizontal;
    public AudioClip shootSoundRandom;

    private AudioSource audioSource;
    private bool isShooting = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayShootSound(AudioClip sound)
    {
        if (sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    // ���Ϻ��� �Ѿ� �������� ��ȯ�ϴ� �޼���
    private GameObject GetBulletPrefab(string pattern)
    {
        switch (pattern)
        {
            case "360":
                return bulletPrefab360;
            case "Fan":
                return bulletPrefabFan;
            case "Wave":
                return bulletPrefabWave;
            case "Horizontal":
                return bulletPrefabHorizontal;
            case "Random":
                return bulletPrefabRandom;
            default:
                return null;
        }
    }

    private AudioClip GetShootSound(string pattern)
    {
        switch (pattern)
        {
            case "360":
                return shootSound360;
            case "Fan":
                return shootSoundFan;
            case "Wave":
                return shootSoundWave;
            case "Horizontal":
                return shootSoundHorizontal;
            case "Random":
                return shootSoundRandom;
            default:
                return null;
        }
    }

    // 360�� ����
    public IEnumerator Shoot360()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("360")); // ���Ϻ� ���� ���

        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            GameObject bullet = Instantiate(GetBulletPrefab("360"), transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletMoveDirection * bulletSpeed;

            angle += angleStep;
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    // ��ä�� ����
    public IEnumerator FanPattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Fan")); // ���Ϻ� ���� ���

        float startAngle = -45f;
        float endAngle = 45f;

        Vector3 targetDirection = (target.position - transform.position).normalized;
        float angleStep = (endAngle - startAngle) / (bulletCount - 1);
        float angle = startAngle;

        for (int i = 0; i < bulletCount; i++)  // ���� 10�� bulletCount�� ����
        {
            GameObject bullet = Instantiate(GetBulletPrefab("Fan"), transform.position, Quaternion.identity);

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 bulletDirection = rotation * targetDirection;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletDirection * bulletSpeed;

            angle += angleStep;
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    // ���� ����
    public IEnumerator WavePattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Wave")); // ���Ϻ� ���� ���

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(GetBulletPrefab("Wave"), transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            float angle = i * (360f / bulletCount);
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            rb.velocity = bulletMoveDirection * bulletSpeed;

            // ���� ����� �������� ���� �߰� �ڵ�
            rb.AddForce(new Vector2(0, Mathf.Sin(Time.time * i)) * 5f);

            yield return new WaitForSeconds(0.1f); // �߻� ���� ����
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    // ���� Ȯ�� ����
    public IEnumerator HorizontalSpreadPattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Horizontal")); // ���Ϻ� ���� ���

        float startAngle = -45f;
        float endAngle = 45f;
        float angleStep = (endAngle - startAngle) / (bulletCount - 1);
        float angle = startAngle;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            GameObject bullet = Instantiate(GetBulletPrefab("Horizontal"), transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletMoveDirection * bulletSpeed;

            angle += angleStep;
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    // ������ Ȯ�� ����
    public IEnumerator RandomSpreadPattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Random")); // ���Ϻ� ���� ���

        for (int i = 0; i < bulletCount; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            float bulletDirX = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            GameObject bullet = Instantiate(GetBulletPrefab("Random"), transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletMoveDirection * bulletSpeed;

            yield return new WaitForSeconds(0.1f); // �߻� ���� ����
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
}
