using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    float sizeX = 100;
    float sizeY = 100;
    float maxHeight = 50;
    public GameObject cube;
    int vertCount = 0;
    public Material classicMaterial;

    CombineInstance[] combine;
    MeshFilter[] filters;

    // Use this for initialization
    void Start () {

        int xOffset = Random.Range(0,1000);
        int yOffset = Random.Range(0, 1000);

		
        for(int i = 0; i<=sizeX; i++)
        {
            for(int j=0; j<=sizeY; j ++)
            {
                GameObject g = Instantiate(cube, new Vector3(i,Mathf.Round(Mathf.PerlinNoise((float)i/sizeX + xOffset, (float)j/sizeY + yOffset)*maxHeight), j), Quaternion.identity);
                g.transform.parent = this.transform;
            }
        }

        filters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combine = new List<CombineInstance>();
        List<List<CombineInstance>> combineList = new List<List<CombineInstance>>();

        int k = 0;

        for(int i = 0; i < filters.Length; i++)
        {
            if(filters[i].mesh != null){
                combine.Add(
                    new CombineInstance()
                    {
                       mesh = filters[i].sharedMesh,
                       transform = filters[i].transform.localToWorldMatrix,
                    });


                    vertCount += combine[k].mesh.vertexCount;
                    
                    k++;

                    if (vertCount > 65535)
                    {
                         combineList.Add(combine);
                         combine = new List<CombineInstance>();
                         vertCount = 0;
                         k = 0;
                    }
            }
                         
        }

        if(vertCount < 65535 && combineList.Count < 1)
        {
            combineList.Add(combine);
        }

        GameObject parentObject = new GameObject("Parent");
       foreach(List<CombineInstance> list in combineList)
        {
            GameObject g = new GameObject();
            g.transform.parent = parentObject.transform;
            MeshFilter mf = g.AddComponent<MeshFilter>();
            MeshRenderer mr = g.AddComponent<MeshRenderer>() ;

            mr.material = classicMaterial;

            CombineInstance[] co = list.ToArray();
            Mesh fmesh = new Mesh();
            fmesh.CombineMeshes(co);

            mf.sharedMesh = fmesh;
            g.AddComponent<MeshCollider>();
        }

       for(int i = 0; i < transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
