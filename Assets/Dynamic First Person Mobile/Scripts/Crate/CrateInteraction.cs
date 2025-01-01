using UnityEngine;
using UnityEngine.UI;

public class CrateInteraction : MonoBehaviour
{
    private Animator mAnimator;
    public Button openBtn;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        if (mAnimator == null)
        {
            Debug.LogError("Animator not assigned!");
        }

        openBtn.onClick.AddListener(OnInteract);
        openBtn.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openBtn.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openBtn.gameObject.SetActive(false);
        }
    }

    private void OnInteract()
    {
        mAnimator.SetBool("open", true);
        openBtn.gameObject.SetActive(false);
    }
}
