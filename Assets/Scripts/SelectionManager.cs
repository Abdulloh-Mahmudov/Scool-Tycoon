using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Instance;

    public GameObject SelectedObject;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void SelectObject(GameObject obj)
    {
        if (obj != null)
        {
            SelectedObject = obj;
            Debug.Log("Selected: " + obj.name);
        }
        else
        {
            SelectedObject = null;
        }
    }

    public void UpgradeButton()
    {
        if (SelectedObject != null)
        {
            if (SelectionManager.Instance.SelectedObject.CompareTag("Building"))
            {
                SelectionManager.Instance.SelectedObject.GetComponent<Building>().Upgrade();
            }
            else
            {
                return;
            }
        }
    }
}
