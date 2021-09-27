using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class testSculpt : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                Vector3 HitPosition = hit.point;

                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 forward = Vector3.up;
                    Debug.DrawRay(HitPosition, forward * 1000, Color.green);

                    var worldPos = transform.TransformPoint(vertices[i]);
                    Vector3 t = vertices[i];
                    //vertices[i] += Vector3.up * 10 * Time.deltaTime;
                    if (HitPosition.x <= worldPos.x + 1 && HitPosition.x >= worldPos.x - 1 && HitPosition.z <= worldPos.z + 1 && HitPosition.z >= worldPos.z - 1)
                    {
                        vertices[i] += Vector3.up * 1 * Time.deltaTime;

                    }
                }


            }
        }

        if (Input.GetKey(KeyCode.L))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                Vector3 HitPosition = hit.point;

                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 forward = Vector3.up;
                    Debug.DrawRay(HitPosition, forward * 1000, Color.green);

                    var worldPos = transform.TransformPoint(vertices[i]);
                    Vector3 t = vertices[i];
                    //vertices[i] += Vector3.up * 10 * Time.deltaTime;
                    if (HitPosition.x <= worldPos.x + 1 && HitPosition.x >= worldPos.x - 1 && HitPosition.z <= worldPos.z + 1 && HitPosition.z >= worldPos.z - 1)
                    {
                        vertices[i] += Vector3.down * 1 * Time.deltaTime;

                    }
                }


            }
        }


        for (var i = 0; i < vertices.Length; i++)
        {
            //vertices[i] += Vector3.up * Time.deltaTime;

        }

        // assign the local vertices array into the vertices array of the Mesh.
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}
