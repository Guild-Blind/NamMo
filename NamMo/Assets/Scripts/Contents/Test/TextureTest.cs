using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTest : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // �浹�ϴ� ��������Ʈ�� SpriteRenderer
    private Texture2D texture;

    private int[] _dx = new int[4] { 0, 1, 0, -1 };
    private int[] _dy = new int[4] { 1, 0, -1, 0 };
    private Color[,] _colors;
    private bool[,] _visited;
    void Start()
    {
        texture = spriteRenderer.sprite.texture;

        // �ؽ�ó�� �б� �����ؾ� �մϴ�.
        if (!texture.isReadable)
        {
            Debug.LogError("Texture is not readable. Check 'Read/Write Enabled' in the Texture Import Settings.");
            return;
        }
        _colors = new Color[texture.width, texture.height];

        // �ȼ����� ���� ����
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                _colors[x, y] = texture.GetPixel(x, y);
                if (texture.GetPixel(x, y).a == 0) continue;
                Color newColor = Color.red; // ���ϴ� ������ ����
                texture.SetPixel(x, y, newColor);
            }
        }

        // ���� ������ �ؽ�ó�� ����
        texture.Apply();
    }
    private void OnApplicationQuit()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, _colors[x, y]);
            }
        }
        texture.Apply();
    }
}
