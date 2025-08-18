using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class UIManageScript : MonoBehaviour
{
    public GameObject tank;
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    public GameObject normalPage;
    public Text nHP, nSupply, nArmorIntegrity, nEffectsText;
    public GameObject informationPage;
    public Text iArmorThickness, iHardDamage, iSoftDamage;
    public Image MainPower, Ally;
    public GameObject itemPage;
    public Image item1Image, item2Image, item3Image;
    public List<Sprite> sprites = new List<Sprite>();
    public Text specialBulletsText;
    public int debugFixPageIndex;
    /*
     * 1 Normal Page
     * 2 Information Page
     * 3 Item Page
     */


    void Start()
    {
        tankDataScript = tank.GetComponent<TankDataScript>();
        tankLogicScript = tank.GetComponent<TankLogicScript>();

        tank = GameObject.FindGameObjectWithTag("Player1").gameObject;
    }

    void Update()
    {
        RenderNormalPage();
        RenderInfoPage();
        // RenderItemPage();
    }

    //渲染普通交互界面
    private void RenderNormalPage()
    {
        nHP.text = tankDataScript.cHP.ToString() + "/" + tankDataScript.HP.ToString();
        nSupply.text = (Mathf.Round(tankDataScript.cSupply * 10f) / 10f).ToString() + "/" + tankDataScript.supplyCapacity.ToString() + ".0";
        nArmorIntegrity.text = (Mathf.Round(100f * tankDataScript.armorIntegrity) / 100f).ToString() + "/1.00";
        string text = "";
        if (tankDataScript.effects[0]) text += "被压制";
        if (tankDataScript.effects[1])
        {
            if (text != "") text += " ";
            text += "冲击";
        }
        if (tankDataScript.effects[2])
        {
            if (text != "") text += " ";
            text += "伏击";
        }
        if (tankLogicScript.isImmune())
        {
            if (text != "") text += " ";
            text += "免疫";
        }
        if (!tankLogicScript.doConsumeSupply())
        {
            if (text != "") text += " ";
            text += "移动与攻击花费为零";
        }
        if (!tankLogicScript.canUseItem())
        {
            if (text != "") text += " ";
            text += "无法使用道具";
        }

        nEffectsText.text = text;
        normalPage.SetActive(true);
    }

    //渲染信息界面
    private void RenderInfoPage()
    {
        _LoadCountryIcon(MainPower, tankDataScript.country);
        _LoadCountryIcon(Ally, tankDataScript.ally);
        iArmorThickness.text = tankDataScript.armorThickness.ToString();
        iHardDamage.text = tankDataScript.hardDamage.ToString();
        iSoftDamage.text = tankDataScript.softDamage.ToString();
        informationPage.SetActive(true);
    }

    //渲染道具界面
    // private void RenderItemPage()
    // {
    //     if (tankDataScript.items.Count >= 1)
    //     {
    //         item1Image.sprite = sprites[tankDataScript.getId(0)];
    //         item1Image.enabled = true;
    //     }
    //     else
    //     {
    //         item1Image.enabled = false;
    //     }
    //     if (tankDataScript.items.Count >= 2)
    //     {
    //         item2Image.sprite = sprites[tankDataScript.getId(1)];
    //         item2Image.enabled = true;
    //     }
    //     else
    //     {
    //         item2Image.enabled = false;
    //     }
    //     if (tankDataScript.items.Count >= 3)
    //     {
    //         item3Image.sprite = sprites[tankDataScript.getId(2)];
    //         item3Image.enabled = true;
    //     }
    //     else
    //     {
    //         item3Image.enabled = false;
    //     }

    //     specialBulletsText.text = "";
    //     int[] specialBullets = tankDataScript.specialBullets.ToArray();
    //     for (int i = 0; i < specialBullets.Length; i++)
    //     {
    //         specialBulletsText.text += tankDataScript.bulletNames[specialBullets[i]] + " " + tankDataScript.FireConsumption[specialBullets[i]] + "K" + "\n";
    //     }

    //     itemPage.SetActive(true);
    // }

    //加载图标主国盟国时用
    private void _LoadCountryIcon(Image Image, string Country) //Country = 'GER'之类
    {
        Sprite icon = Resources.Load<Sprite>("CountriesIcon/" + Country);
        if (icon != null)
        {
            Image.sprite = icon;
        }
        else
        {
            Debug.Log("Failed to load" + Country + ".png");
        }
    }
}