﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI paragraph;

    [SerializeField]
    private Image image;

    public void SetElemet(Sprite sprite, string Title, string Text)
    {
        image.sprite = sprite;
        title.text = Title;
        paragraph.text = Text;
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetParagraph(string text)
    {
        paragraph.text = text;
    }
}
