using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("시야 설정")]
    [SerializeField] public float viewRadius = 10f;
    [SerializeField][Range(0, 360)] public float viewAngle = 90f;
    [SerializeField] public LayerMask targetMask;
    [SerializeField] public LayerMask obstacleMask;

    [HideInInspector] public Transform visibleTarget;

    void Update()
    {
        FindVisibleTarget();
    }

    void FindVisibleTarget()
    {
        visibleTarget = null;
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider col in targetsInRange)
        {
            Transform target = col.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2f)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTarget = target;
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}