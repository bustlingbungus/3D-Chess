using UnityEngine;
using UnityEngine.InputSystem;
using Defs;
using Unity.VisualScripting;
using System.Collections.Generic;

/// <summary>
/// <para>Script to help control the movement of the camera.
/// Ensures the camera is always looking at the centre of the board.</para>
/// 
/// <para>Stores a directed graph whose vertices contain different viewing positions around the board. 
/// Interpolates the camera between these position vertices based on user input.</para>
/// </summary>
public class CameraMovement : MonoBehaviour
{
    /* ==========  MEMBERS  ========== */

    private InputManager input;
    
    /// <summary> The amount of time the camera takes to transition between viewing locations. </summary>
    [SerializeField]
    private float travelTime = 0.25f;

    /// <summary> Directed graph containing all viewing locations for the camera. </summary>
    private AdjacencyGraph<CameraNode> graph;
    /// <summary> The graph vertex (viewing location) the camera is currently positioned at. </summary>
    private CameraNode curr_node;

    /// <summary> The position of the camera's previous viewing location, used for interpolation. </summary>
    private Vector3 prev_pos;
    /// <summary> Timer used for interpolation between the previous location and the current one. </summary>
    private float move_timer;


    /* ==========  PROPERTIES  ========== */

    /// <summary>
    /// <para><b>get:</b> The graph vertex (viewing location) the camera is currently positioned at. </para>
    /// <para><b>set:</b> Assigns current node, and sets up <c>prev_pos</c> and <c>move_timer</c> to begin interpolation.</para>
    /// </summary>
    private CameraNode Node
    {
        get => curr_node;
        set {
            if (value!=null) {
                prev_pos = curr_node.position;
                move_timer = 0f;
                curr_node = value;
            }
        }
    }


    /* ==========  MAIN FUNCTIONS  ========== */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();

        // initialise the vertex graph and add all the viewing locations.
        graph = new AdjacencyGraph<CameraNode>();

        graph.AddVertex(new CameraNode(new Vector3(0,0,-25),    0));    // 0    front face
        graph.AddVertex(new CameraNode(new Vector3(25,0,0),     1));    // 1    right face
        graph.AddVertex(new CameraNode(new Vector3(0,0,25),     2));    // 2    back face
        graph.AddVertex(new CameraNode(new Vector3(-25,0,0),    3));    // 3    left face

        graph.AddVertex(new CameraNode(new Vector3(0,25,-1),    4));    // 4    top, front down
        graph.AddVertex(new CameraNode(new Vector3(1,25,0),     5));    // 5    top, right down
        graph.AddVertex(new CameraNode(new Vector3(0,25,1),     6));    // 6    top, back down
        graph.AddVertex(new CameraNode(new Vector3(-1,25,0),    7));    // 7    top, left down

        graph.AddVertex(new CameraNode(new Vector3(0,-25,-1),   8));    // 8    bottom, front down
        graph.AddVertex(new CameraNode(new Vector3(1,-25,0),    9));    // 9    bottom, right down
        graph.AddVertex(new CameraNode(new Vector3(0,-25,1),    10));   // 10   bottom, back down
        graph.AddVertex(new CameraNode(new Vector3(-1,-25,0),   11));   // 11   bottom, left down

        // front face connections
        graph.AddEdge(0, 1, GraphDirection.Right);  // right faace
        graph.AddEdge(0, 3, GraphDirection.Left);  // left face
        graph.AddEdge(0, 4, GraphDirection.Up);  // top face (front down)
        graph.AddEdge(0, 8, GraphDirection.Down);  // bottom face (front down)
        // right face
        graph.AddEdge(1, 0, GraphDirection.Left);  // front faace
        graph.AddEdge(1, 2, GraphDirection.Right);  // back face
        graph.AddEdge(1, 5, GraphDirection.Up);  // top face (right down)
        graph.AddEdge(1, 9, GraphDirection.Down);  // bottom face (right down)
        // back face
        graph.AddEdge(2, 1, GraphDirection.Left);  // right faace
        graph.AddEdge(2, 3, GraphDirection.Right);  // left face
        graph.AddEdge(2, 6, GraphDirection.Up);  // top face (back down)
        graph.AddEdge(2, 10, GraphDirection.Down);  // bottom face (back down)
        // left face
        graph.AddEdge(3, 0, GraphDirection.Right);  // front faace
        graph.AddEdge(3, 2, GraphDirection.Left);  // back face
        graph.AddEdge(3, 7, GraphDirection.Up);  // top face (left down)
        graph.AddEdge(3, 11, GraphDirection.Down);  // bottom face (left down)

