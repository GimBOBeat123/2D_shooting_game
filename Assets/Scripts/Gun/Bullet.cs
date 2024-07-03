using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �Ѿ� �ӵ�
    private Vector3 shootDirection; // �Ѿ� �߻� ����

    public void Init(Vector3 direction)
    {
        shootDirection = direction;
    }

    void Update()
    {
        // �Ѿ��� �߻� �������� �̵���Ŵ
        transform.Translate(shootDirection * speed * Time.deltaTime, Space.World);

        //// ȭ���� ����� �Ѿ� ����
        //if (!IsVisibleOnScreen())
        //{
        //    Destroy(gameObject);

        //}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �Ѿ��� �ٸ� ��ü�� �浹�ϸ� ����
        Destroy(gameObject);

    }

    //bool IsVisibleOnScreen()
    //{
    //    // �Ѿ��� ȭ�� �ȿ� �ִ��� üũ
    //    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
    //    return (screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height);
    //}


}
