using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManageScript : MonoBehaviour
{
    public GameObject tank1;
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    public GameObject normalPage;
    public Text nHP, nSupply, nArmorIntegrity, nEffectsText;
    public GameObject informationPage;
    public Text iCountry, iAlly, iArmorThickness, iHardDamage, iSoftDamage, iSupplyCapacity, iSupplyBonus;
    public GameObject itemPage;
    public Image item1Image, item2Image, item3Image;
    public Text item1Text, item2Text, item3Text;
    public List<Sprite> sprites = new List<Sprite>();
    public Text specialBulletsText;


    void Start()
    {
        tankDataScript = tank1.GetComponent<TankDataScript>();
        tankLogicScript = tank1.GetComponent<TankLogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.U))
        {
            if (tankDataScript.items.Count >= 1)
            {
                item1Image.sprite = sprites[tankDataScript.items[0]];
                item1Text.text = tankDataScript.itemNames[tankDataScript.items[0]] + "\n" + tankDataScript.itemCosts[tankDataScript.items[0]] + "K";
                if (tankDataScript.items[0] == 4) item1Text.text += " " + tankDataScript.itemDatas[0] + "/8";
                if (tankDataScript.items[0] == 8) item1Text.text += " " + tankDataScript.itemDatas[0] + "/4";
                item1Image.enabled = true;
                item1Text.enabled = true;
            }
            else
            {
                item1Image.enabled = false;
                item1Text.enabled = false;
            }
            if (tankDataScript.items.Count >= 2)
            {
                item2Image.sprite = sprites[tankDataScript.items[1]];
                item2Text.text = tankDataScript.itemNames[tankDataScript.items[1]] + "\n" + tankDataScript.itemCosts[tankDataScript.items[1]] + "K";
                if (tankDataScript.items[1] == 4) item2Text.text += " " + tankDataScript.itemDatas[1] + "/8";
                if (tankDataScript.items[0] == 8) item1Text.text += " " + tankDataScript.itemDatas[0] + "/4";
                item2Image.enabled = true;
                item2Text.enabled = true;
            }
            else
            {
                item2Image.enabled = false;
                item2Text.enabled = false;
            }
            if (tankDataScript.items.Count >= 3)
            {
                item3Image.sprite = sprites[tankDataScript.items[2]];
                item3Text.text = tankDataScript.itemNames[tankDataScript.items[2]] + "\n" + tankDataScript.itemCosts[tankDataScript.items[2]] + "K";
                if (tankDataScript.items[2] == 4) item3Text.text += " " + tankDataScript.itemDatas[2] + "/8";
                if (tankDataScript.items[0] == 8) item1Text.text += " " + tankDataScript.itemDatas[0] + "/4";
                item3Image.enabled = true;
                item3Text.enabled = true;
            }
            else
            {
                item3Image.enabled = false;
                item3Text.enabled = false;
            }

            specialBulletsText.text = "";
            int[] specialBullets = tankDataScript.specialBullets.ToArray();
            for (int i = 0; i < specialBullets.Length; i++)
            {
                specialBulletsText.text += tankDataScript.bulletNames[specialBullets[i]] + " " + tankDataScript.FireConsumption[specialBullets[i]] + "K" + "\n";
            }

            itemPage.SetActive(true);
            informationPage.SetActive(false);
            normalPage.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            iCountry.text = tankDataScript.country;
            iAlly.text = tankDataScript.ally;
            iArmorThickness.text = tankDataScript.armorThickness.ToString();
            iHardDamage.text = tankDataScript.hardDamage.ToString();
            iSoftDamage.text = tankDataScript.softDamage.ToString();
            iSupplyCapacity.text = tankDataScript.supplyCapacity.ToString();
            iSupplyBonus.text = tankDataScript.supplyComsumptionBonus.ToString();
            itemPage.SetActive(false);
            informationPage.SetActive(true);
            normalPage.SetActive(false);
        }
        else
        {
            nHP.text = tankDataScript.cHP.ToString() + "/" + tankDataScript.HP.ToString();
            nSupply.text = (Mathf.Round(tankDataScript.cSupply * 10f) / 10f).ToString() + "/" + tankDataScript.supplyCapacity.ToString() + ".0";
            nArmorIntegrity.text = ((Mathf.Round(100f * tankDataScript.armorIntegrity)) / 100f).ToString() + "/1.00";
            string text = "";
            if (tankDataScript.effects[0]) text += "ѹ��";
            if (tankDataScript.effects[1])
            {
                if (text != "") text += "��";
                text += "���";
            }
            if (tankDataScript.effects[2])
            {
                if (text != "") text += "��";
                text += "����";
            }
            if (tankLogicScript.isImmune())
            {
                if (text != "") text += "��";
                text += "����";
            }
            if (!tankLogicScript.canUseItem())
            {
                if (text != "") text += "��";
                text += "�޷�ʹ�õ���";
            }

            nEffectsText.text = text;
            itemPage.SetActive(false);
            informationPage.SetActive(false);
            normalPage.SetActive(true);
        }
    }
}