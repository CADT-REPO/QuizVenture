using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    private AudioSource audioSource;
    public Button toggleSoundButton; // Reference to the Button
    public Sprite soundOnImage; // Image for "Sound On"
    public Sprite soundOffImage; // Image for "Sound Off"

    private void Start()
    {
        // Find the GameObject with the AudioSource
        GameObject audioObject = GameObject.Find("GameObject"); // Replace with your AudioSource GameObject name
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
        }

        // Set the initial button image
        UpdateButtonImage();
    }

    public void ToggleSound()
    {
        if (audioSource != null)
        {
            audioSource.mute = !audioSource.mute; // Toggle the sound
            UpdateButtonImage(); // Update the button image
        }
    }

    private void UpdateButtonImage()
    {
        if (audioSource.mute)
        {
            toggleSoundButton.image.sprite = soundOffImage; // Set to "Sound Off" image
        }
        else
        {
            toggleSoundButton.image.sprite = soundOnImage; // Set to "Sound On" image
        }
    }
}
