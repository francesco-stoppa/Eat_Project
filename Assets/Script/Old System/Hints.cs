using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Hints : MonoBehaviour
{
    Controls input;
    RectTransform rectTransform;

    [SerializeField]
    Button commandIcon;

    [SerializeField]
    float animationVelocity;
    float initialPositionX;
    [SerializeField]
    float finallPositionX = 199.0f;
    bool startAnimation = false;

    private void Awake()
    {
        // SplitHint();
        input = new Controls();
        input.PlayerAction.Quit.started += context => OpenHints();
        rectTransform = this.GetComponent<RectTransform>();
        initialPositionX = rectTransform.anchoredPosition.x;
    }
    public void OpenHints()
    {
        if (rectTransform != null)
        {
            if (startAnimation)
            {
                animationVelocity = -animationVelocity;
            }
            else
            {
                startAnimation = true;
                if (commandIcon)
                    commandIcon.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(startAnimation)
        {
            rectTransform.anchoredPosition += Vector2.left * animationVelocity;
        }

        if (rectTransform.anchoredPosition.x <= finallPositionX && animationVelocity > 0)
        {
            EndAnimation();
        }

        if (rectTransform.anchoredPosition.x >= initialPositionX && animationVelocity < 0)
        {
            EndAnimation();
        }
    }

    void EndAnimation()
    {
        startAnimation = false;
        animationVelocity = -animationVelocity;
        if(commandIcon)
            commandIcon.gameObject.SetActive(true); 
    }

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
