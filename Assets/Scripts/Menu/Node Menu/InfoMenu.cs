using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void SetDisplay(NodeDescription description)
    {
        image.sprite = description.nodeSprite;
        image.color = description.color;
        title.text = description.nodeName;
        this.description.text = description.nodeDescription;
    }
}
