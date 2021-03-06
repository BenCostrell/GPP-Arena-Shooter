﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Prefab DB")]
public class PrefabDB : ScriptableObject {
	[SerializeField]
	private GameObject enemy;
	public GameObject Enemy { get { return enemy; } }

	[SerializeField]
	private GameObject player;
	public GameObject Player { get { return player; } }

	[SerializeField]
	private GameObject bullet;
	public GameObject Bullet { get { return bullet; } }

	[SerializeField]
	private GameObject bossBullet;
	public GameObject BossBullet { get { return bossBullet; } }

    [SerializeField]
    private GameObject[] scenes;
    public GameObject[] Scenes { get { return scenes; } }
}
