using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float velocity = 5;

    public Rigidbody rigidBody;
    public Vector3 moveDirection;
    public float horizontalIn;
    public float verticalIn;
    public Transform orientation;
    public GameObject floor;
    public Vector3 size;
    public int layerGround;
    public int layerWall;
    public int rayQtd;
    public bool[] rays;



    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.Find("Floor");
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        size = GetComponent<Collider>().bounds.size;
        layerGround = LayerMask.NameToLayer("Ground");
        layerWall = LayerMask.NameToLayer("Wall");
        rayQtd = 360;
        rays = new bool[rayQtd];
        orientation = transform;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void InputPlayer()
    {
        horizontalIn = Input.GetAxis("Horizontal");
        verticalIn = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * (verticalIn - horizontalIn) + orientation.right * ((horizontalIn + verticalIn));
    
        rigidBody.AddForce(moveDirection.normalized * velocity * 7f, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        // check if coliding with the ground
        bool grounded = Physics.Raycast(transform.position, Vector3.down, size.y * 0.5f + 0.2f, layerGround);

        InputPlayer();
        LimitSpeed();
        UpdateRays();

        // handle drag
        if (grounded)
            rigidBody.drag = 5;
        else
            rigidBody.drag = 0;

        // handle running and walking
        if (Input.GetKey(KeyCode.LeftShift))
            velocity = 14f;
        else
            velocity = 7f;
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        if(flatVel.magnitude > velocity)
        {
            Vector3 limitedVel = flatVel.normalized * velocity;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

    private void UpdateRays()
    {
        float angle = 0;
        for(int i = 0; i < rayQtd; i++)
        {
            RaycastHit hit;
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2f * Mathf.PI / rayQtd;
            Vector3 rotation = new Vector3(x, 0, z);
            if(!Physics.Raycast(transform.position, rotation * 24f, out hit, 24f, layerWall))
                Debug.DrawRay(transform.position, rotation * 24f, Color.red);
            else
            {
               Debug.DrawRay(transform.position, rotation * hit.distance, Color.green);
            }
        }
    }
}
