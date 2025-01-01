// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class InteractableItemBase : MonoBehaviour
// {

//     public Sprite Image;

//     public string InteractText = "Press F to pickup the item";


//     public virtual void OnInteract()
//     {
//     }
// }
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Crate : InteractableItemBase {

//     private bool mIsOpen = false;

//     public override void OnInteract()
//     {
//         InteractText = "Press F to ";

//         mIsOpen = !mIsOpen;
//         InteractText += mIsOpen ? "to close" : "to open";

//         GetComponent<Animator>().SetBool("open", mIsOpen);
//     }
// }
