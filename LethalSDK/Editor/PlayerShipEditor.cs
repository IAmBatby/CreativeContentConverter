using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerShip))]
public class PlayerShipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerShip lockPosition = (PlayerShip)target;
        if (lockPosition.transform.position != lockPosition.shipPosition)
        {
            lockPosition.transform.position = lockPosition.shipPosition;
        }
    }
}
