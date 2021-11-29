using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelCollider : MonoBehaviour
{
    public int triggerIndex;
    private void OnTriggerEnter(Collider other)
    {
        ICrowdUnit enemy;
        if (other.TryGetComponent<ICrowdUnit>(out enemy))
        {
            if (enemy.TeamIndex == triggerIndex)
            {
                if (triggerIndex == 0)
                    ScreensManager.Instance.ShowSuccessScreen();
                else
                {
                    ScreensManager.Instance.ShowLoseScreen();

                }
            }
        }
    }
}
