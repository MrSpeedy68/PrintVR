// This script draws a debug line around mesh triangles
// as you move the mouse over them.
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    Camera cam;

    public float radius = 2f; //Radius of the affected area
    [Range(0.1f, 5f)]
    public float deformationStrength = 2f; //Deformation strength
    [Range(0.5f, 10f)]
    public float smoothingFactor = 2f; //How much the deformation will get smoothened out
    public bool vertexVisualization = true;
    public GameObject sphere; //Object to represent vertices
    public GameObject InputObj; //Object to be manipulated

    Vector3[] vertices, deformedVerts; //Array of mesh vertices
    int[] triangles, deformedTris; //Array of mesh triangles

    Mesh mesh;
    GameObject[] spheres;

    void Start()
    {
        //Set mesh and the vert and tris arrays
        mesh = InputObj.GetComponent<MeshFilter>().mesh; 

        vertices = mesh.vertices;
        deformedVerts = mesh.vertices;

        triangles = mesh.triangles;
        deformedTris = mesh.triangles;

        //Make new array for amount of verts to set them as spheres
        spheres = new GameObject[vertices.Length];

        MakeSpheres();
    }

    //Method to make spheres on vert points
    public void MakeSpheres()
    { 
        if(vertexVisualization)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 newPos = InputObj.transform.TransformPoint(vertices[i]);

                spheres[i] = Instantiate(sphere, newPos, Quaternion.identity);

                spheres[i].transform.parent = InputObj.transform;
            }
        }
    }

    //Method to update position of vert points of spheres
    public void UpdateSpheres(Vector3[] verPos)
    {
        for (int i = 0; i < spheres.Length; i++)
        {
            Vector3 newPos = InputObj.transform.TransformPoint(verPos[i]);

            spheres[i].transform.position = newPos;
        }
    }

    //Method to get the index of the closet vertice depending on Raycast Hit using the triangles of the mesh.
    //Detects what triangle the user hit and does calculation to determine what vertice is closest to the hit point on the give triangle.
    public static int GetClosestVertex(RaycastHit aHit, int[] aTriangles)
    {
        var b = aHit.barycentricCoordinate;
        int index = aHit.triangleIndex * 3;
        if (aTriangles == null || index < 0 || index + 2 >= aTriangles.Length)
            return -1;
        if (b.x > b.y)
        {
            if (b.x > b.z)
                return aTriangles[index]; // x
            else
                return aTriangles[index + 2]; // z
        }
        else if (b.y > b.z)
            return aTriangles[index + 1]; // y
        else
            return aTriangles[index + 2]; // z
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) //Raycast from mouse point to screen
        {
            MeshCollider meshCollider = hit.collider as MeshCollider; //Check to make sure a mesh was hit
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return;

            Transform hitTransform = hit.collider.transform;

            Vector3 vert = deformedVerts[GetClosestVertex(hit, deformedTris)]; //Gets the closest vert to the hit point
            vert = hitTransform.TransformPoint(vert); //Converts transform to world position

            Debug.DrawRay(vert, Vector3.up, Color.red); //Draw ray on vert point

            //Manipulate mesh on mouse button click
            if (Input.GetMouseButton(0))
            {
                print(deformedVerts[GetClosestVertex(hit, deformedTris)].sqrMagnitude);
                deformedVerts[GetClosestVertex(hit, deformedTris)] += (hit.normal * deformationStrength) / smoothingFactor;

            }
            else if (Input.GetMouseButton(1))
            {   
                deformedVerts[GetClosestVertex(hit, deformedTris)] += (hit.normal * -deformationStrength) / smoothingFactor;
            }

        }
        mesh.vertices = deformedVerts; //Set new verts as mesh verts
        mesh.RecalculateBounds(); //Recalculate the mesh bounds
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        if (vertexVisualization)
        {
            UpdateSpheres(deformedVerts); //Update the spheres to new vert positions
        }

    }
}
