using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[System.Serializable]
public class Hex : MonoBehaviour {

    public int id = 0;

    public SpriteRenderer hexRend;
    public MeshFilter hexTopMeshFilter;
    public MeshRenderer hexTopMeshRenderer;

    public Mesh[] hexTopMeshes;
    public Material[] hexTopMeshMaterials;
    public GameObject trees;

    public int resNumber = 0;
    public bool desert;
    public bool hasRobbers;

    public int number = 0;
    public TextMesh numberMesh;

    public List<Town> towns = new List<Town>();

    //ивенты. когда выпадает число хекса
    public delegate void HexEvent();
    public HexEvent scoreHex;


    // Use this for initialization
    void Awake () {

	}

    public void ScoreHex()
    {
        if (!hasRobbers)

            foreach(Town t in towns)
            {
                if (!t.empty)
                {
                    t.ScoreTown(resNumber);
                }
            }
    }

    public void ClearHexSelection()
    {
        hexRend.color = Color.white;
    }

    public void SelectHex()
    {
        hexRend.color = Color.cyan;
    }

    public void SetupHex(int resNumT, int numT)
    {
        resNumber = resNumT;

        number = numT;
        if (number != 7)
            numberMesh.text = number.ToString();

        if (number == 6 || number == 8)
            numberMesh.color = Color.red;

        hexTopMeshFilter.mesh = hexTopMeshes[resNumber];
        hexTopMeshRenderer.material = hexTopMeshMaterials[resNumber];
        if (resNumber == 0) trees.SetActive(true);

    }

    public void SetDesert()
    {
        hexTopMeshFilter.mesh = hexTopMeshes[5];
        hexTopMeshRenderer.material = hexTopMeshMaterials[5];
    }
}


