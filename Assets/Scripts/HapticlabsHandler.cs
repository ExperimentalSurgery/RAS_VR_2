using UnityEngine;

public class HapticlabsHandler : MonoBehaviour
{
    [SerializeField] private string firstTrackName;
    [SerializeField] private string secondTrackName;
    public void StartHapticFeedbackWithFirstTrack()
    {
        // Assuming Hapticlabs SDK has a method called PlayPattern
        Hapticlabs.StartTrack(firstTrackName);
    }
    public void StartHapticFeedbackWithSecondTrack()
    {
        // Assuming Hapticlabs SDK has a method called PlayPattern
        Hapticlabs.StartTrack(secondTrackName);
    }
}
