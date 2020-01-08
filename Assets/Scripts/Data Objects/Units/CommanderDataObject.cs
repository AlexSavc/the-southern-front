using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderDataObject : MonoBehaviour
{
    private static CommanderDataObject _instance;
    public static CommanderDataObject Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<Sprite> faces;

    public Sprite selected;
    public Sprite norankBadge;
    public Sprite officerBadge;
    public Sprite captainBadge;
    public Sprite majorBadge;
    public Sprite generalBadge;

    public Sprite legendaryBackround;
    public Sprite laurel;

    public Sprite[] rankSprites;

    void Awake()
    {
        _instance = this;
        rankSprites = new Sprite[] { norankBadge, officerBadge, captainBadge, majorBadge, generalBadge };
    }
}
