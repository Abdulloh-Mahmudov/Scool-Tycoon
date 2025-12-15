using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider _sliderBar;
    [SerializeField] Text _finances;
    [SerializeField] Button _upgrade;
    [SerializeField] Text _upgradeText;
    private GameObject _selected;
    private int _price;
    [SerializeField] private Text _time;
    private float _timer;
    private int _timeSeconds;
    private int _timeMinutes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        _selected = SelectionManager.Instance.SelectedObject;
        if(_selected != null)
        {
          _price = _selected.GetComponent<Building>().GiveUpgradePrice();
        }
        _upgrade.onClick.RemoveAllListeners();
        TimeCounter();
        _time.text = _timeMinutes + " : " + _timeSeconds;
        _upgradeText.text = "Upgrade cost:" + _price;
    }

    public void SliderValue(float percentage)
    {
        _sliderBar.value = percentage;
    }

    public void FinancesValue(int money)
    {
        _finances.text = money.ToString();
        if(money >= _price)
        {
            _upgrade.gameObject.GetComponent<Image>().color = Color.green;
        }
        else
        {
            _upgrade.gameObject.GetComponent<Image>().color = Color.red;
        }
    }

    void TimeCounter()
    {
        _timer += Time.deltaTime;

        _timeSeconds = (int)_timer;
        if (_timeSeconds >= 60)
        {
            _timeMinutes++;
            _timer -= 60f; 
        }
    }
}
