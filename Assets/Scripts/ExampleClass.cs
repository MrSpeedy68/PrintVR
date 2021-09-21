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

    void Start()
    {
        cam = GetComponent<Camera>();

        Mesh mesh = InputObj.GetComponent<MeshFilter>().mesh;

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
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            Transform hitTransform = hit.collider.transform;

            Vector3 vert = vertices[GetClosestVertex(hit, triangles)];
            vert = hitTransform.TransformPoint(vert);

            Debug.DrawRay(vert, Vector3.up, Color.red);

            float smoothingFactor = 2f;
            float force = deformationStrength / (1f + hit.point.sqrMagnitude);

            if (Input.GetMouseButton(0))
            {
                vertices[GetClosestVertex(hit, triangles)] = new Vector3(vert.x, vert.y + 5f, vert.z);
            }
            else if (Input.GetMouseButton(1))
            {
                vertices[GetClosestVertex(hit, triangles)] = vertices[GetClosestVertex(hit, triangles)] + (Vector3.down * force) / smoothingFactor;
            }

        }

        /*RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Debug.DrawRay(p0, Vector3.up, Color.green);
        //Debug.DrawLine(p1, p2);
        //Debug.DrawLine(p2, p0);

        vertices[triangles[hit.triangleIndex * 3 + 0]] += Vector3.up * 5;*/
    }
}
