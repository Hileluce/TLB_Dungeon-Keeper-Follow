using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUIpanelV2 : MonoBehaviour
{
    public Sprite healthEmpty;
    public Sprite healthHalf;
    public Sprite healthFull;
    public Sprite armoredHeart;
    public Sprite currentGadget;
    public Sprite secondGadget;

    Text keyCountText;
    List<Image> healthImages;
    int healthBarCount;

    private void Start()
    {
        DrayEvents.RefreshHealthUI.AddListener(RefreshDisplayedHearts);
        Transform trans = transform.Find("Key Count");
        keyCountText = trans.GetComponent<Text>();
        Transform healthPanel = transform.Find("Health Panel");
        healthImages = new List<Image>();
        if (healthPanel != null)
        {
            for (int i = 0; i < 20; i++)
            {
                trans = healthPanel.Find("H_" + i);
                if (trans == null) break;
                healthImages.Add(trans.GetComponent<Image>());
            }
        }
        RefreshDisplayedHearts();
        
    }
    private void Update()
    {
        keyCountText.text = Dray.NUM_KEYS.ToString();
    }
    void RefreshDisplayedHearts()
    {
        healthBarCount = Dray.MAX_HEALTH;
        int tMax = healthBarCount;
        for (int i = 0; i < healthImages.Count; i++)
        {
            
            healthImages[i].gameObject.SetActive(false);
            if (tMax > 0) { healthImages[i].gameObject.SetActive(true); }
            tMax -= 2;
        }
        int healthDisp = Dray.HEALTH;
        for (int i = 0; i < healthBarCount/2; i++)
        {
            //print("This is i " + i + "and this is " + healthImages[i].name);
            if (healthDisp > 1)
            {
                healthImages[i].sprite = healthFull;
            }
            else if (healthDisp == 1)
            {
                healthImages[i].sprite = healthHalf;
            }
            else { healthImages[i].sprite = healthEmpty; }
            healthDisp -= 2;
        }
        int armoredHearts = Dray.ARMORED_HEALTH;
        if (armoredHearts > 0)
        {
            for (int i = 0; i < armoredHearts; i++)
            {
                healthImages[i].sprite = armoredHeart;
            }
        }
    }
}
