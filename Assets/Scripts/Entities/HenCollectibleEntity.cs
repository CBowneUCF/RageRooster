using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenCollectibleEntity : MonoBehaviour, IInteractable
{
    public string henName = "INSERT_HEN_NAME_HERE";
    public WorldChange worldChange;
    public int ammoCount = 1;
    public string hintString;
    public UltEvents.UltEvent interactEvent;

    bool IInteractable.canInteract => true;

    private void Awake()
    {
        if (worldChange == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"This Hen Collectible ({gameObject.name}) is without a WorldChange. It will work for the time being, but it will disable itself in the final build and in testing will not permanently disappear once collected.");
            return;
#else
            gameObject.SetActive(false);
            return;
#endif
        }
        if (worldChange.Enabled) gameObject.SetActive(false);
    }
    bool IInteractable.Interaction()
    {
        PlayerRanged.Ammo.UpdateMax(PlayerRanged.Ammo.maxAmmo + 1);
        worldChange.Enable();
        gameObject.SetActive(false);
        PlayerInteracter.LostInteractable(this);
        UIHUDSystem.Get().ShowHint(hintString);
        interactEvent?.Invoke();
        return true;
    }

    public Vector3 PopupPosition => transform.position + Vector3.up * 2;

}
