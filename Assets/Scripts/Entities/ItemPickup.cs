using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        Coin,
        Health,
        Hint,
        Upgrade,
        Wishbone,
        Hen
    }

    public ItemType type;
    public string upgradeName;
    public Upgrade activateUpgrade;
    [TextArea]
    public string hintString;
    public int addAmount = 1;
    public WorldChange oneTime;


    private void Awake()
    {
        if(oneTime != null && oneTime.Enabled) gameObject.SetActive(false);
        if(type == ItemType.Wishbone && oneTime == null)
        {
#if UNITY_EDITOR
            Debug.LogError($"This wishbone {gameObject.name} does not have a world change.");
#endif
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (type == ItemType.Coin)
        {
            GlobalState.AddCurrency(addAmount);
        }
        else if (type == ItemType.Health)
        {
            health.Heal(1);
            
        }
        else if (type == ItemType.Hint)
        {
            UIHUDSystem.Get().ShowHint(hintString);
        }
        else if (type == ItemType.Upgrade)
        {
            activateUpgrade.value = true;
            //if(upgradeName == "Health") health.AddMaxHealth();
            //else FindObjectOfType<PlayerStateMachine>().SetUpgrade(upgradeName, true);
            UIHUDSystem.Get().ShowHint(hintString);
        }
        else if (type == ItemType.Wishbone)
        {
            PlayerHealth.Global.UpdateMax(PlayerHealth.Global.maxHealth + addAmount);
            UIHUDSystem.Get().ShowHint(hintString);
        }
        else if(type == ItemType.Hen)
        {
            UIHUDSystem.Get().ShowHint(hintString);
        }

        if (type != ItemType.Hint) 
        {
            DOTween.Kill(this.gameObject.transform);
            Destroy(gameObject);
        }


        if (oneTime != null && oneTime)
        {
            oneTime.Enable();
        }
    }




}