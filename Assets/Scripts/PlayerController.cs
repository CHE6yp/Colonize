using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {

    public Player player;
    public Player.PlayerEvent click;
    public int startClickCount=0;
    public bool placedTown;

    public bool switchTurnCheck; //temperary

	// Use this for initialization
	void Start () {
        click = Click2;
	}
	
	// Update is called once per frame
	void Update () {

        if (!player.isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            if (player.isServer)
            {
                player.Cmd_StartGame();
            }


        if (!player.turn)
            return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            switchTurnCheck = true;
        }



        if (Input.GetMouseButtonDown(0))
            click();

	}

    public void Click()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        //Ray2D ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (hit.collider !=null)
        {
            GameObject objectHit = hit.transform.gameObject;

            if (objectHit.GetComponent<Town>() != null)
                player.ManageTown(objectHit.GetComponent<Town>().id);
             
            if (objectHit.GetComponent<Road>() != null)
                player.Cmd_BuildRoad(objectHit.GetComponent<Road>().id);

        }
    }

    public void Click2()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        //Ray2D ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (hit.collider != null)
        {
            GameObject objectHit = hit.transform.gameObject;

            if (!placedTown && objectHit.GetComponent<Town>() != null)

                player.Cmd_BuildTownWithoutAllow(objectHit.GetComponent<Town>().id);


            if (placedTown && objectHit.GetComponent<Road>() != null)
            {
                player.Cmd_BuildRoadBool(objectHit.GetComponent<Road>().id);
                
            }
        }
    }
}
