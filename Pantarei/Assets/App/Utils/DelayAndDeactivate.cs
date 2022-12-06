using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAndDeactivate : MonoBehaviour
{
    public FollowMeToggle fileExplorer;
    public void DelayAndDeactivateFunction()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        fileExplorer.GetComponent<FollowMeToggle>().SetFollowMeBehavior(true);
        yield return new WaitForSeconds(0.5f);
        fileExplorer.GetComponent<FollowMeToggle>().SetFollowMeBehavior(false);
    }
}
