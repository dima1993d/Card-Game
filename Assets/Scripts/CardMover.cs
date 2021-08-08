using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility.Variable;

public class CardMover : MonoBehaviour
{
    public CardInfoScriptableObject draggedCard;
    private Camera mainCam;
    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (draggedCard.cardInfo != null)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            draggedCard.cardInfo.rectTransform.localPosition = new Vector3(mousePos.x - Screen.width/2,mousePos.y- Screen.height/2,0) ;
            draggedCard.cardInfo.rectTransform.SetSiblingIndex(draggedCard.cardInfo.rectTransform.parent.childCount - 1);
            //Debug.Log(mousePos.ToString());
        }
    }
}
