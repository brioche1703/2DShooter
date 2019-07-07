using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSceneManager : MonoBehaviour
{
    
    public Button addLifeButton;
    public Button addShieldCapacityButton;
    public Button decreaseShieldRecoveringRateButton;
    public Button rechargeRocketAmmoButton;
    public Button addRocketAmmoCapacityButton;
    public Button nextButton;

    public int addLifePrice = 5;
    public int addShieldCapacityPrice = 5;
    public int decreaseShieldRecoveringRatePrice = 5;
    public int oneRocketPrice = 1;
    public int addRocketAmmoCapacityPrice = 5;

    public Image[] hearts;
    public Text coinsText;
    public Text rocketText;

    void Start()
    {
        UpdateButtons();
        UpdateHeartsUI();
        UpdateCoinsTextUI();
        UpdateRocketTextUI();
    }

    void Update()
    {
        UpdateButtons();
        UpdateHeartsUI();
        UpdateCoinsTextUI();
        UpdateRocketTextUI();
    }

    public void UpdateButtons()
    {
        int coins = PersistentManagerScript.Instance.coins;
        // Add life button
        if (PersistentManagerScript.Instance.p_lives >= PersistentManagerScript.Instance.MAX_LIVES)
        {
            MakeButtonUninteractable(addLifeButton);
            addLifeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Already 3 lives";
        }
        else
        {
            if (coins >= addLifePrice)
            {
                MakeButtonInteractable(addLifeButton);
                addLifeButton.GetComponentInChildren<TextMeshProUGUI>().text = "+1 life (" + addLifePrice + "c)";
            }
            else
            {
                MakeButtonUninteractable(addLifeButton);
                addLifeButton.GetComponentInChildren<TextMeshProUGUI>().text = "+1 life\nNot enough coins (" + addLifePrice + "c)";
            }
        }
       
        // Add shield capacity button 
        if (PersistentManagerScript.Instance.p_startingShieldCapacity 
                >= PersistentManagerScript.Instance.MAX_SHIELD)
        {
            MakeButtonUninteractable(addShieldCapacityButton);
            addShieldCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "Already max shield";
        }
        else
        {
            if (coins >= addShieldCapacityPrice)
            {
                MakeButtonInteractable(addShieldCapacityButton);
                addShieldCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "+10 shield capacity (" + addShieldCapacityPrice + "c)";
            }
            else
            {
                MakeButtonUninteractable(addShieldCapacityButton);
                addShieldCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "+10 shield capacity\nNot enough coins (" + addShieldCapacityPrice + "c)";
            }
        }

        // Decrease shield recover rate button
        if (PersistentManagerScript.Instance.p_shieldRecoverTimeDuration <= PersistentManagerScript.Instance.MIN_SHIELD_RECOVER_RATE)
        {
            MakeButtonUninteractable(decreaseShieldRecoveringRateButton);
            addShieldCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "Already minimum recover shield rate";
        }
        else
        {
            if (coins >= decreaseShieldRecoveringRatePrice)
            {
                MakeButtonInteractable(decreaseShieldRecoveringRateButton);
                decreaseShieldRecoveringRateButton.GetComponentInChildren<TextMeshProUGUI>().text = "-0.5s shield recovering rate (" + decreaseShieldRecoveringRatePrice + "c)";
            }
            else
            {
                MakeButtonUninteractable(decreaseShieldRecoveringRateButton);
                decreaseShieldRecoveringRateButton.GetComponentInChildren<TextMeshProUGUI>().text = "-0.5s shield recovering rate\nNot enough coins (" + decreaseShieldRecoveringRatePrice + "c)";
            }
        }

        // Recharge rocket ammo to current max capacity
        if (PersistentManagerScript.Instance.p_rocketAmmo >= PersistentManagerScript.Instance.p_rocketAmmoCurrentCapacityMax)
        {
            MakeButtonUninteractable(rechargeRocketAmmoButton);
            rechargeRocketAmmoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Already max rocket ammo";
        }
        else
        {
            int totalPrice = oneRocketPrice * (PersistentManagerScript.Instance.p_rocketAmmoCurrentCapacityMax - PersistentManagerScript.Instance.p_rocketAmmo);
            if (coins >= totalPrice)
            {
                MakeButtonInteractable(rechargeRocketAmmoButton);
                rechargeRocketAmmoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Recharge rockets (" + totalPrice + "c)";
            }
            else
            {
                MakeButtonUninteractable(rechargeRocketAmmoButton);
                rechargeRocketAmmoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Recharge rockets\nNot enough coins (" + totalPrice + "c)";
            }
        }
        // Add rocket capacity 
        if (PersistentManagerScript.Instance.p_rocketAmmoCurrentCapacityMax >= PersistentManagerScript.Instance.MAX_ROCKET_AMMO_CAPACITY)
        {
            MakeButtonUninteractable(addRocketAmmoCapacityButton);
            addRocketAmmoCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "Already max rocket ammo capacity";
        }
        else
        {
            if (coins >= addRocketAmmoCapacityPrice)
            {
                MakeButtonInteractable(addRocketAmmoCapacityButton);
                addRocketAmmoCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "+1 Rocket ammo capacity (" + addRocketAmmoCapacityPrice + "c)";
            }
            else
            {
                MakeButtonUninteractable(addRocketAmmoCapacityButton);
                addRocketAmmoCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "+1 Rocket ammo capacity\nNot enough coins (" + addRocketAmmoCapacityPrice + "c)";
            }
        }
    }

    public void MakeButtonInteractable(Button b)
    {
        b.interactable = true; 
        var tempColor = b.GetComponent<Image>().color;
        tempColor.a = 1.0f;
        b.GetComponent<Image>().color = tempColor;
        b.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    }

    public void MakeButtonUninteractable(Button b)
    {
        b.interactable = false; 
        var tempColor = b.GetComponent<Image>().color;
        tempColor.a = 0.5f;
        b.GetComponent<Image>().color = tempColor;
        b.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(1.0f, 1.0f, 1.0f, 0.5f);
    }

    public void AddLife()
    {
        PersistentManagerScript.Instance.p_lives++;
        PersistentManagerScript.Instance.coins -= addLifePrice;
    }

    public void AddShieldCapacity()
    {
        PersistentManagerScript.Instance.coins -= addShieldCapacityPrice;
        PersistentManagerScript.Instance.p_startingShieldCapacity += 10;
    }

    public void DecreaseShieldRecoveringRate()
    {
        PersistentManagerScript.Instance.coins -= decreaseShieldRecoveringRatePrice;
        PersistentManagerScript.Instance.p_shieldRecoverTimeDuration -= 0.5f;
    }

    public void RechargeRocketAmmo()
    {
        int price = PersistentManagerScript.Instance.p_rocketAmmoCurrentCapacityMax - PersistentManagerScript.Instance.p_rocketAmmo;
        PersistentManagerScript.Instance.coins -= price;
        PersistentManagerScript.Instance.p_rocketAmmo = PersistentManagerScript.Instance.p_rocketAmmoCurrentCapacityMax;
    }

    public void AddRocketAmmoCapacity()
    {
        PersistentManagerScript.Instance.coins -= addRocketAmmoCapacityPrice;
        PersistentManagerScript.Instance.p_rocketAmmoCurrentCapacityMax++;
    }

    public void NextScene()
    {
        PersistentManagerScript.Instance.LoadLevelScene();
    }

    void UpdateHeartsUI()
    {
        int nbLives = PersistentManagerScript.Instance.p_lives;
        for (int i = 0; i < PersistentManagerScript.Instance.MAX_LIVES; i++)
        {
            if (i < nbLives)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void UpdateCoinsTextUI()
    {
        coinsText.text = PersistentManagerScript.Instance.coins.ToString(); 
    }

    void UpdateRocketTextUI()
    {
        rocketText.text = PersistentManagerScript.Instance.p_rocketAmmo.ToString(); 
    }
}
