using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMovement : MonoBehaviour
{
    [Header("Hole Mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshColllider;

    Mesh mesh;
    List<int> Holevertices;
    List<Vector3> HoleVert_offsets;

    [Header("Hole Radius")]
    [SerializeField] float Radius;
    [SerializeField] Transform HoleCentre;

    int holeVerticesCount;

    float x,y;
    Vector3 touch;
    Vector3 TargetPos;
    [Space]
    [SerializeField] float MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Game_Script.IsMoving = false;
        Game_Script.IsGameOver = false;


        Holevertices = new List<int>();
        HoleVert_offsets = new List<Vector3>();
        
        mesh = meshFilter.mesh;

        FindHoleVertices();
    }

    // Update is called once per frame
    void Update()
    {
        Game_Script.IsMoving = Input.GetMouseButton(0);
        if(!Game_Script.IsGameOver && Game_Script.IsMoving)
        {
            // move hole centre
            MoveHole();
            // Update Holevertices
            
            UpdateHoleVerticePos();
        }
    }

    void MoveHole()
    {
        x = Input.GetAxis ("Mouse X");
		y = Input.GetAxis ("Mouse Y");

        touch = Vector3.Lerp(HoleCentre.position,HoleCentre.position + new Vector3(x,0f,y),MoveSpeed * Time.deltaTime);

        HoleCentre.position = touch;
    }

    void UpdateHoleVerticePos()
    {
        Vector3[] vertices = mesh.vertices;
        for(int i = 0; i<holeVerticesCount;i++)
        {
            vertices [Holevertices [i]] = HoleCentre.position + HoleVert_offsets[i];
        }

        // Update mesh

        mesh.vertices = vertices;

        meshFilter.mesh = mesh;
        
        meshColllider.sharedMesh =mesh;
    }

    void FindHoleVertices()
    {
        for(int i= 0;i<mesh.vertices.Length;i++)
        {
            float Dist = Vector3.Distance(HoleCentre.position, mesh.vertices[i]);

            if(Dist<Radius)
            {
                Holevertices.Add(i);

                HoleVert_offsets.Add(mesh.vertices[i]-HoleCentre.position);
            }
        }

        holeVerticesCount = Holevertices.Count;
    }

    void OnDrawGizmos() 
    {
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(HoleCentre.position,Radius);
        }
    }

}
