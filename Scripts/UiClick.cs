using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class UiClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler , IPointerDownHandler , IPointerUpHandler
{
    
    public GameObject Image;
    public bool Change = false;
    


    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // Image.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Image.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void OnPointerClick(PointerEventData eventData )
    {      
       
        if (eventData.button == PointerEventData.InputButton.Left)
        {             
            Change = !Change;
            Image.SetActive(Change);
            Debug.Log(Change);
        }
       
    }

       


}
