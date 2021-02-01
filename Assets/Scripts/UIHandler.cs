using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    Player player;
    MiniBoss miniBoss;
    [SerializeField]  Boss[] bosses;
    [SerializeField] Image playerHP, miniBossHP, boss1, boss2, boss3;
    bool switched;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        miniBoss = FindObjectOfType<MiniBoss>();
        //bosses = FindObjectsOfType<Boss>();
    }
    private void Update()
    {

        playerHP.fillAmount = Mathf.Clamp(player.currentHP / player.maxHP, 0, 1);
        if (miniBoss.currentHP > 0)
        {
            miniBossHP.fillAmount = Mathf.Clamp(miniBoss.currentHP / miniBoss.maxHP, 0, 1);
        }
        else if (!switched)
        {
            switched = true;
            miniBossHP.transform.parent.parent.gameObject.SetActive(false);
            boss1.transform.parent.parent.gameObject.SetActive(true);
            boss2.transform.parent.parent.gameObject.SetActive(true);
            boss3.transform.parent.parent.gameObject.SetActive(true);
        }
        if (switched)
        {
            boss1.fillAmount = Mathf.Clamp(bosses[0].currentHP / bosses[0].maxHP, 0, 1);
            boss2.fillAmount = Mathf.Clamp(bosses[1].currentHP / bosses[1].maxHP, 0, 1);
            boss3.fillAmount = Mathf.Clamp(bosses[2].currentHP / bosses[2].maxHP, 0, 1);
        }
    }
}
