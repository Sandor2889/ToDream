using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider coll;

    private float moveSpeed = 30f;

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Item"))
        {
            inventory.AcquireItem(collision.gameObject.GetComponent<ItemPickUp>().item);
            Destroy(collision.gameObject);
        }
    }


    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(h, 0, v).normalized * moveSpeed;
        rigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

}
