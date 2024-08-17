using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weapon; // 무기 Transform
    [SerializeField] private Transform character; // 캐릭터 Transform
    [SerializeField] private Transform playerHand; // 캐릭터의 손 위치
    [SerializeField] private float radius = 1.5f; // 무기가 이동할 원의 반지름
    [SerializeField] private Transform shootPoint; // 발사 위치
    [SerializeField] private GameObject[] bulletPrefabs; // 총알 프리팹 배열
    [SerializeField] private float shootForce = 20f; // 발사 속도
    [SerializeField] private PlayerController playerController; // 플레이어 컨트롤러 참조

    public GameObject aimImage; // 조준점 이미지

    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip shootSound;

    void Update()
    {
        if (playerController == null || playerController.health <= 0)
        {
            return; // 플레이어가 없거나 체력이 0이면 총 발사를 막음
        }

        // 마우스 위치 계산
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 2D 게임에서 필요 없는 z축 값을 0으로 설정

        // 손에서 마우스 위치까지의 방향 벡터 계산
        Vector3 direction = mousePosition - playerHand.position;
        direction.z = 0f; // z축을 0으로 설정하여 2D 평면에서만 계산

        // 무기의 회전 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 무기의 위치를 손을 기준으로 원을 따라 이동시킴
        Vector3 weaponPosition = playerHand.position + direction.normalized * radius;
        weapon.position = weaponPosition;

        // 무기의 회전 설정
        weapon.rotation = Quaternion.Euler(0f, 0f, angle);

        // 조준점 위치 설정
        if (aimImage != null)
        {
            aimImage.transform.position = mousePosition; // 마우스 위치로 조준점 이동
        }

        // 총알 발사
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (playerController == null || playerController.health <= 0)
        {
            return; // 플레이어가 없거나 체력이 0이면 총 발사를 막음
        }

        // 랜덤 총알 프리팹 선택
        GameObject selectedBulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];

        // 총알 발사
        GameObject bullet = Instantiate(selectedBulletPrefab, shootPoint.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // 총알 발사 방향 및 속도 설정
            Vector3 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position);
            shootDirection.z = 0f; // z축 값을 0으로 설정하여 2D 평면에서만 사용
            shootDirection.Normalize();

            bulletScript.Initialize(shootDirection, shootForce); // 방향과 속도를 전달

            // 총알의 회전 설정
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
