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

    [SerializeField] private Sprite[] _playerSprites;
    
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

    void OnGUI()
    {
        if (_textures == null)
        {
            return;
        }

        UpdatePlayerTimer();
        
        var startCamera = new Vector2(Screen.width * 0.3f, 0);
        var cellSize = new Vector2(Screen.height * 0.08f, Screen.height * 0.08f);
            
        GUI.DrawTexture(new Rect(startCamera, cellSize), _textures[0]);
        GUI.DrawTexture(new Rect(startCamera, cellSize), _textures[1]);
        GUI.DrawTexture(new Rect(startCamera, cellSize), _textures[2]);
        GUI.DrawTexture(new Rect(startCamera, cellSize), _playerTextures[GetPlayerAnimeIndex()]);
    }
}
