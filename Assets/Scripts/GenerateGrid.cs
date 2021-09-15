using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject blackGameObject;

    private int worldSizeX = 100, worldSizeY = 100;
    private int noiseHeight = 5
        ;
    private float gridOffset = 1.1f;

    
    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < worldSizeX; x++)
        {
            for(int z = 0; z < worldSizeY; z++)
            {
                Vector3 pos = new Vector3(x * gridOffset,
                    generateNoise(x,z,8f) * noiseHeight,
                    z * gridOffset);
                GameObject block = Instantiate(blackGameObject, pos, Quaternion.identity) as GameObject;

                block.transform.SetParent(this.transform);
            }
        }    
    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}
