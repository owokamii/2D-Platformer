using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private float smoothTime;
    private Vector3 velocity = Vector3.zero;
    private Vector3 offset = new Vector3(0f, 0f, -10f);

    [SerializeField] private Transform target;

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
