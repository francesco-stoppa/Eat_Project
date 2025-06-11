using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Hints : MonoBehaviour
{/*
    [SerializeField]
    Image eatImage;
    [SerializeField]
    Image splitImage;*/

    Controls input;
    RectTransform rectTransform;


    private void Awake()
    {
        // SplitHint();
        input = new Controls();
        input.PlayerAction.Quit.started += context => OpenHints();
        rectTransform = this.GetComponent<RectTransform>();
    }
    public void OpenHints()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector3(199, 48, 200);
        }
    }
    /*
    public void SplitHint()
    {
        if(eatImage)
            eatImage.gameObject.SetActive(true);
        if (splitImage)
            splitImage.gameObject.SetActive(false);

    }
    public void EatHint()
    {
        if (splitImage)
            splitImage.gameObject.SetActive(true);
        if (eatImage)
            eatImage.gameObject.SetActive(false);
    }*/
   #region InputMap
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    #endregion
}
