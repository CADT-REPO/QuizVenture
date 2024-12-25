using UnityEngine;

public class ConfettiEffect : MonoBehaviour
{
    public ParticleSystem confetti;

    public void PlayConfetti()
    {
        if (confetti != null)
        {
            confetti.Play();
        }
    }
}
