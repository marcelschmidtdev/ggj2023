using System;
using UnityEngine;

[Serializable]
public class ProjectileFiredStateController : AProjectileStateController
{
    public event Action OnLandedAction;

    [Range(0.1f, 5)] public float TimeOfTravel;
    protected override ProjectileState AnimationState { get; set; } = ProjectileState.Fired;
    [SerializeField] private AnimationCurve ScaleComponent;


    private double travelFinishTime;
    private (float initial, float final) travelSegment;
    private float velocity = 0;
    private Vector3 initialScale;

    public override void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, ComputeLapsedTimePercent() * Vector3.one);
    }
    public override void Init(Transform t, Animator a, CircleCollider2D c)
    {
        base.Init(t, a, c);
        this.initialScale = this.transform.localScale;

    }
    public override void Update()
    {
        if (velocity != 0)
        {
            float progress = ComputeLapsedTimePercent();
            if (progress == 0)
            {
                Stop();

                OnLandedAction?.Invoke();
            }
            MoveProjectile();
            Debug.Log(progress);
            this.transform.localScale = Vector3.Lerp(initialScale, initialScale + Vector3.one, ScaleComponent.Evaluate(1.0f- progress));

        }
    }
    public void Fire(float travelDistance, int direction)
    {
        velocity = direction * (travelDistance / TimeOfTravel);
        travelFinishTime = Time.time + TimeOfTravel;
        travelSegment = (transform.position.x, transform.position.x + direction * travelDistance);

    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
    }
    private float ComputeLapsedTimePercent()
    {
        return Mathf.Max(0, (float)(travelFinishTime - Time.timeAsDouble) / TimeOfTravel);
    }

    private void MoveProjectile()
    {
        var position = this.transform.position;
        position.x = Mathf.Clamp(this.transform.position.x + velocity * Time.deltaTime, travelSegment.initial, travelSegment.final);
        this.transform.position = position;

    }
    private void Stop()
    {
        velocity = 0;
        travelFinishTime = Time.timeAsDouble;
        this.transform.localScale = initialScale;

    }

}