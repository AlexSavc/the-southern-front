using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour, ISelectable
{
    public float scaleForOne = 0.475f;

    public Animator animator;
    public AudioManager sound;

    public SpriteRenderer rend;
    public GameObject spriteObj;

    public GameObject target;
    public Node targetNode;
    public Node parentNode;

    public bool IsActive = false;
    public bool resistDeselection = false;

    public BuyableInfoSO buyableInfo;

    public Player owner;

    public Road()
    {
        
    }

    public void Awake()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        sound = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        spriteObj = rend.gameObject;
        DisableSprite();
    }

    public void OnSelection()
    {
        animator.SetTrigger("OnSelection");
        sound.PlaySelectionSound();

        if (parentNode.HasRoadTo(targetNode) == false)
        {
            OnTryBuyRoad();
        }
    }

    public void OnDeSelection()
    {

    }
    
    void Start()
    {

    }

    public void SetParent(Node node)
    {
        parentNode = node;
    }

    public void SetOwner(Player Owner)
    {
        owner = Owner;
    }

    public void SetTarget(Node targ)
    {
        target = targ.gameObject;
        targetNode = targ;
    }

    public void DisplayRoad()
    {
        if (IsActive)
        {
            resistDeselection = true;
        }
        IsActive = true;
        EnableSprite();
        ReachTo(target.transform.position);
    }

    public void HideBuildableRoad()
    {
        if (targetNode == null) return;
        if (parentNode.HasRoadTo(targetNode)) return;
        if (resistDeselection)
        {
            resistDeselection = false;
            return;
        }
        IncreaseAlpha();
        DisableSprite();
    }

    public void ReachTo(Vector3 targetPos)
    {
        float dist = Vector3.Distance(transform.position, targetPos);
        SetScale(dist);
        SetRotation();
    }

    public void SetScale(float s)
    {
        Vector3 local = spriteObj.transform.localScale;
        spriteObj.transform.localScale = new Vector3(local.x, s*scaleForOne, local.z);
    }

    public void SetRotation()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 rot = transform.localEulerAngles;

        float opposite = transform.position.y - targetPos.y;
        float adjacent = transform.position.x - targetPos.x;

        float angle = Mathf.Atan2(opposite, adjacent);
        angle *= Mathf.Rad2Deg;
        //Idk why i need to add 90 degrees, maybe i forgot a step of the calculation
        transform.localEulerAngles = new Vector3(rot.x, rot.y, angle + 90);
    }

    public void DisplayBuildableRoad(Node node)
    {
        if (IsBuilt()) return;
        if (!IsBuildable(node)) { return; }
        DisplayRoad();
        DecreaseAlpha();
    }

    public bool IsBuildable(Node node)
    {

        //checked for a radius of 1
        int Xdiff = node.X - parentNode.X;
        int Ydiff = node.Y - parentNode.Y;
        if (Xdiff < -1 || Xdiff > 1) return false;
        if (Ydiff < -1 || Ydiff > 1) return false;

        return true;
    }

    public void DecreaseAlpha()
    {
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.3f);
    }

    public void IncreaseAlpha()
    {
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1);
    }

    public void EnableSprite()
    {
        spriteObj.SetActive(true);
    }

    public void DisableSprite()
    {
        IsActive = false;
        spriteObj.SetActive(false);
    }

    public void OnTryBuyRoad()
    {
        QuestionPopupInfo popup = new QuestionPopupInfo
        {
            questionText = "Would you like to buy Road for " + buyableInfo.buyInfo.price,
            NoButtonText = "No",
            YesButtonText = "Yes",
            OnYes = OnBuyRoad
        };
        PopupHandler.Instance.OnQuestionPopup(popup);
    }

    void OnBuyRoad()
    {
        Economy economy = Economy.Instance;
        bool bought;
        economy.OnBuy(new BuyableInfo(buyableInfo.buyInfo), out bought);
        if(bought)
        {
            Player player = economy.CurrentPlayer;
            parentNode.OnBoughtRoad(this);
            owner = player;
            player.AddBuyable(buyableInfo.buyInfo);
        }
    }

    public bool IsBuilt()
    {
        return parentNode.HasRoadTo(targetNode);
    }

    public void DemolishRoad()
    {
        parentNode.OnDestroyRoad(this);
        parentNode.GetOwner().RemoveBuyable(buyableInfo.buyInfo);
    }
}