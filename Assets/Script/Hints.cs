using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{
    [SerializeField]
    Image eatImage;
    [SerializeField]
    Image splitImage;

    private void Start()
    {
        SplitHint();
    }
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
    }
}
