using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private static UIManager instance;
    public static UIManager Instance {get => instance;}

    // public static UIManager Instance;

    private void Awake()
    {
        UIManager.instance = this;
    }

    [SerializeField] Text coinText;

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}

