using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public readonly Vector3 shipPosition = new Vector3(-17.5f, 5.75f, -16.55f);

    void Start()
    {
        Destroy(this);
    }
}
