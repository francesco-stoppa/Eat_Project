using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI hint;

    float titleTransparency = 0;
    Color titleColor;
    bool loadTitle = true;
    bool blinkHint = false;

    bool buttonPressed = false;
    void Start()
    {
        if (title)
            titleColor = title.color;

        titleColor.a = titleTransparency;
        title.color = titleColor;

        hint.gameObject.SetActive(false);
        StartCoroutine(LoadingTitle());
    }

    // Update is called once per frame
    void Update()
    {
        if (titleTransparency > 1)
        {
            loadTitle = false;
            blinkHint = true;
        }

        if(!loadTitle && !buttonPressed)
        {
            if(Input.anyKeyDown)
            {
                buttonPressed = true;
                hint.gameObject.SetActive(false);
                blinkHint = false;

                // load main menu
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Debug.Log("load main menu");
            }
        }
    }

    IEnumerator LoadingTitle()
    {
        if(title)
        {
            while (loadTitle)
            {
                titleTransparency += 0.01f;
                titleColor.a = titleTransparency;
                title.color = titleColor;
                yield return new WaitForSeconds(0.05f);
            }
        }

        if(hint)
        {
            while (blinkHint)
            {
                if(!buttonPressed)
                {
                    hint.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    hint.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    hint.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}
