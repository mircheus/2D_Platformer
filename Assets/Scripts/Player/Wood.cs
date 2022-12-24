using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private bool _isChopped = false;
    
    public void GetChopped()
    {
        if (_isChopped == false)
        {
            Debug.Log("I'm chopped");
            _isChopped = true;
        }    
    }
}
