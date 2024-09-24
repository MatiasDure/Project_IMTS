using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Toolbars;
using UnityEngine;

public class CompanionBehavior : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float smoothFactor=0.2f;
    [SerializeField] private List<string> lineList = new List<string>();
    [SerializeField] private TextMeshPro textMeshProObject;
    private int _textCount = 0;
    private GameObject _arCamera;
    
    private void Awake()
    {
        textMeshProObject.text = lineList[0];
        _textCount += 1;
        _arCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = _arCamera.transform.position + _arCamera.transform.forward * 0.3f + _arCamera.transform.right*0.05f ;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFactor);
        
        transform.LookAt(_arCamera.transform);

        if (Input.touchCount> 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (lineList != null)
                {
                    textMeshProObject.text = lineList[_textCount];

                    _textCount = (_textCount + 1) % lineList.Count;

                }
            }
            
        }
            
    }
}
