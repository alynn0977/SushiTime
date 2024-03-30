using BreakoutSystem;
using UnityEngine;

public class RotateWithBall : MonoBehaviour
{
    private const int adjustedAngle = 90;
    [SerializeField]
    private BallBehaviour ballBehaviour;
    [SerializeField]

    private void Update()
    {
        if (ballBehaviour != null)
        {
            Debug.LogWarning($"Ball is facing {ballBehaviour.CurrentAngle} degrees.");
            var fxAngle = ballBehaviour.CurrentAngle - adjustedAngle;
            transform.rotation = Quaternion.Euler(0, 0, fxAngle);
        }
    }
}
