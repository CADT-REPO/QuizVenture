using UnityEngine;
using UnityEngine.UI;

public class ButtonScale : MonoBehaviour
{
    private Vector3 defaultScale;
    private Vector3 clickedScale;

    void Start()
    {
        // Save the default scale and define the clicked scale
        defaultScale = new Vector3(3f, 3f, 3f);
        clickedScale = new Vector3(3.2f, 3.2f, 3.2f);

        // Ensure the button starts at the default scale
        transform.localScale = defaultScale;

        // Add listeners if the Button component is present
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick()
    {
        // Animate the button scale to the clicked scale
        StartCoroutine(ScaleAnimation(clickedScale, defaultScale, 0.1f));
    }

    private System.Collections.IEnumerator ScaleAnimation(Vector3 targetScale, Vector3 returnScale, float duration)
    {
        // Animate to the target scale
        yield return AnimateScale(targetScale, duration);

        // Animate back to the default scale
        yield return AnimateScale(returnScale, duration);
    }

    private System.Collections.IEnumerator AnimateScale(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
