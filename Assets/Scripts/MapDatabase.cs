using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDatabase
{
    private static Dictionary<int, Map2dStart.MapData> _mapDatas = new Dictionary<int, Map2dStart.MapData>()
    {
        {
            1, 
            new Map2dStart.MapData()
            {
                Width = 1,
                Height = 4,
                Data = new int[]
                {
                    -1,
                    3,
                    3,
                    1,
                }
            }
        },
        {
            2, 
            new Map2dStart.MapData()
            {
                Width = 1,
                Height = 4,
                Data = new int[]
                {
                    -1,
                    3,
                    2,
                    1,
                }
            }
        },
        {
            3, 
            new Map2dStart.MapData()
            {
                Width = 2,
                Height = 4,
                Data = new int[]
                {
                    -1, 0,
                    3, 0,
                    0, 0,
                    3, 1,
                }
            }
        },
        {
            4, 
            new Map2dStart.MapData()
            {
                Width = 4,
                Height = 4,
                Data = new int[]
                {
                    -1, 0, 3, 0,
                    0, 2, 0, 0,
                    0, 2, 0, 0,
                    0, 0, 3, 1,
                }
            }
        },
        {
            5, 
            new Map2dStart.MapData()
            {
                Width = 6,
                Height = 6,
                Data = new int[]
                {
                    -1, 2, 0, 2, 0, 2,
                     2, 0, 2, 3, 2, 0,
                     0, 2, 0, 2, 0, 2,
                     2, 0, 3, 0, 2, 0,
                     0, 2, 0, 2, 0, 2,
                     2, 0, 2, 0, 2, 1,
                }
            }
        }
    };

    public static Map2dStart.MapData LoadMapDataByStageId(int id)
    {
        return _mapDatas[id];
    }
}
