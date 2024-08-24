using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform character;
    [SerializeField] private Transform playerHand;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private PlayerController playerController;

    public GameObject aimImage;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip shootSound;

    public float damageBoostMultiplier = 2f;
    public float damageBoostDuration = 5f;
    public float damageBoostCooldown = 10f;
    public float powerShotMultiplier = 5f;
    public float powerShotCooldown = 15f;

    private bool isDamageBoostActive = false;
    private bool isDamageBoostReady = true;
    private bool isPowerShotReady = true;

    public Image damageBoostCooldownImage;
    public Image powerShotCooldownImage;

    private Color originalColor;

    void Start()
    {
        if (damageBoostCooldownImage != null)
        {
            originalColor = damageBoostCooldownImage.color;
            SetCooldownImageAlpha(damageBoostCooldownImage, 1f);
        }
        if (powerShotCooldownImage != null)
        {
            SetCooldownImageAlpha(powerShotCooldownImage, 1f);
        }
    }

    void Update()
    {
        if (playerController == null || playerController.health <= 0)
        {
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = mousePosition - playerHand.position;
        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 weaponPosition = playerHand.position + direction.normalized * radius;
        weapon.position = weaponPosition;

        weapon.rotation = Quaternion.Euler(0f, 0f, angle);

        if (aimImage != null)
        {
            aimImage.transform.position = mousePosition;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && isDamageBoostReady)
        {
            StartCoroutine(ActivateDamageBoost());
        }

        if (Input.GetKeyDown(KeyCode.T) && isPowerShotReady)
        {
            StartCoroutine(ActivatePowerShot());
        }
    }

    void Shoot()
    {
        if (playerController == null || playerController.health <= 0)
        {
            return;
        }

        GameObject selectedBulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
        GameObject bullet = Instantiate(selectedBulletPrefab, shootPoint.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            if (isDamageBoostActive)
            {
                bulletScript.damage *= damageBoostMultiplier;
            }

            Vector3 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position);
            shootDirection.z = 0f;
            shootDirection.Normalize();

            bulletScript.Initialize(shootDirection, shootForce);

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private IEnumerator ActivateDamageBoost()
    {
        isDamageBoostActive = true;
        isDamageBoostReady = false;

        yield return new WaitForSeconds(damageBoostDuration); // 데미지 증가 지속 시간

        isDamageBoostActive = false;
        yield return StartCoroutine(CooldownRoutine(damageBoostCooldown, damageBoostCooldownImage));

        isDamageBoostReady = true;
    }

    private IEnumerator ActivatePowerShot()
    {
        isPowerShotReady = false;

        // 강력한 발사 스킬을 사용하면 그 즉시 발사되도록 할 수 있음
        Shoot(); // 강력한 발사

        yield return StartCoroutine(CooldownRoutine(powerShotCooldown, powerShotCooldownImage));

        isPowerShotReady = true;
    }

    private IEnumerator CooldownRoutine(float cooldownTime, Image cooldownImage)
    {
        float elapsed = 0f;
        SetCooldownImageAlpha(cooldownImage, 0.5f); // 쿨타임 동안 반투명

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Clamp01(1 - (elapsed / cooldownTime));
            yield return null;
        }

        cooldownImage.fillAmount = 1f;
        SetCooldownImageAlpha(cooldownImage, 1f); // 쿨타임 종료 후 완전히 보이게
    }

    private void SetCooldownImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
