using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

[RequireComponent (typeof(LazyFollow))]
public class LazyFollowToggle : MonoBehaviour
{
    [SerializeField] private GameObject lockImage;
    [SerializeField] private GameObject lockUnlockedImage;
    LazyFollow lazyFollow;
    private void Awake()
    {
        lazyFollow = GetComponent<LazyFollow>();
    }

    void Start()
    {
        lazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
        lockUnlockedImage.SetActive(true);
        lockImage.SetActive(false);
    }
    public void ToggleUIFollowing()
    {
        if (lazyFollow.positionFollowMode == LazyFollow.PositionFollowMode.Follow)
        {
            lazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.None;
        }
        else
        {
            lazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
        }
        
        lockUnlockedImage.SetActive(!lockUnlockedImage.activeSelf);
        lockImage.SetActive(!lockImage.activeSelf);
    }
}
