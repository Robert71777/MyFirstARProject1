using UnityEngine;
using Vuforia;


[RequireComponent(typeof(Animator))]
public class CactusProximity : MonoBehaviour
{
    [Header("Image targets")]
    [SerializeField] private ObserverBehaviour ownTarget;
    [SerializeField] private ObserverBehaviour otherTarget;

    [Header("Distances (meters)")]
    [Tooltip("Characters start attacking when closer than this.")]
    [SerializeField] private float attackDistance = 0.25f;

    [Tooltip("Characters go back to idle only when farther than this (avoids flickering).")]
    [SerializeField] private float releaseDistance = 0.30f;

    private static readonly int IsAttackingHash = Animator.StringToHash("isAttacking");

    private Animator animator;
    private bool isAttacking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsTracked(ownTarget) || !IsTracked(otherTarget))
        {
            SetAttacking(false);
            return;
        }

        float distance = Vector3.Distance(
            ownTarget.transform.position,
            otherTarget.transform.position);

        float threshold = isAttacking ? releaseDistance : attackDistance;
        SetAttacking(distance < threshold);
    }

    private void SetAttacking(bool value)
    {
        if (value == isAttacking)
        {
            return;
        }

        isAttacking = value;
        animator.SetBool(IsAttackingHash, isAttacking);
    }

    private static bool IsTracked(ObserverBehaviour target)
    {
        if (target == null)
        {
            return false;
        }

        Status status = target.TargetStatus.Status;
        return status == Status.TRACKED || status == Status.EXTENDED_TRACKED;
    }
}
