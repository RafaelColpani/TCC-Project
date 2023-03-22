using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class save : MonoBehaviour
{
    enum SaveMode { UNABLE, WAIT, ABLE }
    SaveMode saveMode = SaveMode.UNABLE;

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag) 
        {
            case "save":
                saveMode = SaveMode.WAIT;
                print("gotta wait");
                break;
            case "saveWait":
                saveMode = SaveMode.UNABLE;
                print("save disabled");
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (saveMode == SaveMode.UNABLE && collision.tag.Equals("save"))
        {
            print("save enabled");
            saveMode = SaveMode.ABLE;
        }
        else if (collision.tag.Equals("saveWait")) print("entered save area");
    }

    private void Update()
    {
        if (saveMode == SaveMode.ABLE)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Salvou. save wait");
                saveMode = SaveMode.WAIT;
            }
        }
    }
}
