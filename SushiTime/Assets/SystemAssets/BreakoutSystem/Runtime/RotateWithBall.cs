using BreakoutSystem;
using UnityEngine;

public class RotateWithBall : MonoBehaviour
{
    [SerializeField]
    private const int adjustedAngle = 90;

    [SerializeField]
    private BallBehaviour ballBehaviour;

    private void Update()
    {
        if (ballBehaviour != null)
        {
            var fxAngle = ballBehaviour.CurrentAngle - adjustedAngle;
            transform.rotation = Quaternion.Euler(0, 0, fxAngle);
        }
    }
}
