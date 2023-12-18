using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera cam;

    public GameObject Player;
    public GameObject World;
    public Vector3 pos;
    public Vector3 rotation;
    float camOffset;
    float distance;
    private int layerWall;
    private Collider currCollider;

    // Start is called before the first frame update
    void Start()
    {
        distance = 5;
        camOffset = 40.0f;
        this.Player = GameObject.Find("Player");
        this.World = GameObject.Find("World");
        this.cam = Camera.main;
        this.cam.enabled = true;

        this.pos = new Vector3((this.Player.transform.position.x - camOffset), 5.0f, (this.Player.transform.position.z - camOffset));
        transform.position = this.pos;
        this.cam.orthographic = true;
        this.layerWall = LayerMask.NameToLayer("Wall");

    }

  

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = Input.mousePosition;
        transform.LookAt(this.Player.transform);
        transform.position = new Vector3(Player.transform.position.x - camOffset, 15.0f, this.Player.transform.position.z - camOffset);
        if (distance > 0 && distance < 10)
        {
            distance += Input.mouseScrollDelta.y;
            Camera.main.orthographicSize = distance;
        }
        if(distance <= 0)
        {
            distance = 1;
        }
        if (distance >= 10)
        {
            distance = 9;
        }
    }
    private void CheckCollision()
    {
        Vector3 PlayerPos = Player.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, PlayerPos, out hit, 24f, layerWall))
        {
            if (currCollider != null)
            {
                currCollider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 1));
            }
            currCollider = hit.collider;
            currCollider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1,1,1,0));
            Debug.DrawRay(cam.transform.position, PlayerPos * hit.distance, Color.green);
        }

    }
}
