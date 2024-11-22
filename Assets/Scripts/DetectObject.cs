﻿using System;
using System.Collections;
using Common;
using UnityEngine;

public class DetectObject : MonoBehaviour
{

    private Camera _mainCamera;

    private Rigidbody _rigidbodyToJump = null;

    private Vector3 _totalForce = Vector3.zero;
    private float _forceFactor = 5f;

    
    void Start()
    {
        _mainCamera = Camera.main;

        TouchManager.Instance.OnTouchBegan += OnTouchBegan;
        TouchManager.Instance.OnTouchMoved += OnTouchMoved;
        TouchManager.Instance.OnTouchEnded += OnTouchEnded;
    }


    private void OnDestroy()
    {
        if (TouchManager.Instance == null)
            return;

        TouchManager.Instance.OnTouchBegan -= OnTouchBegan;
        TouchManager.Instance.OnTouchMoved -= OnTouchMoved;
    }

    private void OnTouchBegan(TouchData data)
    {
        CastRay(data.position);
    }


    private void OnTouchMoved(TouchData data)
    {
        if (_rigidbodyToJump != null)
        {
            _totalForce += new Vector3(data.deltaPosition.x, 1f, data.deltaPosition.y) * Time.deltaTime;
        }
    }

    private void OnTouchEnded(TouchData data)
    {
        _rigidbodyToJump = null;
    }


    private void CastRay(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Object name: " + hit.transform.name);
            if (hit.rigidbody != null)
            {
                _rigidbodyToJump = hit.rigidbody;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_rigidbodyToJump != null)
        {
            
            _rigidbodyToJump.AddForce(_totalForce * _forceFactor, ForceMode.Impulse);
            _totalForce = Vector3.zero;
        }

    }


}