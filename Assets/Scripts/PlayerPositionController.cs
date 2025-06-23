using UnityEngine;

public class PlayerPositionController : MonoBehaviour
{
    [SerializeField]
    private Transform resetTransform;
    GameObject player;
    [SerializeField]
    Camera playerHead;
    [SerializeField]
    CalibrationManager calibrationManager;

    private void Awake()
    {
        player = this.gameObject;
    }

    private void OnEnable()
    {
        calibrationManager.OnCalibrationComplete += SetResetTransform;
    }

    [ContextMenu("Reset Player Position")]
    public void ResetPlayerPosition()
    {
        var rotationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;
        player.transform.Rotate(0, rotationAngleY, 0);
        var distanceDifference = resetTransform.position - playerHead.transform.position;
        player.transform.position += distanceDifference;
    }

    void SetResetTransform()
    {
        Debug.Log("Setting reset transform position to player position");
        resetTransform.position = playerHead.transform.position;
    }
}
