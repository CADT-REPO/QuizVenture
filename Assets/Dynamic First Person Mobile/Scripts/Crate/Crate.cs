using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Default,
    Consumable,
    Weapon
}

public class InteractableItemBase : MonoBehaviour
{
    public string Name;

    public Sprite Image;

    public string InteractText = "Press F to pickup the item";

    public EItemType ItemType;

    public virtual void OnInteractAnimation(Animator animator)
    {
        animator.SetTrigger("tr_pickup");
    }

    public virtual void OnInteract()
    {
    }

    public virtual bool CanInteract(Collider other)
    {
        return true;   
    }
}
public class Crate : InteractableItemBase {

    private bool mIsOpen = false;

    public override void OnInteract()
    {
        InteractText = "Press F to ";

        mIsOpen = !mIsOpen;
        InteractText += mIsOpen ? "to close" : "to open";

        GetComponent<Animator>().SetBool("open", mIsOpen);
    }
}
