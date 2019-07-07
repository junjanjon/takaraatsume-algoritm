using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MapDatabase
{
    private static Dictionary<int, Map2dStart.MapData> _mapDatas = new Dictionary<int, Map2dStart.MapData>()
    {
        {
            1, 
            new Map2dStart.MapData(
                width: 1,
                height: 4,
                data: new int[]
                {
                    -1,
                    3,
                    3,
                    1,
                }
            )
        },
        {
            2, 
            new Map2dStart.MapData(
                width: 1,
                height: 4,
                data: new int[]
                {
                    -1,
                    3,
                    2,
                    1,
                }
            )
        },
        {
            3, 
            new Map2dStart.MapData(
                width: 2,
                height: 4,
                data: new int[]
                {
                    -1, 0,
                    3, 0,
                    0, 0,
                    3, 1,
                }
            )
        },
        {
            4, 
            new Map2dStart.MapData(
                width: 4,
                height: 4,
                data: new int[]
                {
                    -1, 0, 3, 0,
                    0, 2, 0, 0,
                    0, 2, 0, 0,
                    0, 0, 3, 1,
                }
            )
        },
        {
            5, 
            new Map2dStart.MapData(
                width: 6,
                height: 6,
                data: new int[]
                {
                    -1, 2, 0, 2, 0, 2,
                    2, 0, 2, 3, 2, 0,
                    0, 2, 0, 2, 0, 2,
                    2, 0, 3, 0, 2, 0,
                    0, 2, 0, 2, 0, 2,
                    2, 0, 2, 0, 2, 1,
                }
            )
        }
    };

    public static Map2dStart.MapData LoadMapDataByStageId(int id)
    {
        return new Map2dStart.MapData(_mapDatas[id]);
    }
}
