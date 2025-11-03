using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Screen : ActivityScreen
{
    override protected IEnumerator PlayScreenAnimation()
    {
        Debug.Log("ending screen");
        yield break;
    }



}
