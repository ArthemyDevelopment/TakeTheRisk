using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;

public class CombineMeshRooms : MonoBehaviour
{
    public static CombineMeshRooms current;
    
    [SerializeField]private GameObject MainConfiner;
    [SerializeField]private CinemachineConfiner CC;

    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current != this)
            Destroy(this);
    }


    
    //Metodo para combinar dos mesh y crear un nuevo espacio de confine para la CinemachineCamera, toma como
    //parametro el mesh que se va a incorporar (dado por lo general desde el script TriggerDoor) y lo combina
    //al mesh de MainConfiner, que es el
    public void CombineMeshs(GameObject g)
    {
        Vector3 pos = MainConfiner.transform.position;
        pos.y = 0;
        Destroy(MainConfiner.GetComponent<BoxCollider>());
        MeshFilter g1Mesh = MainConfiner.GetComponent<MeshFilter>();
        MeshFilter g2Mesh = g.GetComponent<MeshFilter>();
        MeshCollider mainColl = MainConfiner.GetComponent<MeshCollider>();
        
        List<MeshFilter> meshs = new List<MeshFilter>();
        meshs.Add(g1Mesh);
        meshs.Add(g2Mesh);

        CombineInstance[] combine = new CombineInstance[meshs.Count];

        for (int i = 0; i < meshs.Count; i++)
        {
            combine[i].mesh = meshs[i].sharedMesh;
            combine[i].transform = meshs[i].transform.localToWorldMatrix;
            meshs[i].gameObject.SetActive(false);
        }

        g1Mesh.mesh = new Mesh();
        g1Mesh.mesh.CombineMeshes(combine, true, true);
        MainConfiner.transform.localScale = new Vector3(1, 1, 1);
        mainColl.sharedMesh = g1Mesh.mesh;
        mainColl.convex = true;
        mainColl.isTrigger = true;
        CC.m_BoundingVolume = MainConfiner.GetComponent<MeshCollider>();
        MainConfiner.transform.position = pos;
        MainConfiner.gameObject.SetActive(true);


    }
}
