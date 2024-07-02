using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Input 받기
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // 애니메이션 제어
        if (movement.magnitude > 0)
        {
            animator.SetFloat("Speed", 1f); // Speed 파라미터 설정
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // 캐릭터 방향 전환
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 좌측
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // 우측
        }
    }

    void FixedUpdate()
    {
        // 물리적 이동
        rb.velocity = movement * speed;
    }
}
