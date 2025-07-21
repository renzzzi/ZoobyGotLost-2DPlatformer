using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New SolidTile", menuName = "Smart Tiles/SolidTile")]
public class SolidTile : Tile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        tileData.colliderType = Tile.ColliderType.Sprite;
    }
}