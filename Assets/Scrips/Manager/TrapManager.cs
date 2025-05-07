using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using static Cinemachine.DocumentationSortingAttribute;

enum GameLevel
{
    easy = 0,
    normal,
    hard,
    hell
}

public class TrapManager : MonoBehaviour
{
    public static TrapManager Instance { get; private set; }
    public GameObject trap;
    public int trapIncrease;
    public float trapGameDuration;
    private int damage;
    private int gameState;
    public Vector2 minPos = new Vector2(-9, 14);
    public Vector2 maxPos = new Vector2(9, 24);
    [SerializeField]NpcsTraper npc;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
    }

    public void GameStart()
    {
        GameLevelSetting();
        StartCoroutine(MakeTrap());
    }

    public void GameOver()
    {

        StopAllCoroutines();


        if (npc != null)
        {
            npc.NpcApper();
        }
    }

    public IEnumerator MakeTrap()
    {
        float frameTimer = 0f;

        while (frameTimer < trapGameDuration)
        {
            frameTimer += 1f;

            for (int i = 0; i < trapIncrease; i++)
            {
                GameObject theTrap = Instantiate(trap, this.transform);
                Trap trapScrip = theTrap.GetComponent<Trap>();
                trapScrip.trapDamage = damage;
                trapScrip.type = (TrapType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(TrapType)).Length);
                trapScrip.TrapSetting();

                float randomPosX = UnityEngine.Random.Range(minPos.x, maxPos.x);
                float randomPosY = UnityEngine.Random.Range(minPos.y, maxPos.y);
                theTrap.transform.position = new Vector2(randomPosX, randomPosY);
                Destroy(theTrap, 1.05f);
            }


            yield return new WaitForSeconds(1f);
            //1초마다 함수를 쪼개서 실행
        }
        int score = 0;
        switch (gameState)
        {
            case 0:
                score = 10;
                UiManager.Instance.UpdateScore(score);
                break;

            case 1:
                score = 10;
                UiManager.Instance.UpdateScore(score);
                break;

            case 2:
                score = 10;
                UiManager.Instance.UpdateScore(score);
                break;

            case 3:
                score = 10;
                UiManager.Instance.UpdateScore(score);
                break;
        }
        GameOver();
        
    }


    public void GameLevelSetting()
    {

        Array gameLevel = Enum.GetValues(typeof(GameLevel));
        GameLevel level = (GameLevel)gameLevel.GetValue(UnityEngine.Random.Range(0, gameLevel.Length));

        switch (level)
        {
            case GameLevel.easy:
                Debug.Log("쉬움 난이도");
                trapIncrease = 9;
                trapGameDuration = 15f;
                damage = 15;
                gameState = 0;
                break;

            case GameLevel.normal:
                Debug.Log("보통 난이도");
                trapIncrease = 15;
                trapGameDuration = 30f;
                damage = 30;
                gameState = 1;
                break;

            case GameLevel.hard:
                Debug.Log("어려움 난이도");
                trapIncrease = 24;
                trapGameDuration = 45f;
                damage = 50;
                gameState = 2;
                break;

            case GameLevel.hell:
                Debug.Log("지옥 난이도");
                trapIncrease = 30;
                trapGameDuration = 60f;
                damage = 100;
                gameState = 3;
                break;
        }
    }




}
