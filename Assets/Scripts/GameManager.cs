using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    Player player;
    MiniBoss miniBoss;
    Boss[] bosses;
    bool allBosses;
    bool sceneRestarting;
    bool allDead;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        miniBoss = FindObjectOfType<MiniBoss>();
        bosses = FindObjectsOfType<Boss>();
        for (int i = 0; i < bosses.Length; i++)
        {
            bosses[i].gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (!player.gameObject.activeSelf && !sceneRestarting)
        {
            sceneRestarting = true;
            StartCoroutine(WaitThenRestartScene());
        }
        if (!miniBoss.gameObject.activeSelf && !allBosses)
        {
            allBosses = true;
            EnableBosses();
        }
        if (allBosses)
        {

            CheckBosses();
            if (allDead && !sceneRestarting)
            {
                sceneRestarting = true;
                StartCoroutine(WaitThenRestartScene());
            }
        }
    }
    void CheckBosses()
    {
        for (int i = 0; i < bosses.Length; i++)
        {
            if (!bosses[i].gameObject.activeSelf)
            {
                allDead = true;
            }
            else
            {
                allDead = false;
                break;
            }
        }
    }
    private void EnableBosses()
    {
        for (int i = 0; i < bosses.Length; i++)
        {
            bosses[i].gameObject.SetActive(true);
            bosses[i].Rise();
        }
    }

    IEnumerator WaitThenRestartScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
