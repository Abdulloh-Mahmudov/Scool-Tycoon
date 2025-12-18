using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private float _earnRate = 5f;
    [SerializeField] private GameObject _platform;
    [SerializeField] private GameObject _buildingModel;
    [SerializeField] private GameObject _buildingUpgrade;
    [SerializeField] private GameObject _buildingFinalUpgrade;
    [SerializeField] private Transform _buildingLocation;
    [SerializeField] private GameObject _currentBuilding;
    [SerializeField] private bool _isMainBuilding;
    private int _buildingLevel = 1;
    private bool _isUpgraded;
    private Player _player;
    private UIManager _uiManager;
    private float _coolDown;
    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _coolDown = Time.time + _earnRate;
        _player = GameObject.Find("Player").GetComponent<Player>();
        GivePeople();
        _isUpgraded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if(SelectionManager.Instance.SelectedObject == this.gameObject)
        {
            IsSelected(true);
        }
        else
        {
            IsSelected(false);
        }

        Profits();
    }

    public void IsSelected(bool selected)
    {
        if(selected == true)
        {
            _platform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            _platform.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    public void Upgrade()
    {
        if (_player._finances >= _currentBuilding.GetComponent<Building_Stats>().upgrade)
        {
            if ( _isUpgraded == false)
            {
                _isUpgraded = true;
                _player._finances -= _currentBuilding.GetComponent<Building_Stats>().upgrade;
                Destroy(_currentBuilding);
                _currentBuilding = Instantiate(_buildingUpgrade, _buildingLocation.position, _buildingLocation.rotation);
                _currentBuilding.transform.parent = this.transform;
                GivePeople();
                _buildingLevel++;
            }

            else if(_isMainBuilding == true && _buildingLevel == 2)
            {
                Debug.Log("FinalBuilding");
                _buildingLevel++;
                _player._finances -= _currentBuilding.GetComponent<Building_Stats>().upgrade;
                Destroy(_currentBuilding);
                _currentBuilding = Instantiate(_buildingFinalUpgrade, _buildingLocation.position, _buildingLocation.rotation);
                _currentBuilding.transform.parent = this.transform;
                GivePeople();
            }
        }
    }

    public int GiveUpgradePrice()
    {
         int current = _currentBuilding.GetComponent<Building_Stats>().upgrade;
        return current;
    }

    void GivePeople()
    {
        int currentPeople = _currentBuilding.GetComponent<Building_Stats>().people;
        int maxPeople = _currentBuilding.GetComponent<Building_Stats>().maxpeople;

        _player.GetPeopleCurrent(currentPeople);

        if(_isMainBuilding == true)
        {
            _player.GetPeopleMax(maxPeople);
        }
    }
    public void Profits()
    {
        if (_coolDown < Time.time)
        {
            _coolDown = Time.time + _earnRate;
            _player.Finances(_currentBuilding.GetComponent<Building_Stats>().income);

            Debug.Log("Profit earned!!!");
        }
        _timer = _earnRate - (_coolDown - Time.time);
        _uiManager.SliderValue(_timer);
    }

    public int BuildingPrice()
    {
        return _currentBuilding.GetComponent<Building_Stats>().upgrade;
    }
}
