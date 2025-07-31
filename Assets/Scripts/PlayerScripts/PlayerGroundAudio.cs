using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerGroundAudio : MonoBehaviour
{
    [Tooltip("Assign the singular Tilemap that contains all your ground tiles.")]
    [SerializeField] private Tilemap groundTilemap;

    private void OnEnable()
    {
        PlayerController.OnGroundHit += HandleOnGroundHit;
        PlayerController.OnPlayerStep += HandlePlayerStep;
    }

    private void OnDisable()
    {
        PlayerController.OnGroundHit -= HandleOnGroundHit;
        PlayerController.OnPlayerStep -= HandlePlayerStep;

    }

    private void HandleOnGroundHit()
    {
        SurfaceType surface = GetSurfaceTypeBelowPlayer();
    
        switch (surface)
        {
            case SurfaceType.Grass:
                AudioManager.Instance.PlaySFX(SoundType.GroundHitGrass);
                break;
            case SurfaceType.Wood:
                AudioManager.Instance.PlaySFX(SoundType.GroundHitWood);
                break;
            case SurfaceType.Stone:
                AudioManager.Instance.PlaySFX(SoundType.GroundHitStone);
                break;
            case SurfaceType.Gravel:
                AudioManager.Instance.PlaySFX(SoundType.GroundHitGravel);
                break;
            case SurfaceType.Metal:
                AudioManager.Instance.PlaySFX(SoundType.GroundHitMetal);
                break;
            case SurfaceType.Rubber:
                AudioManager.Instance.PlaySFX(SoundType.GroundHitRubber);
                break;
            case SurfaceType.None:
            default:
                break;
        }
    }

    private void HandlePlayerStep()
    {
        SurfaceType surface = GetSurfaceTypeBelowPlayer();

        switch (surface)
        {
            case SurfaceType.Grass:
                AudioManager.Instance.PlaySFX(SoundType.WalkGrass);
                break;
            case SurfaceType.Wood:
                AudioManager.Instance.PlaySFX(SoundType.WalkWood);
                break;
            case SurfaceType.Stone:
                AudioManager.Instance.PlaySFX(SoundType.WalkStone);
                break;
            case SurfaceType.Gravel:
                AudioManager.Instance.PlaySFX(SoundType.WalkGravel);
                break;
            case SurfaceType.Metal:
                AudioManager.Instance.PlaySFX(SoundType.WalkMetal);
                break;
            case SurfaceType.Rubber:
                AudioManager.Instance.PlaySFX(SoundType.WalkRubber);
                break;
            case SurfaceType.None:
            default:
                break;
        }
    }

    private SurfaceType GetSurfaceTypeBelowPlayer()
    {
        if (groundTilemap == null)
        {
            return SurfaceType.None;
        }

        // Defines the width of the player's feet
        float footWidth = 0.3f;

        Vector3[] checkPoints = new Vector3[3];
        checkPoints[0] = transform.position + new Vector3(0, -0.6f, 0);         // Center
        checkPoints[1] = transform.position + new Vector3(footWidth, -0.6f, 0);  // Right foot
        checkPoints[2] = transform.position + new Vector3(-footWidth, -0.6f, 0); // Left foot

        foreach (var point in checkPoints)
        {
            Vector3Int cellPosition = groundTilemap.WorldToCell(point);
            SolidRuleTile tile = groundTilemap.GetTile<SolidRuleTile>(cellPosition);

            if (tile != null)
            {
                return tile.surfaceType;
            }
        }

        return SurfaceType.None;
    }
}