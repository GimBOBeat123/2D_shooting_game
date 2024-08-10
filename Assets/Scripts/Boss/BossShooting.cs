using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public Transform target; // 캐릭터의 Transform
    public GameObject bulletPrefab; // 총알 프리팹을 연결할 변수
    public int bulletCount = 20; // 발사할 총알의 개수
    public float bulletSpeed = 5f; // 총알의 속도
    public float fireRate = 2f; // 총알 발사 주기
    public float rotationDuration = 2f; // 회전 탄막 패턴의 지속 시간
    public float rotationSpeed = 180f; // 보스의 회전 속도 (도/초)

    private bool isShooting = false;

    public IEnumerator Shoot360()
    {
        isShooting = true;

        float angleStep = 360f / bulletCount; // 각 총알 사이의 각도
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // 각도에 따라 총알의 방향 계산
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            // 총알 생성 및 발사
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

            angle += angleStep; // 다음 총알의 각도 증가
        }

        yield return null;
        isShooting = false;
    }

    public IEnumerator FanPattern()
    {
        isShooting = true;

        // 부채꼴 모양을 형성할 각도 범위
        float startAngle = -45f; // 부채꼴의 시작 각도 (좌측)
        float endAngle = 45f;    // 부채꼴의 끝 각도 (우측)

        // 타겟과 보스의 방향 계산
        Vector3 targetDirection = (target.position - transform.position).normalized;
        float angleStep = (endAngle - startAngle) / (bulletCount - 1); // 총알 사이의 각도
        float angle = startAngle;

        for (int i = 0; i < 10; i++) // 10개만 발사
        {
            // 총알 생성 및 발사
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 부채꼴 방향 계산 (타겟 방향과 부채꼴 각도 추가)
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 bulletDirection = rotation * targetDirection;

            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

            angle += angleStep; // 다음 총알의 각도 증가
        }

        yield return null; // 모든 총알이 발사된 후
        isShooting = false;
    }

    public IEnumerator WavePattern()
    {
        isShooting = true;

        float waveAmplitude = 1f; // 파동의 높이
        float waveFrequency = 2f; // 파동의 주기

        for (int i = 0; i < bulletCount; i++)
        {
            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // 파동 방향
            float angle = i * (360f / bulletCount);
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            // 총알의 속도 및 파동 효과 적용
            rb.velocity = bulletMoveDirection * bulletSpeed;
            rb.angularVelocity = waveFrequency;

            yield return new WaitForSeconds(fireRate); // 발사 간격 조정
        }

        isShooting = false;
    }

    public IEnumerator HorizontalSpreadPattern()
    {
        isShooting = true;

        float startAngle = -45f; // 왼쪽에서 시작
        float endAngle = 45f;    // 오른쪽으로 끝
        float angleStep = (endAngle - startAngle) / (bulletCount - 1); // 각 총알 사이의 각도
        float angle = startAngle;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

            angle += angleStep;
            yield return new WaitForSeconds(fireRate); // 발사 간격 조정
        }

        isShooting = false;
    }

    public IEnumerator RandomSpreadPattern()
    {
        isShooting = true;

        float angleStep = 360f / bulletCount; // 각 총알 사이의 각도
        for (int i = 0; i < bulletCount; i++)
        {
            // 무작위 각도 생성
            float randomAngle = Random.Range(0f, 360f);
            float bulletDirX = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            // 총알 생성 및 발사
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

            yield return new WaitForSeconds(fireRate); // 발사 간격 조정
        }

        isShooting = false;
    }


}
