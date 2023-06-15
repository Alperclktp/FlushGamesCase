using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private float followSpeed;
    [TabGroup("Options")][SerializeField] private Vector3 offset;

    [TabGroup("Options")][SerializeField][LabelText("Offset")][OnValueChanged("UpdateOffset")] private Vector3 offsetInspector;

    [TabGroup("References")][SerializeField] private Transform target;

    private void Awake()
    {
        UpdateOffset();
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, followSpeed * 60 * Time.deltaTime);

        transform.position = newPosition;
    }

    private void UpdateOffset()
    {
        offset = offsetInspector;
    }
}
