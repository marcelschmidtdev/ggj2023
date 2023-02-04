using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private Animator animator;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Up = Animator.StringToHash("Up");
    private static readonly int Down = Animator.StringToHash("Down");
    private static readonly int Left = Animator.StringToHash("Left");
    private static readonly int Right = Animator.StringToHash("Right");

    private static readonly string[] Actions =
    {
        "NoneAction",
        "PickingUpAction",
        "CarryingAction",
        "ThrowingAction",
        "StunnedAction",
    };
    
    private void OnEnable()
    {
        playerState.OnActionChanged += OnPlayerActionChanged;

        OnPlayerActionChanged(playerState.CurrentAction);
    }
    
    private void OnDisable()
    {
        playerState.OnActionChanged -= OnPlayerActionChanged;
    }

    private void LateUpdate()
    {
        UpdateWalking();
    }

    private void UpdateWalking()
    {
        animator.SetBool(Idle, !playerState.IsWalking);
        animator.SetBool(Up, playerState.WalkDirection.y > 0);
        animator.SetBool(Down, playerState.WalkDirection.y < 0);
        animator.SetBool(Left, playerState.WalkDirection.x > 0);
        animator.SetBool(Right, playerState.WalkDirection.x < 0);
    }

    private void OnPlayerActionChanged(PlayerAction action)
    {
        var currentActionStr = $"{action.ToString()}Action";
        foreach (var actionStr in Actions)
        {
            animator.SetBool(actionStr,currentActionStr.Equals(actionStr));
            Debug.Log($"Updating Action to current: {currentActionStr}, selected: {actionStr}, are equal: {currentActionStr.Equals(actionStr)}");
        }
    }
}
