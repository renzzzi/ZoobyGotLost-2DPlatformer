using UnityEngine;

public enum SurfaceType
{ 
    None,
    Grass, Wood,
    Stone, Gravel,
    Metal, Rubber
}


[CreateAssetMenu(fileName = "New SolidRuleTile", menuName = "Tiles/Solid Rule Tile")]
public class SolidRuleTile : RuleTile
{
    public SurfaceType surfaceType;
}
