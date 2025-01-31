﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    public GameObject IMovementInputGO;
    private IMoveInput _moveInput;

    [SerializeField]
    private float _linearSpeed = 1f;
    [SerializeField]
    private float _maxLinearSpeed = 1f;
    [SerializeField]
    private float _rotationSpeed = 1f;
    [SerializeField]
    private float _maxRotationSpeed = 1f;

    private Rigidbody2D _rigidbody2D;

    public float Speed { 
        get => _linearSpeed;
        set
        {
            if (value <= _maxLinearSpeed)
            {
                _linearSpeed = value; 
            }
        } 
    }

    public float RotationSpeed
    {
        get => _rotationSpeed;
        set
        {
            if (value <= _maxRotationSpeed)
            {
                _rotationSpeed = value;
            }
        }
    }


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _moveInput = IMovementInputGO.GetComponent<IMoveInput>();
        _moveInput.OnMove += MovementTick;
        _moveInput.OnMove += RotationTick;
    }

    private void OnDisable()
    {
        _moveInput.OnMove -= MovementTick;
        _moveInput.OnMove -= RotationTick;
    }

    public void MovementTick(Vector2 direction)
    {
        _rigidbody2D.velocity = _linearSpeed * direction;
    }

    private void RotationTick(Vector2 direction)
    {
        float degreeAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion quternionAngle = Quaternion.AngleAxis(degreeAngle, Vector3.forward);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, quternionAngle,
            _rotationSpeed * Time.deltaTime);
    }
}
