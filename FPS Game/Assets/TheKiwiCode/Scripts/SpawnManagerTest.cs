using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerTest : MonoBehaviour
{
    Transform [] spawnPoints;
    void Start()
    {
        Transform[] spawnPoints = GetComponentsInChildren<Transform>();
    }
}
