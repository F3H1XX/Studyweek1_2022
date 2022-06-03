using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{
    [SerializeField] Text _CoinText;
    public  UICoinCounter _UICoinCounter;

    private string _counterText;
    // Start is called before the first frame update
    void Start()
    {
        _CoinText = GetComponent<Text>();
        _UICoinCounter.CoinCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _counterText = _UICoinCounter.CoinCounter.ToString();
        _CoinText.text = _counterText;
        
    }
}
