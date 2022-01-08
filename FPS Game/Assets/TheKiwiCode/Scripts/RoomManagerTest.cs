using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManagerTest : MonoBehaviour
{
    public static RoomManagerTest Instance;
    PhotonView photonView;


	public GameObject spawnManager;
	Transform[] spawnPoints;

	public GameObject PlayerController;
    private void Start()
    {
		photonView = GetComponent<PhotonView>();
		spawnPoints = spawnManager.GetComponentsInChildren<Transform>();
		foreach(var spawnpoint in spawnPoints) {
			print(spawnpoint.position);
		}
       
    }
    void Awake()
    {
		
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // public override void OnEnable()
    // {
    //     base.OnEnable();
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // public override void OnDisable()
    // {
    //     base.OnDisable();
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {

        if (scene.buildIndex == 1) // We're in the game scene
        {
			int index = 0;
            if (PhotonNetwork.IsMasterClient)
            {
				
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    photonView.RPC("InstantiationPlayer", player, index);
                    index++;
                }
            }
            
        }
    }

	[PunRPC]
	private void InstantiationPlayer(int index) {
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"), spawnPoints[index].position, Quaternion.identity);
	}
}
