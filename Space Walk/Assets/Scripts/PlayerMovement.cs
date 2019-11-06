using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public MoveInput moveInput;
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private bool _useKeyboard;
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _maxSpeed = 1f;
    [SerializeField]
    private float _rotationSpeed = 1f;

    public float Speed { 
        get => _speed;
        set
        {
            if (value <= _maxSpeed)
            {
                _speed = value; 
            }
        } 
    }

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        moveInput.OnMove += Move;
        moveInput.OnMove += UpdateRotation;
    }

    private void OnDisable()
    {
        moveInput.OnMove -= Move;
        moveInput.OnMove -= UpdateRotation;
    }
    public void Move(Vector2 direction)
    {
        _rigidbody2D.velocity = _speed * direction;
    }

    private void UpdateRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y,
            direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation =
        Quaternion.RotateTowards(transform.rotation,
            Quaternion.AngleAxis(angle, Vector3.forward),
            _rotationSpeed * Time.deltaTime);
    }
}

public class MoveInput : MonoBehaviour
{
    public Action<Vector2> OnMove;
}
