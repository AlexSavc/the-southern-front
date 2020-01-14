using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    private static StructureManager _instance;
    public static StructureManager Instance { get { return _instance; } }

    public void Awake()
    {
        _instance = this;
    }

    public void BuildStructure(Structure toBuild, Node node)
    {
        //Pass prefabs into here, because they will be instantiated
        if(node.Structure != null)
        {
            node.Structure.Demolish();
        }
        else
        {
            GameObject obj = Instantiate(toBuild.gameObject, node.transform);
            Structure newStructure = obj.GetComponent<Structure>();
            node.SetStructure(newStructure);
            newStructure.SetOwner(node);
        }

        if(toBuild.StructureName == "Gulag")
        {
            AudioManager.Instance.PlaySovietAnthem();
        }

        OpenNodeMenu.Instance.GoBack();
    }
}
