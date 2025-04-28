using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerAvatarView : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera firstPersonCamera;
    [SerializeField]
    private CinemachineCamera thirdPersonCamera;
    [SerializeField]
    private bool isFPS;
    public bool IsFPS => isFPS;

    [SerializeField]
    private TextMeshPro nameLabel;

    public void MakeCameraTarget()
    {
        SetThirdPersonCamera();
    }

    /// <summary>
    /// 視点の切り替えを実行する
    /// </summary>
    [ContextMenu("SwitchCamera")]
    public void SwitchCamera()
    {
        if (isFPS)
        {
            SetThirdPersonCamera();
        }
        else
        {
            SetFirstPersonCamera();
        }
    }

    /// <summary>
    /// 一人称視点に切り替える
    /// </summary>
    private void SetFirstPersonCamera()
    {
        firstPersonCamera.Priority = 10;
        thirdPersonCamera.Priority = 0;
        isFPS = true;
    }

    /// <summary>
    /// 三人称視点に切り替える
    /// </summary>
    private void SetThirdPersonCamera()
    {
        firstPersonCamera.Priority = 0;
        thirdPersonCamera.Priority = 10;
        isFPS = false;
    }

    public void SetNickName(string nickName)
    {
        nameLabel.text = nickName;
    }

    private void LateUpdate()
    {
        // プレイヤー名のテキストを、ビルボード（常にカメラ正面向き）にする
        nameLabel.transform.rotation = Camera.main.transform.rotation;
    }

}