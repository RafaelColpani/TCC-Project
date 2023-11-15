using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class SequentialPlatformPuzzle : MonoBehaviour
{
    [HeaderPlus(" ", "- GRID -", (int)HeaderPlusColor.green)]
    [Tooltip("The number of rows that the grid have")]
    [SerializeField] int rowsCount;
    [SerializeField] int columnsCount;
}
