using UnityEngine;

public class PlanePositions : MonoBehaviour
{
    private int[,] _planePoints = new int[10, 10];

    public Vector3 GetPlanePoint(Vector3 point)// всё работает корректно
    {
        var planePoint = transform.InverseTransformPoint(point);

        var x = Mathf.FloorToInt(planePoint.x) + (int)(_planePoints.GetLength(0) / 2);
        var y = Mathf.FloorToInt(planePoint.z) - (int)(_planePoints.GetLength(1) / 2) + 1;

        // Debug.Log(x + " " + y);
        return new Vector3(x, 0, y);
    }
}
