using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float velocidad;
    private Transform transform;
    private Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        transform = this.GetComponent<Transform>();
        pos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (pos.x < transform.position.x) {
            transform.position += Vector2. * 10;
        }
    }
}
