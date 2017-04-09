using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Services {
	public static EventManager EventManager { get; set; }
	public static EnemyManager EnemyManager { get; set; }
	public static GameManager GameManager { get; set; }
	public static PrefabDB PrefabDB { get; set; }
	public static TaskManager TaskManager { get; set; }
    public static SceneStackManager<TransitionData> SceneStackManager { get; set; }
    public static MainGame MainGame { get; set; }
}
