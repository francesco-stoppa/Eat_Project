using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class WorldSelection : MonoBehaviour
{
    // Singelton
    SaveSystem saveSystem;
    // Input Map
    Controls input;

    [SerializeField]
    ChangeWorld changeWorld;
    // Variables UI
    [SerializeField]
    Image left_hint;
    [SerializeField]
    Image right_hint;
    [SerializeField]
    Image left_button;
    [SerializeField]
    Image right_button;
    [SerializeField]
    Image select_button;

    [SerializeField]
    TextMeshProUGUI left_text;
    [SerializeField]
    TextMeshProUGUI right_text;
    [SerializeField]
    TextMeshProUGUI select_text;

    // Varialbes
    int currentWorld;
    bool blockInput = false;
    List<Image> inputsImage = new List<Image>();
    List<TextMeshProUGUI> inputsText = new List<TextMeshProUGUI>();

    private void Awake()
    {
        // Input Map
        input = new Controls();
        // Direction
        input.PlayerMovement.Left.started += context => MoveLeft();
        input.PlayerMovement.Right.started += context => MoveRight();
        // Select
        input.PlayerAction.EatSplit.started += context => Select();
        // Quit
        // input.PlayerAction.Reset.started += context => Reset();
    }
    void Start()
    {
        // Singelton
        saveSystem = SaveSystem.Instance;
        currentWorld = saveSystem.GetWorld();
        LoadWorldUI();
        blockInput = false;
        if (changeWorld)
            changeWorld.SetScript(this);

        Debug.Log(currentWorld);


        if (left_hint)
            inputsImage.Add(left_hint);
        if(right_hint) 
            inputsImage.Add(right_hint);
        if(left_button)
            inputsImage.Add(left_button);
        if(right_button)
            inputsImage.Add(right_button);
        if(select_button)
            inputsImage.Add(select_button);

        if(left_text)
            inputsText.Add(left_text);
        if(right_text)
            inputsText.Add(right_text);
        if(select_text)
            inputsText.Add(select_text);
    }

    // Update is called once per frame
    void LoadWorldUI()
    {
        // load on ui the last world you unlock = currentWorld
        if (currentWorld == saveSystem.GetWorld())
        {
            if (right_button)
            {
                Color color = right_button.color;
                color.a = 0.25f;
                right_button.color = color;
            }
            if (right_text)
            {
                Color color = right_text.color;
                color.a = 0.25f;
                right_text.color = color;
            }
            if (right_hint)
            {
                Color color = right_hint.color;
                color.a = 0.25f;
                right_hint.color = color;
            }
        }
        if (currentWorld == 0)
        {
            if (left_button)
            {
                Color color = left_button.color;
                color.a = 0.25f;
                left_button.color = color;
            }
            if (left_text)
            {
                Color color = left_text.color;
                color.a = 0.25f;
                left_text.color = color;
            }
            if (left_hint)
            {
                Color color = left_hint.color;
                color.a = 0.25f;
                left_hint.color = color;
            }
        }
        if (currentWorld == 4)
        {
            if (select_button)
            {
                Color color = select_button.color;
                color.a = 0.25f;
                select_button.color = color;
            }
            if (select_text)
            {
                Color color = select_text.color;
                color.a = 0.25f;
                select_text.color = color;
            }
        }
    }
    void BlockInputs(bool block)
    {
        float a = 0f;
        if (block)
            a = 0.25f;
        else
            a = 1;

        foreach (Image image in inputsImage)
        {
            Color color = image.color;
            color.a = a;
            image.color = color;
        }
        foreach (TextMeshProUGUI text in inputsText)
        {
            Color color = text.color;
            color.a = a;
            text.color = color;
        }
    }
    void Select()
    {
        if (blockInput)
            return;

        Debug.Log(currentWorld);

        saveSystem.SetLevel(1);
        if (currentWorld == 4)
            return;


        switch(currentWorld)
        {
            case 0:
                SceneManager.LoadScene(2);
                break;
            case 1:
                SceneManager.LoadScene(12);
                break;
            case 2:
                SceneManager.LoadScene(22);
                break;
            case 3:
                SceneManager.LoadScene(32);
                break;
            case 4:
                SceneManager.LoadScene(42);
                break;
        }
    }
    void MoveLeft()
    {
        if (blockInput)
            return;
        if (currentWorld > 0)
            ChangeWorld(false);
        // else
            // lock
    }
    void MoveRight()
    {
        if (blockInput)
            return;

        if (currentWorld < saveSystem.GetWorld())
            ChangeWorld(true);
        // else
            // lock
    }

    void ChangeWorld(bool isIncreasing)
    {
        // lock input

        int mod = 0;
        if(isIncreasing)
        {
            mod = +1;
        }
        else
        {
            mod = -1;
        }
        currentWorld += mod;

        blockInput = true;
        BlockInputs(true);
        // start animation
        if (changeWorld)
            changeWorld.StartAnim(isIncreasing);
    }

    public void UnlockInput()
    {
        blockInput = false; 
        BlockInputs(false);
        LoadWorldUI();
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
