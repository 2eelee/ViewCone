using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class FieldOfViewVisualizer : MonoBehaviour
{
    [SerializeField] private int rayCount = 50;
    [SerializeField] private Material fovMaterial;

    [Header("상태별 색상")]
    [SerializeField] private Color patrolColor = new Color(1f, 1f, 1f, 0.3f);
    [SerializeField] private Color suspiciousColor = new Color(1f, 1f, 0f, 0.4f);
    [SerializeField] private Color alertColor = new Color(1f, 0.5f, 0f, 0.5f);
    [SerializeField] private Color chaseColor = new Color(1f, 0f, 0f, 0.6f);

    private GuardStateMachine _stateMachine;

    private FieldOfView _fov;
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    void Start()
    {
        _fov = GetComponent<FieldOfView>();

        GameObject fovObj = new GameObject("FOVMesh");
        fovObj.transform.SetParent(transform);
        fovObj.transform.localPosition = Vector3.zero;
        fovObj.transform.localRotation = Quaternion.identity;

        _meshFilter = fovObj.AddComponent<MeshFilter>();
        _meshRenderer = fovObj.AddComponent<MeshRenderer>();
        _meshRenderer.material = fovMaterial;

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

        _stateMachine = GetComponent<GuardStateMachine>();
    }

    void LateUpdate()
    {
        UpdateFOVColor();
        DrawFOV();
    }

    void DrawFOV()
    {
        float angleStep = _fov.viewAngle / rayCount;
        float currentAngle = -_fov.viewAngle / 2f;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = _fov.DirFromAngle(currentAngle, false);
            RaycastHit hit;
            Vector3 point;

            if (Physics.Raycast(transform.position, dir, out hit, _fov.viewRadius, _fov.obstacleMask))
                point = transform.InverseTransformPoint(hit.point);
            else
                point = transform.InverseTransformPoint(transform.position + dir * _fov.viewRadius);

            point.y = 0;
            vertices[i + 1] = point;

            if (i < rayCount)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            currentAngle += angleStep;
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

    void UpdateFOVColor()
    {
        if (_stateMachine == null) return;

        Color c = _stateMachine.currentState switch
        {
            GuardStateMachine.GuardState.Patrol => patrolColor,
            GuardStateMachine.GuardState.Suspicious => suspiciousColor,
            GuardStateMachine.GuardState.Alert => alertColor,
            GuardStateMachine.GuardState.Chase => chaseColor,
            _ => patrolColor
        };

        _meshRenderer.material.color = c;
    }

}