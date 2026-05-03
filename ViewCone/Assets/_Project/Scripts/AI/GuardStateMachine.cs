using UnityEngine;

public class GuardStateMachine : MonoBehaviour
{
    public enum GuardState
    {
        Patrol,
        Suspicious,
        Alert,
        Chase,
        Search
    }

    [Header("의심도")]
    [SerializeField] private float suspicionIncreaseRate = 20f;
    [SerializeField] private float suspicionDecreaseRate = 10f;
    [SerializeField] private float suspicionMax = 100f;

    [Header("수색")]
    [SerializeField] private float searchDuration = 5f;

    [HideInInspector] public GuardState currentState = GuardState.Patrol;
    [HideInInspector] public float suspicionLevel = 0f;
    [HideInInspector] public Vector3 lastKnownPosition;

    private FieldOfView _fov;
    private float _searchTimer = 0f;

    void Start()
    {
        _fov = GetComponent<FieldOfView>();
    }

    void Update()
    {
        UpdateSuspicion();
        UpdateState();
    }

    void UpdateSuspicion()
    {
        if (_fov.visibleTarget != null)
        {
            lastKnownPosition = _fov.visibleTarget.position;
            suspicionLevel += suspicionIncreaseRate * Time.deltaTime;
        }
        else
        {
            suspicionLevel -= suspicionDecreaseRate * Time.deltaTime;
        }

        suspicionLevel = Mathf.Clamp(suspicionLevel, 0f, suspicionMax);
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case GuardState.Patrol:
            case GuardState.Suspicious:
            case GuardState.Alert:
                if (suspicionLevel <= 0f) SetState(GuardState.Patrol);
                else if (suspicionLevel < 30f) SetState(GuardState.Suspicious);
                else if (suspicionLevel < 70f) SetState(GuardState.Alert);
                else SetState(GuardState.Chase);
                break;

            case GuardState.Chase:
                if (_fov.visibleTarget == null)
                {
                    SetState(GuardState.Search);
                    _searchTimer = searchDuration;
                }
                break;

            case GuardState.Search:
                _searchTimer -= Time.deltaTime;
                if (_fov.visibleTarget != null)
                    SetState(GuardState.Chase);
                else if (_searchTimer <= 0f)
                {
                    SetState(GuardState.Patrol);
                }
                break;
        }
    }

    [Header("협동")]
    [SerializeField] private float alertRadius = 15f;

    void AlertNearbyGuards()
    {
        Collider[] guards = Physics.OverlapSphere(transform.position, alertRadius, LayerMask.GetMask("Guard"));
        Debug.Log($"주변 가드 감지: {guards.Length}명");
        foreach (Collider col in guards)
        {
            GuardStateMachine other = col.GetComponent<GuardStateMachine>();
            if (other != null && other != this)
            {
                other.suspicionLevel = 70f;
                other.lastKnownPosition = lastKnownPosition;
                Debug.Log($"{col.name} Alert!");
            }
        }
    }

    public void ReceiveAlert(Vector3 position, float lockDuration = 3f)
    {
        suspicionLevel = 70f;
        lastKnownPosition = position;
    }

    void SetState(GuardState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        if (newState == GuardState.Chase)
            AlertNearbyGuards();

        Debug.Log($"상태 변경: {currentState} / 의심도: {suspicionLevel:0}");
    }
}