using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform

    // 총알 프리팹
    public GameObject bulletPrefab360;
    public GameObject bulletPrefabFan;
    public GameObject bulletPrefabWave;
    public GameObject bulletPrefabHorizontal;
    public GameObject bulletPrefabRandom;

    // 발사 관련 변수
    public int bulletCount = 20;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    public float rotationDuration = 2f;
    public float rotationSpeed = 180f;

    // 패턴별 사운드
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

    // 패턴별로 총알 프리팹을 반환하는 메서드
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

    // 360도 패턴
    public IEnumerator Shoot360()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("360")); // 패턴별 사운드 재생

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

    // 부채꼴 패턴
    public IEnumerator FanPattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Fan")); // 패턴별 사운드 재생

        float startAngle = -45f;
        float endAngle = 45f;

        Vector3 targetDirection = (target.position - transform.position).normalized;
        float angleStep = (endAngle - startAngle) / (bulletCount - 1);
        float angle = startAngle;

        for (int i = 0; i < bulletCount; i++)  // 기존 10을 bulletCount로 변경
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

    // 물결 패턴
    public IEnumerator WavePattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Wave")); // 패턴별 사운드 재생

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(GetBulletPrefab("Wave"), transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            float angle = i * (360f / bulletCount);
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            rb.velocity = bulletMoveDirection * bulletSpeed;

            // 물결 모양의 움직임을 위한 추가 코드
            rb.AddForce(new Vector2(0, Mathf.Sin(Time.time * i)) * 5f);

            yield return new WaitForSeconds(0.1f); // 발사 간격 조정
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    // 수평 확산 패턴
    public IEnumerator HorizontalSpreadPattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Horizontal")); // 패턴별 사운드 재생

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

    // 무작위 확산 패턴
    public IEnumerator RandomSpreadPattern()
    {
        isShooting = true;
        PlayShootSound(GetShootSound("Random")); // 패턴별 사운드 재생

        for (int i = 0; i < bulletCount; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            float bulletDirX = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            GameObject bullet = Instantiate(GetBulletPrefab("Random"), transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletMoveDirection * bulletSpeed;

            yield return new WaitForSeconds(0.1f); // 발사 간격 조정
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
}
