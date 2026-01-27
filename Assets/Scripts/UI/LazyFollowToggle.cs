using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

[RequireComponent (typeof(LazyFollow))]
public class LazyFollowToggle : MonoBehaviour
{

    LazyFollow lazyFollow;
    private void Awake()
    {
        lazyFollow = GetComponent<LazyFollow>();
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
    }
}
