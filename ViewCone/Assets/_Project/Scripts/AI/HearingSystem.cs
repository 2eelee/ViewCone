using UnityEngine;
using UnityEngine.InputSystem;

public class HearingSystem : MonoBehaviour
{
    [SerializeField] private float walkSoundRadius = 0f;
    [SerializeField] private float runSoundRadius = 8f;

    private PlayerController _playerController;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        bool isRunning = Keyboard.current.leftShiftKey.isPressed;
        float soundRadius = isRunning ? runSoundRadius : walkSoundRadius;

        if (soundRadius > 0f)
            EmitSound(soundRadius);
    }

    void EmitSound(float radius)
    {
        Collider[] guards = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Guard"));
        foreach (Collider col in guards)
        {
            GuardStateMachine guard = col.GetComponent<GuardStateMachine>();
            if (guard != null)
                guard.ReceiveAlert(transform.position, 2f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runSoundRadius);
    }
}