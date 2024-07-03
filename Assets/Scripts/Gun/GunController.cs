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

    public GameObject aimImage; // 조준점 이미지

    void Update()
    {
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

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 총알을 발사할 방향 계산 (마우스 위치 기준)
        Vector3 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position).normalized;

        // 랜덤 총알 프리팹 선택
        GameObject selectedBulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];

        // 총알 발사
        GameObject bullet = Instantiate(selectedBulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDirection * shootForce; // 총알에 발사 속도 적용
        }
    }
}
