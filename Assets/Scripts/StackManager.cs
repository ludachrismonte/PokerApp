﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StackManager : NetworkBehaviour {

    //Stack Counts
    [SyncVar] private int Stack0;
    [SyncVar] private int Stack1;
    [SyncVar] private int Stack2;
    [SyncVar] private int Stack3;
    [SyncVar] private int Stack4;
    [SyncVar] private int Stack5;
    [SyncVar] private int Stack6;
    [SyncVar] private int Stack7;
    [SyncVar] private int Stack8;

    public void SetStack(int ID, int ChipCount)
    {
        Debug.Log("Updating " + ID + " to " + ChipCount);
        switch (ID)
        {
            case 0:
                Stack0 = ChipCount;
                break;
            case 1:
                Stack1 = ChipCount;
                break;
            case 2:
                Stack2 = ChipCount;
                break;
            case 3:
                Stack3 = ChipCount;
                break;
            case 4:
                Stack4 = ChipCount;
                break;
            case 5:
                Stack5 = ChipCount;
                break;
            case 6:
                Stack6 = ChipCount;
                break;
            case 7:
                Stack7 = ChipCount;
                break;
            case 8:
                Stack8 = ChipCount;
                break;
        }
    }

    public void ModStack(int ID, int amt)
    {
        switch (ID)
        {
            case 0:
                Stack0 += amt;
                break;
            case 1:
                Stack1 += amt;
                break;
            case 2:
                Stack2 += amt;
                break;
            case 3:
                Stack3 += amt;
                break;
            case 4:
                Stack4 += amt;
                break;
            case 5:
                Stack5 += amt;
                break;
            case 6:
                Stack6 += amt;
                break;
            case 7:
                Stack7 += amt;
                break;
            case 8:
                Stack8 += amt;
                break;
        }
    }

    public int GetStack(int ID) {
        switch (ID)
        {
            case 0:
                return Stack0;
            case 1:
                return Stack1;
            case 2:
                return Stack2;
            case 3:
                return Stack3;
            case 4:
                return Stack4;
            case 5:
                return Stack5;
            case 6:
                return Stack6;
            case 7:
                return Stack7;
            case 8:
                return Stack8;
        }
        return -1;
    }
}
