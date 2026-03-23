using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 左右移動のみ（A/D または左右矢印）。W/S や上下矢印は使わない。
/// </summary>
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        float horizontal = 0f;
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.leftArrowKey.isPressed || kb.aKey.isPressed)
                horizontal -= 1f;
            if (kb.rightArrowKey.isPressed || kb.dKey.isPressed)
                horizontal += 1f;
        }

        var pos = transform.position;
        pos.x += horizontal * moveSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
