using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHint : MonoBehaviour
{
    [SerializeField] GameObject ButtonHint;

    // Start is called before the first frame update
    void Start()
    {
        ButtonHint.SetActive(false);
    }

    // OnMouseOver is called when Cursor over GameObject
    public void OnMouseOver()
    {
        ButtonHint.SetActive(true);
        Vector3 mousePos = Input.mousePosition;
        ButtonHint.transform.position = mousePos;
    }

    public void OnMouseExit()
    {
        ButtonHint.SetActive(false);
    }
}
