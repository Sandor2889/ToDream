using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    // use input to move sphere

    public Rigidbody _sphereRb;
    public Rigidbody _carRB;
    
    private float _moveInput;
    private float _turnInput;
    private bool _isGrounded;

    public float _fwdSpeed;
    public float _revSpeed;
    public float _turnSpeed;
    public LayerMask _groundLayer;

    public float _normalDrag;
    public float _modifiedDrag;

    public float _alignToGroundTime;

    private void Start()
    {
        _sphereRb.transform.parent = null;
        _carRB.transform.parent = null;

        _normalDrag = _sphereRb.drag;
    }

    private void Update()
    {
        // get input
        _moveInput = Input.GetAxisRaw("Vertical");
        _turnInput = Input.GetAxisRaw("Horizontal");
        
        // set cars rotation
        float newRotation = _turnInput * _turnSpeed * Time.deltaTime * _moveInput;
        if(_isGrounded) transform.Rotate(0, newRotation, 0, Space.World);

        // set cars position to sphere
        transform.position = _sphereRb.transform.position;

        // raycast ground check
        RaycastHit hit;
	    _isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 2f, _groundLayer);
	    Debug.Log(hit.normal);
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, _alignToGroundTime * Time.deltaTime);

        // adjust speed for car
        _moveInput *= _moveInput > 0 ? _fwdSpeed : _revSpeed;
        _sphereRb.drag = _isGrounded ? _normalDrag : _modifiedDrag;
    }

    private void FixedUpdate()
    {
        if(_isGrounded)
        {
            // move car
            _sphereRb.AddForce(transform.forward * _moveInput, ForceMode.Acceleration);
        }
        else
        {
            // add extra gravity
            _sphereRb.AddForce(transform.up * -30f);
        }

        _carRB.MoveRotation(transform.rotation);
    }
}
