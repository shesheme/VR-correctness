using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public event EventHandler OnStatsChanged;

    public static int Min_Value = 0;
    public static int Max_Value = 100;

    public enum Type { 
        Head,
        RH,
        RF,
        LF,
        LH,
    }

    private SingleStat Head_Value;
    private SingleStat RH_Value;
    private SingleStat RF_Value;
    private SingleStat LF_Value;
    private SingleStat LH_Value;

    public Stats(int Head_ValueAmount, int RH_ValueAmount, int RF_ValueAmount, int LF_ValueAmount, int LH_ValueAmount)
    {
        Head_Value = new SingleStat(Head_ValueAmount);
        RH_Value = new SingleStat(RH_ValueAmount);
        RF_Value = new SingleStat(RF_ValueAmount);
        LF_Value = new SingleStat(LF_ValueAmount);
        LH_Value = new SingleStat(LH_ValueAmount);
    }

    private SingleStat GetSingleStat(Type statType)
    {
        switch (statType) {
            default:
            case Type.Head:   return Head_Value;
            case Type.RH:  return RH_Value;
            case Type.RF:    return RF_Value;
            case Type.LF:     return LF_Value;
            case Type.LH:   return LH_Value;
        }

    }
    public void SetStatAmount(Type statType, int StatAmount)
    {
        GetSingleStat(statType).SetStatAmount(StatAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }
    public void IncreaseStatAmount(Type statType)
    {
        SetStatAmount(statType, GetStatAmount(statType) + 1);
    }
    public void DecreaseStatAmount(Type statType)
    {
        SetStatAmount(statType, GetStatAmount(statType) - 1);
    }
    public int GetStatAmount(Type statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }
    public float GetStatAmountNormalized(Type statType)
    {
        return GetSingleStat(statType).GetStatAmountNormalized();
    }


    private class SingleStat
    {
        private int stat;
        public SingleStat(int statAmount)
        {
            SetStatAmount(statAmount);
        }
        public void SetStatAmount(int StatAmount)
        {
            stat = Mathf.Clamp(StatAmount, Min_Value, Max_Value);
        }
        public int GetStatAmount()
        {
            return stat;
        }
        public float GetStatAmountNormalized()
        {
            return (float)stat / Max_Value;
        }
    }
}
