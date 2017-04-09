using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : Scene<TransitionData> {

    public GameObject player;
    public bool gameOver;
    public int score;
    public GameObject scoreText;
    public GameObject bossBattleMessage;
    public GameObject bossHealthBack;
    public GameObject bossHealthBar;
    public float setupDuration;
    public float appearanceDuration;
    public float endingDuration;
    public Vector3 playerPositionBeforeBossBattle;

    // Use this for initialization
    void Start () {
        
    }

    internal override void OnEnter(TransitionData data)
    {
        gameOver = false;
        Services.EventManager.Register<EnemyDied>(Score);
        bossBattleMessage.SetActive(false);
        bossHealthBack.SetActive(false);
        Services.MainGame = this;
        Services.EnemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();

        InitializePlayer();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void EndGame()
    {
        Services.EventManager.Fire(new GameOver());
        Services.SceneStackManager.PushScene<GameOverScreen>(new TransitionData(score));
    }

    private void InitializePlayer()
    {
        player = Instantiate(Services.PrefabDB.Player, Services.MainGame.transform) as GameObject;
    }

    public void Score(EnemyDied e)
    {
        score += e.enemyThatDied.pointValue;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    public void ItsBossTime()
    {
        BossSetup setup = new BossSetup();
        BossAppearance appearance = new BossAppearance();
        BossSpawnMode spawnMode = new BossSpawnMode();
        BossFireMode fireMode = new BossFireMode();
        BossChaseMode chaseMode = new BossChaseMode();
        BossEnding ending = new BossEnding();

        setup
            .Then(appearance)
            .Then(spawnMode)
            .Then(fireMode)
            .Then(chaseMode)
            .Then(ending);

        Services.TaskManager.AddTask(setup);
    }
}
