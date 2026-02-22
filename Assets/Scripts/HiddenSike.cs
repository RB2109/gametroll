using UnityEngine;

public class HiddenSpikeTrap : MonoBehaviour
{
    public GameObject spikes;          // Assign spike sprite/animator
    public float revealDelay = 0.5f;   // Delay before spikes appear
    private bool triggered = false;

    private void Start()
    {
        spikes.SetActive(false); // Hide spikes initially
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            Invoke(nameof(RevealSpikes), revealDelay);
        }
    }

    private void RevealSpikes()
    {
        spikes.SetActive(true);
        // ⚠️ Notice: no Die() call here
        // The spikes themselves should have a collider + script to kill the player
    }
}
