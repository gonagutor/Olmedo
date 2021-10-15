using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public LayerMask whatIsTeleportable;
    public Teleporter destination;
    public Vector2 destinationOffset = new Vector2(1, 0);
    public Vector2 size = new Vector2(1, 4);
    public bool horizontal;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(
            transform.position.x + destinationOffset.x, 
            transform.position.y + destinationOffset.y,
            transform.position.z
        ), size);
    }

    void Update()
    {
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(transform.position, size / 2, 0f, Vector2.zero);
        for (int i = 0; i < raycastHits.Length; i++)
            destination.HandleTeleport(raycastHits[i].transform);
    }

    void HandleTeleport(Transform transform)
    {
        transform.position = new Vector3(
            this.transform.position.x + ((horizontal) ? destinationOffset.x : 0),
            this.transform.position.y + ((horizontal) ? 0 : destinationOffset.y),
            this.transform.position.z
        );
    }
}
