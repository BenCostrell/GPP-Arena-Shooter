using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : Scene<TransitionData> {

    public Vector3 playerSpawn;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal override void OnEnter(TransitionData data)
    {
        PlayerController player = Instantiate(Services.PrefabDB.Player, playerSpawn, 
            Quaternion.identity, transform).GetComponent<PlayerController>();
        player.inTitleScreen = true;

    }

    public void StartGame()
    {
        Services.SceneStackManager.Swap<MainGame>();
    }
}
