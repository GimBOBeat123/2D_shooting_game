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
        // Input �ޱ�
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // �ִϸ��̼� ����
        if (movement.magnitude > 0)
        {
            animator.SetFloat("Speed", 1f); // Speed �Ķ���� ����
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // ĳ���� ���� ��ȯ
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ����
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // ����
        }
    }

    void FixedUpdate()
    {
        // ������ �̵�
        rb.velocity = movement * speed;
    }
}
