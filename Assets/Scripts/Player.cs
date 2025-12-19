using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public int _finances = 0;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private SelectionManager _sManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private float _xBoundary;
    [SerializeField] private float _xBoundaryNegative;
    [SerializeField] private float _yBoundary;
    [SerializeField] private float _yBoundaryNegative;
    [SerializeField] private float _zBoundary;
    [SerializeField] private float _zBoundaryNegative;
    [SerializeField] private int _peopleMax;
    [SerializeField] private int _peopleCurrent;
    Vector2 lastTouchPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        MobileMovement();
        MobileZoom();
#else
        Movement();
#endif

        Boundaries();
        //if (IsTouchOverUI()) return;
        _uiManager.FinancesValue(_finances);
        _uiManager.GetPeopleCount(_peopleCurrent, _peopleMax);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.CompareTag("Building"))
                {
                    SelectionManager.Instance.SelectObject(hit.transform.gameObject);
                }
                else
                {
                    SelectionManager.Instance.SelectObject(null);
                }
            }
        }


    }


    public void Movement()
    {
        if (Input.GetKey(KeyCode.R)) 
        {
            transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.T))
        {
            transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime, Space.Self);
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(vertical, 0, -horizontal) * _speed * Time.deltaTime, Space.Self);
    }

    public void MobileMovement()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;

                Vector3 move = new Vector3
                    (
                    -delta.x * _speed / 5 * Time.deltaTime,
                    0,
                    -delta.y * _speed / 5 * Time.deltaTime
                    );

                transform.Translate(move, Space.World);
            }
        }
    }

    public void MobileZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float prevDist = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;

            float currDist = (t0.position - t1.position).magnitude;

            float delta = currDist - prevDist;

            Camera.main.fieldOfView -= delta * _speed / 5 * Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 60f);
        }
    }

    bool IsTouchOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }

    public void Finances(int earnings)
    {
        if (_peopleMax != 0)
        {
            float percent = 0;
            if (_peopleCurrent <= _peopleMax)
            {
                percent = (float)_peopleCurrent / _peopleMax;
               
            }
            else if (_peopleCurrent > _peopleMax)
            {
                 percent = 0.75f;
               
            }

            _finances += Mathf.FloorToInt(earnings * percent);
        }
        
    }

    public void Boundaries()
    {
        if (transform.position.x > _xBoundary)
        {
            transform.position = new Vector3(_xBoundary, transform.position.y,transform.position.z);
        }
        else if (transform.position.x < _xBoundaryNegative)
        {
            transform.position = new Vector3(_xBoundaryNegative, transform.position.y, transform.position.z);
        }

        if (transform.position.y > _yBoundary)
        {
            transform.position = new Vector3(transform.position.x, _yBoundary, transform.position.z);
        }
        else if (transform.position.y < _yBoundaryNegative)
        {
            transform.position = new Vector3(transform.position.x, _yBoundaryNegative, transform.position.z);
        }

        if (transform.position.z > _zBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zBoundary);
        }
        else if (transform.position.z < _zBoundaryNegative)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zBoundaryNegative);
        }
    }


    public void GetPeopleCurrent(int current)
    {
        _peopleCurrent += current;
    }

    public void GetPeopleMax(int max)
    {
        _peopleMax = max;
    }

    public bool GameEnded()
    {
        bool gameEnd = false;
        if(_peopleCurrent >= 60)
        {
            gameEnd = true;
        }
        return gameEnd;
    }
    }
