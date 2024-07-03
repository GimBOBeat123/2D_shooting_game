using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 총알 속도
    private Vector3 shootDirection; // 총알 발사 방향

    public void Init(Vector3 direction)
    {
        shootDirection = direction;
    }

    void Update()
    {
        // 총알을 발사 방향으로 이동시킴
        transform.Translate(shootDirection * speed * Time.deltaTime, Space.World);

        //// 화면을 벗어나면 총알 제거
        //if (!IsVisibleOnScreen())
        //{
        //    Destroy(gameObject);

        //}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 총알이 다른 객체와 충돌하면 제거
        Destroy(gameObject);

    }

    //bool IsVisibleOnScreen()
    //{
    //    // 총알이 화면 안에 있는지 체크
    //    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
    //    return (screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height);
    //}


}
