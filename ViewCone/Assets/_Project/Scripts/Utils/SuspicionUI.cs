using UnityEngine;
using UnityEngine.UI;

public class SuspicionUI : MonoBehaviour
{
    [SerializeField] private Slider suspicionBar;
    [SerializeField] private GuardStateMachine guardStateMachine;
    [SerializeField] private Image fillImage;

    void Update()
    {
        if (guardStateMachine == null) return;

        suspicionBar.value = guardStateMachine.suspicionLevel;

        // 상태별 색상
        switch (guardStateMachine.currentState)
        {
            case GuardStateMachine.GuardState.Patrol:
                fillImage.color = Color.white;
                break;
            case GuardStateMachine.GuardState.Suspicious:
                fillImage.color = Color.yellow;
                break;
            case GuardStateMachine.GuardState.Alert:
                fillImage.color = new Color(1f, 0.5f, 0f);
                break;
            case GuardStateMachine.GuardState.Chase:
            case GuardStateMachine.GuardState.Search:
                fillImage.color = Color.red;
                break;
        }
    }
}