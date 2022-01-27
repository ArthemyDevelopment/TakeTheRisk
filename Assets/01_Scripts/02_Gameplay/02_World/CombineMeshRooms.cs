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


    public void CombineMeshs(GameObject g)
    {
        Vector3 pos = MainConfiner.transform.position;
        Destroy(MainConfiner.GetComponent<BoxCollider>());
        MeshFilter g1Mesh = MainConfiner.GetComponent<MeshFilter>();
        MeshFilter g2Mesh = g.GetComponent<MeshFilter>();
        
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
        CC.m_BoundingVolume = MainConfiner.AddComponent<BoxCollider>();
        MainConfiner.AddComponent<BoxCollider>().isTrigger = true;
        MainConfiner.transform.position = pos;
        MainConfiner.gameObject.SetActive(true);


    }
}
