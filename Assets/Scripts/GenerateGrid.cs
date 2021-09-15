using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject blackGameObject;

    public int worldSizeX = 10, worldSizeY = 10, gridOffset = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < worldSizeX; x++)
        {
            for(int z = 0; z < worldSizeY; z++)
            {
                Vector3 pos = new Vector3(x * gridOffset,
                    0,
                    z * gridOffset);
                GameObject block = Instantiate(blackGameObject, pos, Quaternion.identity) as GameObject;

                block.transform.SetParent(this.transform);
            }
        }    
    }
}
