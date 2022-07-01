using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("<Components>")]
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider coll;

    private float moveSpeed = 30f;

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(h, 0, v).normalized * moveSpeed;
        rigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

}
