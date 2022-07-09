using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("<Components>")]

    [SerializeField] private float moveSpeed = 100f;

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(h, 0, v).normalized * moveSpeed;
        transform.position = transform.position + velocity * Time.deltaTime; 
    }

}
