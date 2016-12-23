using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject : MonoBehaviour {

    public string ExamineText = "A generic object or something.";

    private void Examine()
    {
        GamePlayLog.LogMessage(ExamineText);
    }
}
