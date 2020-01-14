using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Node))]
public class Builder : MonoBehaviour
{
    [SerializeField]
    private List<Structure> buildable;
    public List<Structure> Buildable { get { return buildable; } }

    private StructureManager manager;
    private Node node;

    public bool debugBuild = false;
    public bool built = false;

    void Start()
    {
        manager = StructureManager.Instance;
        node = GetComponent<Node>();
    }

    void Update()
    {
        if (!built)
        {
            if (debugBuild)
            {
                OnTryBuild(buildable[0]);
                built = true;
            }
        }
        else if (!debugBuild) { Demolish(); built = false; }
    }

    /*public void OnTryBuild(object obj)
    {
        if(obj.GetType() == typeof(string))
        {
            foreach(Structure build in buildable)
            {
                if(build.StructureName == (string)obj)
                {
                    Build(build);
                    return;
                }
            }
        }
    }*/

    public void OnTryBuild(object obj)
    {
        if (obj.GetType() == typeof(Structure))
        {
            if(Buildable.Contains((Structure)obj))
            {
                Build((Structure)obj);
            }
        }
        else Debug.LogError("Builder.OnTryBuild is not a structure");
    }

    private void Demolish()
    {
        node.Structure.Demolish();
    }

    private void Build(Structure toBuild)
    {
        manager.BuildStructure(toBuild, node);
    }

    public List<RoundButtonData> GetBuildableData()
    {
        List<RoundButtonData> buttonData = new List<RoundButtonData>();
        foreach(Structure build in buildable)
        {
            RoundButtonData data = new RoundButtonData
            {
                buttonDelegate = OnTryBuild,
                sprite = build.sprite,
                text = build.StructureName,
                obj = build
            };
            buttonData.Add(data);
        }

        return buttonData;
    }
}