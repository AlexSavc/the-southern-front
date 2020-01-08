using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSlot : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI title;
    public Slider slider;
    public TextMeshProUGUI secondLine;
    public Unit unit;
    public DraggableImage draggableImage;

    public void SetData(Unit unit)
    {
        image.sprite = unit.sprite;
        title.text = unit.unitName;
        slider.maxValue = unit.TotalHealth;
        slider.value = unit.Health;
        secondLine.text = "Strength: "+unit.strength;
        this.unit = unit;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
