using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        
        // Optionally, start with a default volume (e.g., 0.5f)
        SetVolume(0.5f);
    }

    // Method to change the volume dynamically
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume); // Ensures the volume is between 0 and 1
    }
}
