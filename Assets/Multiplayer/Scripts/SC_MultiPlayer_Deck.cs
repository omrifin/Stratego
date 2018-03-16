using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MultiPlayer_Deck : MonoBehaviour
{


    public void OnMouseDown()
    {
        print("UserPressedSC_MultiPlayer_Deck");
        SC_MultiPlayer_Controller.Instance.UserPressedSC_MultiPlayer_Deck();
    }
}
