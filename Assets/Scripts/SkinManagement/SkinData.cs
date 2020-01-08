using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkinData 
{
    public string skinName;

    public SerializeTexture panel;
    public SerializeTexture button;
    public SerializeTexture technicalButton;
    public SerializeTexture element;
    public SerializeTexture header;

    public List<Color> colors;
}
