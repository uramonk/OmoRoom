using System;
using Fusion;
using UnityEngine;

public class PlayerAvatar : NetworkBehaviour
{
    // プレイヤー名のネットワークプロパティを定義する
    [Networked]
    public NetworkString<_16> NickName { get; set; }
    private NetworkCharacterController characterController;
    private NetworkMecanimAnimator networkAnimator;
    private Boolean cameraChanged = false;

    public override void Spawned()
    {
        characterController = GetComponent<NetworkCharacterController>();
        networkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();

        var view = GetComponent<PlayerAvatarView>();
        // プレイヤー名をテキストに反映する
        view.SetNickName(NickName.Value);
        // 自身がアバターの権限を持っているなら、カメラの追従対象にする
        if (HasStateAuthority)
        {
            view.MakeCameraTarget();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraChanged = true;
        }
    }

    // ネットワークオブジェクトの権限を持つプレイヤーでのみ呼び出される
    // 条件分岐（他のプレイヤーのアバターを誤って操作しないようにする処理など）は不要
    public override void FixedUpdateNetwork()
    {
        var view = GetComponent<PlayerAvatarView>();
        // 移動
        var cameraRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        // カメラの座標系を基準として移動方向を計算する
        var direction = view.IsFPS ? inputDirection : cameraRotation * inputDirection;
        characterController.Move(direction);
        // ジャンプ
        if (Input.GetKey(KeyCode.Space))
        {
            characterController.Jump();
        }
        if (cameraChanged)
        {
            cameraChanged = false;
            // カメラの視点を切り替える
            view.SwitchCamera();
        }
        // アニメーション（ここでは説明を簡単にするため、かなり大雑把な設定になっています）
        var animator = networkAnimator.Animator;
        var grounded = characterController.Grounded;
        var vy = characterController.Velocity.y;
        animator.SetFloat("Speed", characterController.Velocity.magnitude);
        animator.SetBool("Jump", !grounded && vy > 4f);
        animator.SetBool("Grounded", grounded);
        animator.SetBool("FreeFall", !grounded && vy < -4f);
        animator.SetFloat("MotionSpeed", 1f);
    }
}