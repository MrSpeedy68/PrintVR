using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject blackGameObject;

    public GameObject objectToSpawn;

    private int worldSizeX = 100, worldSizeY = 100;
    private int noiseHeight = 3;
    private float gridOffset = 1.1f;

    private List<Vector3> blockPositions = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < worldSizeX; x++) //Loop through x and z axis
        {
            for(int z = 0; z < worldSizeY; z++)
            {
                Vector3 pos = new Vector3(x * gridOffset,
                    generateNoise(x,z,8f) * noiseHeight,
                    z * gridOffset);

                GameObject block = Instantiate(blackGameObject, pos, Quaternion.identity) as GameObject;

                blockPositions.Add(block.transform.position); //Add block to the list

                block.transform.SetParent(this.transform);
            }
        }
        SpawnObject();
    }

    private void SpawnObject()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject toPlaceObject = Instantiate(objectToSpawn, ObjectSpawnLocation(), Quaternion.identity);
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockPositions.Count); //Random Position between 0 and amount of blocks

        Vector3 newpos = new Vector3(
            blockPositions[rndIndex].x,
            blockPositions[rndIndex].y + 1f,
            blockPositions[rndIndex].z
            );

        blockPositions.RemoveAt(rndIndex); //Remove this index so that objects dont spawn at the same place
        return newpos;
    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}
