using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColumnManager : MonoBehaviour
{
    /// Coloum Manager Script Instance..
    public static ColumnManager instance;
    /// All Coloum Reference Array..
    public ColumnScript[] columnScripts;
    public List<GameObject> wildclones;
    public float MyGap;
    public bool LongSpin = false;
    public int ColumnsStopped = 5;
    public bool ShouldStopAtOnce = false;
    public bool isStickyWild = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Set Coloum Indentifire Index..
        for (int i = 0; i < columnScripts.Length; i++)
        {
            columnScripts[i].columnIndex = i;
        }
    }
    public void AssingLuckyPercentage(int value) {
        for (int i = 0; i < columnScripts.Length; i++)
        {
            columnScripts[i].luckyItemPercent = value;
        }

    }

    /// When Called Slot Item Start Spining..
    public IEnumerator StartSpinningColumn()
    {
        ColumnsStopped = 5;
        GameOperations.instance.BonusMatched = 0;
        GameOperations.instance.scattersMatched = 0;
        ShouldStopAtOnce = false;
        for (int i = 0; i < columnScripts.Length; i++)
        {
            columnScripts[i].StartSpin();
            yield return new WaitForSeconds(.05f);
        }
        StopCoroutine(StartSpinningColumn());
    }

    public void StopSpinnigButtonClicked() {
        StartCoroutine(StopSpinningCor());
    }
    public IEnumerator StopSpinningCor() {
        ShouldStopAtOnce = true;
        for (int i = 0; i < columnScripts.Length; i++)
        {
            if (columnScripts[i].isSpinning) {
                columnScripts[i].shouldStopSpinning = true;
                columnScripts[i].CancelInvoke("StopSpin");
                yield return new WaitForSeconds(0.1f);
            }

        }
        StopCoroutine(StopSpinningCor());
    }

    internal SlotItem GetSlotItemAt(int columnIndex, int rowIndex)
    {
        return (SlotItem)columnScripts[columnIndex].slotItemList[rowIndex];
        
    }

}// End Script
