using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [Header("Tile Sprites")]
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite flaggedTile;
    [SerializeField] private List<Sprite> clickedTiles;
    [SerializeField] private Sprite mineTile;
    [SerializeField] private Sprite mineWrongTile;
    [SerializeField] private Sprite mineHitTile;

    private SpriteRenderer spriteRenderer;
    public bool flaged = false;
    public bool active = true;
    public bool isMine = false;
    public int mineCount = 0;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Left click reveals the content
                ClickedTile();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                // Right click flags the tile or removes the flag
                flaged = !flaged;
                spriteRenderer.sprite = flaged ? flaggedTile : unclickedTile;
            }
        }
    }

    public void ClickedTile() {
        if (active & !flaged) {
            active = false;

            if (isMine)
            {
                spriteRenderer.sprite = mineHitTile;
            }
            else
            {
                spriteRenderer.sprite = clickedTiles[mineCount];
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
