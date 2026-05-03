using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI infoText;

    private bool _godMode = false;
    private float _timeScale = 1f;
    private float _deltaTime = 0f;

    void Update()
    {
        // F1: God 모드
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            _godMode = !_godMode;
            Debug.Log($"God Mode: {_godMode}");
        }

        // F4: 시간 배속
        if (Keyboard.current.f4Key.wasPressedThisFrame)
        {
            _timeScale = _timeScale >= 5f ? 0.5f : _timeScale * 2f;
            Time.timeScale = _timeScale;
            Debug.Log($"TimeScale: {_timeScale}");
        }

        // FPS 계산
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

        if (infoText != null)
        {
            float fps = 1.0f / _deltaTime;
            infoText.text = $"FPS: {fps:0}\nGod: {_godMode}\nSpeed: {_timeScale}x";
        }
    }

    public bool IsGodMode() => _godMode;
}