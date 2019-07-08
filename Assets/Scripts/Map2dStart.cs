using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map2dStart : MonoBehaviour
{
    [SerializeField]
    private Sprite _greenSprite;
    [SerializeField]
    private Sprite _holeSprite;
    [SerializeField]
    private Sprite _goalSprite;
    [SerializeField]
    private Sprite _treasureSprite;
    [SerializeField]
    private Sprite _treasureSprite2;

    [SerializeField]
    private Sprite[] _playerSprites;
    
    private Texture2D[] _textures = null;
    private Texture2D[] _playerTextures = null;

    private int playerIndex = 0;
    private int goalIndex = 0;
    private int playerDirection = 0; // 0: down, +1 : turn right
    private int playerAnime = 0;
    private int playerTimer = 0;
    private int playerWalkTimer = 0;
    
    private enum PlayState 
    {
        Playing,
        Success,
        Failled,
    }

    private PlayState current = PlayState.Playing;

    private int GetPlayerAnimeIndex()
    {
        return playerDirection * 3 + playerAnime;
    }

    private void UpdatePlayerTimer()
    {
        const int Span = 50;
        playerTimer = (playerTimer + 1) % (4 * Span);
        var animeSpanTime = playerTimer / Span;
        playerAnime = (animeSpanTime == 3) ? 1 : animeSpanTime;
    }


    private int _actionSpan = 100;

    private bool UpdatePlayerWalkTimer(out float lerp)
    {
        playerWalkTimer = playerWalkTimer + 1;
        lerp = playerWalkTimer * 1.0f / _actionSpan;
        
        if (_actionSpan < playerWalkTimer)
        {
            playerWalkTimer = 0;
            return true;
        }

        return false;
    }

    private void Start()
    {
        Sprite[] sprites = new[]
        {
            _greenSprite,
            _holeSprite,
            _goalSprite,
            _treasureSprite,
            _treasureSprite2,
        };
            
        _textures = sprites.Select((delegate(Sprite sprite)
        {
            var croppedTexture = new Texture2D( (int)sprite.rect.width, (int)sprite.rect.height );
            var pixels = sprite.texture.GetPixels(  (int)sprite.rect.x, 
                (int)sprite.rect.y, 
                (int)sprite.rect.width, 
                (int)sprite.rect.height );
            croppedTexture.SetPixels( pixels );
            croppedTexture.Apply();
            return croppedTexture;
        })).ToArray();
        
        _playerTextures = _playerSprites.Select((delegate(Sprite sprite)
        {
            var croppedTexture = new Texture2D( (int)sprite.rect.width, (int)sprite.rect.height );
            var pixels = sprite.texture.GetPixels(  (int)sprite.rect.x, 
                (int)sprite.rect.y, 
                (int)sprite.rect.width, 
                (int)sprite.rect.height );
            croppedTexture.SetPixels( pixels );
            croppedTexture.Apply();
            return croppedTexture;
        })).ToArray();
    }

    public class MapData
    {
        public int Width;
        public int Height;
        public int[] Data;

        public MapData(int width, int height, int[] data)
        {
            Width = width;
            Height = height;
            Data = data;
        }

        public MapData(MapData mapData)
        {
            Width = mapData.Width;
            Height = mapData.Height;
            Data = new int[mapData.Data.Length];
            mapData.Data.CopyTo(Data, 0);
        }

        public int FindPlayerStartIndex()
        {
            return Data.ToList().IndexOf(-1);
        }

        public int FindGoalIndex()
        {
            return Data.ToList().IndexOf(1);
        }
    }
    
    private MapData _mapData = null;

    public void SetMapData(MapData mapData)
    {
        _mapData = mapData;
        playerIndex = _mapData.FindPlayerStartIndex();
        playerDirection = 0;
        goalIndex = _mapData.FindGoalIndex();
        _algorithmList = null;
    }

    private List<string> _algorithmList = null;

    public void PlayStart(List<string> algorithmList)
    {
        _algorithmList = algorithmList;
        playerWalkTimer = 0;
        current = PlayState.Playing;
    }

    [SerializeField]
    private Font _font;

    void OnGUI()
    {
        if (_textures == null)
        {
            return;
        }

        if (_mapData == null)
        {
            return;
        }

        UpdatePlayerTimer();
        DrawMap();
        
        if (_algorithmList == null)
        {
            GUI.DrawTexture(GetMapTipRect(playerIndex), _playerTextures[GetPlayerAnimeIndex()]);
        }
        else
        {
            var message = (current == PlayState.Playing) ? "プレイ中" : (current == PlayState.Success) ? "クリア！" : "ざんねん";
            GUI.Label(new Rect(Screen.width * 0.3f, 0, Screen.width * 0.5f, Screen.height * 0.05f), message, new GUIStyle
            {
                fontSize = (int)(Screen.height * 0.05f),
                font = _font
            });
            
            if (_algorithmList.Count == 0 || current != PlayState.Playing)
            {
                GUI.DrawTexture(GetMapTipRect(playerIndex), _playerTextures[GetPlayerAnimeIndex()]);
                return;
            }

            if (UpdatePlayerWalkTimer(out float lerp))
            {
                if (_algorithmList[0] == "TurnLeft")
                {
                    playerDirection = (playerDirection + 4 - 1) % 4;
                }
                if (_algorithmList[0] == "TurnRight")
                {
                    playerDirection = (playerDirection + 1) % 4;
                }
                if (_algorithmList[0] == "Treasure")
                {
                    if (_mapData.Data[playerIndex] == 3)
                    {
                        _mapData.Data[playerIndex] = 4;
                    }
                    else
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                }
                if (_algorithmList[0] == "Walk")
                {
                    if (!TryGetMovedIndex(_mapData, playerIndex, playerDirection, 1, out int nowIndex))
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                    else
                    {
                        playerIndex = nowIndex;
                    }
                    
                    if (playerIndex < 0 || _mapData.Data.Length <= playerIndex)
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                    else if (_mapData.Data[playerIndex] == 2)
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                }
                if (_algorithmList[0] == "Jump")
                {
                    if (!TryGetMovedIndex(_mapData, playerIndex, playerDirection, 2, out int nowIndex))
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                    else
                    {
                        playerIndex = nowIndex;
                    }

                    if (playerIndex < 0 || _mapData.Data.Length <= playerIndex)
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                    else if (_mapData.Data[playerIndex] == 2)
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                }

                _algorithmList.RemoveAt(0);

                if (_algorithmList.Count == 0)
                {
                    if (playerIndex < 0 || _mapData.Data.Length <= playerIndex)
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                    else if (_mapData.Data[playerIndex] == 1)
                    {
                        // 勝利判定
                        current = PlayState.Success;
                    }
                    else
                    {
                        // 敗北判定
                        current = PlayState.Failled;
                    }
                    return;
                }
                _actionSpan = 30;
                return;
            }

            var nextPlayerIndex = playerIndex;
            if (_algorithmList[0] == "Walk")
            {
                _actionSpan = 100;
                if (!TryGetMovedIndex(_mapData, playerIndex, playerDirection, 1, out nextPlayerIndex))
                {
                    nextPlayerIndex = playerIndex;
                }
            }
            if (_algorithmList[0] == "Jump")
            {
                _actionSpan = 30;
                if (!TryGetMovedIndex(_mapData, playerIndex, playerDirection, 2, out nextPlayerIndex))
                {
                    nextPlayerIndex = playerIndex;
                }
            }
            

            var rect = new Rect(
                    Vector2.Lerp(GetMapTipPosition(playerIndex), GetMapTipPosition(nextPlayerIndex), lerp),
                    GetCellSize());
            
            GUI.DrawTexture(rect, _playerTextures[GetPlayerAnimeIndex()]);
        }
    }

    private void DrawMap()
    {
        for (int i = 0; i < _mapData.Width * _mapData.Height; i++)
        {
            DrawMapTip(i);
        }

    }

    private static bool TryGetMovedIndex(MapData mapData, int index, int direction, int distanxe, out int nextIndex)
    {
        nextIndex = index;
        
        if (direction == 0)
        {
            nextIndex = index + mapData.Width * +distanxe;
        }
        if (direction == 1)
        {
            nextIndex = index + 1 * -distanxe;

            if (nextIndex / mapData.Width != index / mapData.Width)
            {
                return false;
            }
        }
        if (direction == 2)
        {
            nextIndex = index + mapData.Width * -distanxe;
        }
        if (direction == 3)
        {
            nextIndex = index + 1 * +distanxe;

            if (nextIndex / mapData.Width != index / mapData.Width)
            {
                return false;
            }
        }

        if (nextIndex < 0 || mapData.Data.Length <= nextIndex)
        {
            return false;
        }

        return true;
    }
    
    private void DrawMapTip(int index)
    {
        GUI.DrawTexture(GetMapTipRect(index), _textures[0]);
        
        
        if (_mapData.Data[index] == 1)
        {
            GUI.DrawTexture(GetMapTipRect(index), _textures[2]);
        }
        if (_mapData.Data[index] == 2)
        {
            GUI.DrawTexture(GetMapTipRect(index), _textures[1]);
        }
        if (_mapData.Data[index] == 3)
        {
            GUI.DrawTexture(GetMapTipRect(index), _textures[3]);
        }
        if (_mapData.Data[index] == 4)
        {
            GUI.DrawTexture(GetMapTipRect(index), _textures[4]);
        }
    }
    
    private Rect GetMapTipRect(int index)
    {
        return new Rect(GetMapTipPosition(index), GetCellSize());
    }

    private Vector2 GetMapTipPosition(int index)
    {
        int x = index % _mapData.Width;
        int y = index / _mapData.Width;

        var f = GetCenter() + new Vector2((x - _mapData.Width / 2) * GetCellSize().x, (y - _mapData.Height / 2) * GetCellSize().y);
        return f;
    }

    private Vector2 GetCenter()
    {
        return new Vector2(Screen.width * 0.3f + (Screen.width * 0.7f * 0.5f), Screen.height * 0.8f * 0.5f);
    }

    private Vector2 GetCellSize()
    {
        return new Vector2(Screen.height * 0.08f, Screen.height * 0.08f);
    }
}
