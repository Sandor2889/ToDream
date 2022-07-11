using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    private bool inTrigger = false;

    public List<int> availableQuestId = new List<int>();
    public List<int> receivableQuestId = new List<int>();

    private void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.F))
        {

        }
    }

    private void OnTrggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inTrigger = false;
        }
    }
}
