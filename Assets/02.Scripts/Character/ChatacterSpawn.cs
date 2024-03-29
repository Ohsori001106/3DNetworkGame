using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatacterSpawn : MonoBehaviour
{
    public Transform[] spawnPoints;

    public GameObject characterPrefab;

    void Start()
    {
        // 캐릭터를 생성할 위치를 랜덤하게 선택
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // 선택된 위치에 캐릭터 생성
        Instantiate(characterPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
