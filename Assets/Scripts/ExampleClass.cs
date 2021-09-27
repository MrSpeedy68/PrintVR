// This script draws a debug line around mesh triangles
// as you move the mouse over them.
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    Camera cam;

    public float radius = 2f;
    [Range(0.5f, 5f)]
    public float deformationStrength = 2f;
    public GameObject sphere;
    public GameObject InputObj;

    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;
    void Start()
    {
        cam = GetComponent<Camera>();

        mesh = InputObj.GetComponent<MeshFilter>().mesh;

        vertices = mesh.vertices;
        triangles = mesh.triangles;

        MakeSpheres(mesh);
    }

    public void MakeSpheres(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;

        foreach (Vector3 v in verts)
        {
            Vector3 newPos = InputObj.transform.TransformPoint(v);

            var sp = Instantiate(sphere, newPos, Quaternion.identity);

            sp.transform.parent = InputObj.transform;
        }
    }

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

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return;

            Mesh mesh = meshCollider.sharedMesh;

            Transform hitTransform = hit.collider.transform;

            Vector3 vert = vertices[GetClosestVertex(hit, triangles)];
            vert = hitTransform.TransformPoint(vert);

            Debug.DrawRay(vert, Vector3.up, Color.red);

            //float smoothingFactor = 2f;
            //float force = deformationStrength / (1f + hit.point.sqrMagnitude);

            if (Input.GetMouseButton(0))
            {
                //for (int i = 0; i < vertices.Length; i++)
                //{
                    vertices[GetClosestVertex(hit, triangles)] += Vector3.up * 1 * Time.deltaTime;
                //}
            }
            else if (Input.GetMouseButton(1))
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += Vector3.up * 1 * Time.deltaTime;
                }
            }

        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        //MakeSpheres(mesh);
    }
}
