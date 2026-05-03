using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float patrolSpeed = 3f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private GuardStateMachine _stateMachine;
    private FieldOfView _fov;
    private int _currentWaypoint = 0;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _stateMachine = GetComponent<GuardStateMachine>();
        _fov = GetComponent<FieldOfView>();

        if (waypoints.Length > 0)
            GoToWaypoint(0);
    }

    void Update()
    {
        switch (_stateMachine.currentState)
        {
            case GuardStateMachine.GuardState.Patrol:
            case GuardStateMachine.GuardState.Suspicious:
            case GuardStateMachine.GuardState.Alert:
                Patrol();
                break;
            case GuardStateMachine.GuardState.Chase:
                Chase();
                break;
            case GuardStateMachine.GuardState.Search:
                Search();
                break;
        }

        if (_animator != null)
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    void Patrol()
    {
        _agent.speed = patrolSpeed;

        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
                GoToWaypoint(_currentWaypoint);
            }
        }
        else
        {
            if (!_agent.pathPending && _agent.remainingDistance <= 0.3f)
            {
                _isWaiting = true;
                _waitTimer = waitTime;
            }
        }
    }

    void Chase()
    {
        _agent.speed = chaseSpeed;
        if (_fov.visibleTarget != null)
            _agent.SetDestination(_fov.visibleTarget.position);
    }

    private float _lookTimer = 0f;
    private float _lookDuration = 1.5f;
    private int _lookDirection = 1;

    void Search()
    {
        _agent.speed = patrolSpeed;

        // 마지막 위치로 이동
        if (Vector3.Distance(transform.position, _stateMachine.lastKnownPosition) > 1f)
        {
            _agent.SetDestination(_stateMachine.lastKnownPosition);
        }
        else
        {
            // 도착하면 좌우 둘러보기
            _agent.SetDestination(transform.position);
            _lookTimer += Time.deltaTime;

            if (_lookTimer >= _lookDuration)
            {
                _lookTimer = 0f;
                _lookDirection *= -1;
            }

            transform.Rotate(0, _lookDirection * 60f * Time.deltaTime, 0);
        }
    }

    private void GoToWaypoint(int index)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(waypoints[index].position, out hit, 1.0f, NavMesh.AllAreas);
        _agent.SetDestination(hit.position);
    }


}