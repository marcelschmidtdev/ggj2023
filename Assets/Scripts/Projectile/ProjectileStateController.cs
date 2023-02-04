using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
public enum ProjectileState
{
    Grounded,
    Idle,
    Fired,
    Broken
}


public class ProjectileStateController : MonoBehaviour
{
    [HideInInspector] public event Action<ProjectileState> OnStateChanged;

    [SerializeField] private Rigidbody2D rigigBody;
    [SerializeField] private CircleCollider2D collider2D;
    [SerializeField] private Animator projectileAnimator;

    [SerializeField] private ProjectileFiredStateController ProjectileFiredStateController;
    [SerializeField] private ProjectileGroundedStateController ProjectileGroundedStateController;

    private AProjectileStateController selectedStateController;
    private ProjectileState projectileState;

    public void Start()
    {
        ActivateGround();
    }

    public void FireProjectile(float strength, int direction)
    {
        ActivateState(ProjectileState.Fired);
        ProjectileFiredStateController.Fire(strength, direction);
        ProjectileFiredStateController.OnLandedAction -= ActivateGround;
        ProjectileFiredStateController.OnLandedAction += ActivateGround;
    }

    private void ActivateGround()
    {
        ActivateState(ProjectileState.Grounded);
    }

    private void ActivateState(ProjectileState newState)
    {
        projectileState = newState;
        OnStateChanged?.Invoke(projectileState);
        switch (projectileState)
        {
            case ProjectileState.Grounded:
                selectedStateController = ProjectileGroundedStateController;
                break;
            case ProjectileState.Fired:
                selectedStateController = ProjectileFiredStateController;
                break;
        }
        selectedStateController.Init(this.transform, this.projectileAnimator, this.collider2D);

    }

    public void Update()
    {
        selectedStateController?.Update();
    }
    public void OnDrawGizmos()
    {
        selectedStateController?.OnDrawGizmos();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        selectedStateController?.OnTriggerEnter2D(other);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        selectedStateController?.OnTriggerStay2D(other);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        selectedStateController?.OnTriggerExit2D(other);
    }
}