        // top face (front down)
        graph.AddEdge(4, 0, GraphDirection.Down);  // front face
        graph.AddEdge(4, 5, GraphDirection.Right);  // top (right down)
        graph.AddEdge(4, 7, GraphDirection.Left);  // top face (left down)
        // graph.AddEdge(4, 6, GraphDirection.Up);  // top face (back down)
        // top face (right down)
        graph.AddEdge(5, 1, GraphDirection.Down);  // right face
        graph.AddEdge(5, 6, GraphDirection.Right);  // top (back down)
        graph.AddEdge(5, 4, GraphDirection.Left);  // top face (front down)
        // graph.AddEdge(5, 7, GraphDirection.Up);  // top face (left down)
        // top face (back down)
        graph.AddEdge(6, 2, GraphDirection.Down);  // back face
        graph.AddEdge(6, 7, GraphDirection.Right);  // top (left down)
        graph.AddEdge(6, 5, GraphDirection.Left);  // top face (right down)
        // graph.AddEdge(6, 4, GraphDirection.Up);  // top face (front down)
        // top face (left down)
        graph.AddEdge(7, 3, GraphDirection.Down);  // left face
        graph.AddEdge(7, 4, GraphDirection.Right);  // top (front down)
        graph.AddEdge(7, 6, GraphDirection.Left);  // top face (back down)
        // graph.AddEdge(7, 5, GraphDirection.Up);  // top face (right down)
        
        // bottom face (front down)
        graph.AddEdge(8, 0, GraphDirection.Up);  // front face
        graph.AddEdge(8, 9, GraphDirection.Right);  // bottom (right down)
        graph.AddEdge(8, 11, GraphDirection.Left);  // bottom face (left down)
        // graph.AddEdge(8, 10, GraphDirection.Up);  // bottom face (back down)
        // bottom face (right down)
        graph.AddEdge(9, 1, GraphDirection.Up);  // right face
        graph.AddEdge(9, 10, GraphDirection.Right);  // bottom (back down)
        graph.AddEdge(9, 8, GraphDirection.Left);  // bottom face (front down)
        // graph.AddEdge(9, 11, GraphDirection.Up);  // bottom face (left down)
        // bottom face (back down)
        graph.AddEdge(10, 2, GraphDirection.Up);  // back face
        graph.AddEdge(10, 11, GraphDirection.Right);  // bottom (left down)
        graph.AddEdge(10, 9, GraphDirection.Left);  // bottom face (right down)
        // graph.AddEdge(10, 8, GraphDirection.Up);  // bottom face (front down)
        // bottom face (left down)
        graph.AddEdge(11, 3, GraphDirection.Up);  // left face
        graph.AddEdge(11, 8, GraphDirection.Right);  // bottom (front down)
        graph.AddEdge(11, 10, GraphDirection.Left);  // bottom face (back down)
        // graph.AddEdge(11, 9, GraphDirection.Up);  // bottom face (right down)

        // initilaise camera position
        curr_node = graph.GetVertexAt(0);
        prev_pos = transform.position = curr_node.position;
    }

    // Update is called once per frame
    void Update()
    {
        // move to the next node in the direction selected by user input, if any
        if (input.PanUp)    Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Up);
        else if (input.PanDown)  Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Down);
        else if (input.PanLeft)  Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Left);
        else if (input.PanRight) Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Right);

        // move to the position of the new node by interpolation
        float t = move_timer / travelTime;
        transform.position = interp(prev_pos, Node.position, t);
        // ensure the camera is looking at the centre of the board
        transform.LookAt(Vector3.zero);
        
        // update the timer used for interpolation
        move_timer = Mathf.Clamp(move_timer+Time.deltaTime, 0f, travelTime);
    }


    /* ==========  HELPER FUNCTIONS  ========== */

    // linear interpolation for Vector3
    private Vector3 interp(Vector3 a, Vector3 b, float t) {
        return a + t*(b - a);
    }
}
