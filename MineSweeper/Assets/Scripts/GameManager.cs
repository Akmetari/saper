using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private int width;
    private int height;
    private int numMines;

    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform tilePrefab;

    private List<List<Tile>> tiles = new List<List<Tile>>();

    private readonly float tileSize = 0.5f;

    public void CreateGameBoard(int width, int height, int numMines) { 
        this.width = width;
        this.height = height;
        this.numMines = numMines;

        List<Tile> rowList = new();
        for (int row = 0; row < height; row++) {
            rowList = new();
            for (int col = 0; col < width; col++) {
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent= gameHolder;

                float xIndex= col - ((width-1)/2.0f);
                float yIndex= row - ((height-1)/2.0f);
                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);

                Tile tile = tileTransform.GetComponent<Tile>();
                tile.y = row;
                tile.x = col;
                tile.game = this;
                rowList.Add(tile);
            }
            tiles.Add(rowList);
        }

    }

    public void ResetGameState(){
       
        for (int i = 0; i < numMines; i++) { 

            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            tiles[y][x].isMine = true;
        }

        for (int i = 0; i < (width * height); i++) {
            Tile t = tiles[(i / width)][i % width];
            t.mineCount = HowManyMines(i);
        }

        

    }

    public List<(int, int)> GetNeighbours(int x, int y)
    {
        List<(int, int)> neighbours = new List<(int, int)>();

        if (x - 1 >= 0) neighbours.Add((y, x - 1));
        if (x + 1 < width) neighbours.Add((y, x + 1));

        if (y - 1 >= 0)
        {
            if (x - 1 >= 0) neighbours.Add((y - 1, x - 1));
            neighbours.Add((y - 1, x));
            if (x + 1 < width) neighbours.Add((y - 1, x + 1));
        }

        if (y + 1 < height)
        {
            if (x - 1 >= 0) neighbours.Add((y + 1, x - 1));
            neighbours.Add((y + 1, x));
            if (x + 1 < width) neighbours.Add((y + 1, x + 1));
        }
        return neighbours;
    }

    private Tile GetTile(int tileIndex) {
        int x = tileIndex % width;
        int y = tileIndex / width;
        return tiles[y][x];
    }

    private Tile XYtoTile(int x, int y) {
        return tiles[y][x];
    }

    public List<Tile> XYListToTiles(List<(int, int)> xyList) {
        List<Tile> tiles = new(); 
        foreach ((int, int) xy in xyList) {
            tiles.Add(XYtoTile(xy.Item1, xy.Item2));
        }
        return tiles;
    }

    private int HowManyMines(int tileIndex) {
        int count = 0;
        Tile tile = GetTile(tileIndex);

        foreach ((int,int) pos in GetNeighbours(tile.x, tile.y)) {
            if (tiles[pos.Item1][pos.Item2].isMine) count++;
        }
        return count;
    
    }

    public void ClickNeighbours(Tile tile) {
        foreach ((int, int) pos in GetNeighbours(tile.x, tile.y))
        {
            tiles[pos.Item1][pos.Item2].ClickedTile();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGameBoard(10, 10, 10);
        ResetGameState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
