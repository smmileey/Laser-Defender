using UnityEngine;

public class MovementController : MonoBehaviour {

    public Vector3 MoveHorizontally(HorizontalMovementInfo horizontalMovementInfo)
    {
        Vector3 vectorDirection = horizontalMovementInfo.MoveRight ? Vector3.right : Vector3.left;
        horizontalMovementInfo.GameObject.transform.position += vectorDirection * horizontalMovementInfo.SpeedFactor * Time.deltaTime;
        float restrictedXPositon = Mathf.Clamp(horizontalMovementInfo.GameObject.transform.position.x, horizontalMovementInfo.LeftBoundary, horizontalMovementInfo.RightBoundary);
        horizontalMovementInfo.GameObject.transform.position = new Vector3(restrictedXPositon, horizontalMovementInfo.GameObject.transform.position.y,
            horizontalMovementInfo.GameObject.transform.position.z);
        return horizontalMovementInfo.GameObject.transform.position;
    }
}
