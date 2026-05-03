using UnityEngine;
using TMPro;

public class GuardUIIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshPro indicatorText;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, 0);

    private GuardStateMachine _stateMachine;
    private Camera _camera;

    void Start()
    {
        _stateMachine = GetComponent<GuardStateMachine>();
        _camera = Camera.main;

        // 텍스트 오브젝트 자동 생성
        if (indicatorText == null)
        {
            GameObject textObj = new GameObject("Indicator");
            textObj.transform.SetParent(transform);
            textObj.transform.localPosition = offset;
            indicatorText = textObj.AddComponent<TextMeshPro>();
            indicatorText.alignment = TextAlignmentOptions.Center;
            indicatorText.fontSize = 5f;
        }
    }

    void Update()
    {
        // 카메라 향하게
        if (_camera != null)
            indicatorText.transform.rotation = _camera.transform.rotation;

        // 상태별 아이콘
        switch (_stateMachine.currentState)
        {
            case GuardStateMachine.GuardState.Patrol:
                indicatorText.text = "";
                break;
            case GuardStateMachine.GuardState.Suspicious:
                indicatorText.text = "<color=yellow>?</color>";
                break;
            case GuardStateMachine.GuardState.Alert:
                indicatorText.text = "<color=orange>!</color>";
                break;
            case GuardStateMachine.GuardState.Chase:
                indicatorText.text = "<color=red>!!</color>";
                break;
            case GuardStateMachine.GuardState.Search:
                indicatorText.text = "<color=orange>?!</color>";
                break;
        }
    }
}