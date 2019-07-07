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
    private int playerDirection = 0; // 0: down, +1 : turn right
    private int playerAnime = 0;
    private int playerTimer = 0;

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
    }

    private MapData _mapData = null;

    void OnGUI()
    {
        if (_textures == null)
        {
            return;
        }

        if (_mapData == null)
        {
            _mapData = MapDatabase.LoadMapDataByStageId(5);
        }

        UpdatePlayerTimer();
        DrawMap();
    }

    private void DrawMap()
    {
        for (int i = 0; i < _mapData.Width * _mapData.Height; i++)
        {
            DrawMapTip(i);
        }
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
        
        // GUI.DrawTexture(GetMapTipRect(mapData, index), _playerTextures[GetPlayerAnimeIndex()]);
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
