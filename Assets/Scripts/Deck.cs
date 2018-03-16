using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {


    public void OnMouseDown()
    {
        print("UserPressedDeck");
        SC_Controller.Instance.UserPressedDeck();
    }
}
