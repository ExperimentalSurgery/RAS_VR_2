using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Track
{
    public string name;
    public float duration;
}
public class HapticlabsHandler : MonoBehaviour
{
    [SerializeField] List <Track> tracks = new List<Track>();

    // One looping coroutine per track (or per stylus button)
    private readonly Dictionary<int, Coroutine> _running = new Dictionary<int, Coroutine>();

    public void StartHapticFeedbackWithTrack(int index)
    {
        Debug.Log($"Request to start haptic track at index: {index}");
        if (index < 0 || index >= tracks.Count)
        {
            Debug.LogWarning($"Invalid track index: {index}");
            return;
        }

        // If already running, do nothing (or restart if you prefer)
        if (_running.ContainsKey(index))
            return;

        var track = tracks[index];
        _running[index] = StartCoroutine(PlayTrackInLoop(track));

    }

    public void StopTrack(int index)
    {
        if (_running.TryGetValue(index, out var co) && co != null)
        {
            StopCoroutine(co);
            _running.Remove(index);

            // this will stop all haptics globally
            //Hapticlabs.Stop();
        }
    }
    

    IEnumerator PlayTrackInLoop(Track track)
    {
        float durationMS = track.duration * 0.001f;
        while (true)
        {
            Hapticlabs.StartTrack(track.name);
            Debug.Log($"Playing haptic track: {track.name}");
            yield return new WaitForSeconds(durationMS); 
        }
    }

    private void OnDisable()
    {
        // Safety: stop everything if object is disabled
        foreach (var kv in _running)
        {
            if (kv.Value != null) StopCoroutine(kv.Value);
        }
        _running.Clear();
        Hapticlabs.Stop();
    }
}
