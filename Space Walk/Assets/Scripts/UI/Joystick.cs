using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IMoveInput
{
    //interface implementations IMoveInput
    public event Action<Vector2> OnMove = (v2) => { };

    public float HandleRange
    {
        get => _handleRange;
        set => _handleRange = Mathf.Abs(value);
    }

    public float DeadZone
    {
        get => _deadZone;
        set => _deadZone = Mathf.Abs(value);
    }


    [SerializeField] 
    private float _handleRange = 1;
    [SerializeField] 
    private float _deadZone = 0;

    [SerializeField]
    private RectTransform _background;
    [SerializeField] 
    private RectTransform _handle;

    private Canvas _canvas;
    private Camera _cam;

    private Vector2 _input = Vector2.zero;
    private bool _stay;

    protected virtual void Start()
    {
        HandleRange = _handleRange;
        DeadZone = _deadZone;
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;
        _cam = _canvas.worldCamera;
    }

    private void Update()
    {
        if (_stay)
        {
            OnMove(_input);
        }
    }

    //interface implementations IPointerDownHandler
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        _stay = true;
    }

    //interface implementations IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, _background.position);
        Vector2 radius = _background.sizeDelta / 2;
        _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        HandleInput(_input.magnitude, _input.normalized);
        _handle.anchoredPosition = _input * radius * _handleRange;
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > _deadZone)
        {
            if (magnitude > 1)
                _input = normalised;
             OnMove(normalised);
        }
        else
        {
            _input = Vector2.zero;
        }
    }

    //interface implementations IPointerUpHandler
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
        _stay = false;
    }
}