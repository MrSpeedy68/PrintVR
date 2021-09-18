using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [Range(2, 100)] public int length = 5;
    public GameObject cube;
    public GameObject player;
    public int detailScale = 8;
    public int noiseHeight = 3;
    public List<GameObject> blockList;
    private Vector3 startPos = Vector3.zero;
    private Hashtable cubePos;

    private int XPlayerMove => (int)(player.transform.position.x - startPos.x);
    private int ZPlayerMove => (int)(player.transform.position.z - startPos.z);

    private int XPlayerLocation => (int)Mathf.Floor(player.transform.position.x);
    private int ZPlayerLocation => (int)Mathf.Floor(player.transform.position.z);

    // Start is called before the first frame update
    void Start()
    {
        cubePos = new Hashtable();
        GenerateTerrain(length);
    }

    private void Update()
    {
        if(Mathf.Abs(XPlayerMove) >= 1 || Mathf.Abs(ZPlayerMove) >= 1)
        {
            GenerateTerrain(length);
        }
    }

    private void GenerateTerrain(int length)
    {
        for (int x = -length; x < length; x++) //Loop through x and z axis
        {
            for (int z = -length; z < length; z++)
            {
                Vector3 pos = new Vector3(x + XPlayerLocation, generateYNoise(x + XPlayerLocation, z + ZPlayerLocation, detailScale) * noiseHeight, z + ZPlayerLocation);

                    if(!cubePos.ContainsKey(pos))
                    {
                        GameObject blockInstance = Instantiate(cube, pos, Quaternion.identity, transform);

                        blockList.Add(blockInstance);

                        cubePos.Add(pos, blockInstance);
                    }
            }
        }
    }

/*    private void SpawnObject()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject toPlaceObject = Instantiate(cube, ObjectSpawnLocation(), Quaternion.identity);
        }
    }*/

/*    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockList.Count); //Random Position between 0 and amount of blocks
        Vector3 newpos = new Vector3(
            blockList[rndIndex].x,
            blockList[rndIndex].y + 1f,
            blockList[rndIndex].z
            );

        //blockPositions.RemoveAt(rndIndex); //Remove this index so that objects dont spawn at the same place
        return newpos;
    }*/

    private float generateYNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;
        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}
