// PlayerAudioController.cs - Attach to the Player
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Required Components")]
    [Tooltip("Assign the singular Tilemap that contains all your ground tiles.")]
    [SerializeField] private Tilemap groundTilemap;

    private void OnEnable()
    {
        PlayerController.OnPlayerStep += HandlePlayerStep;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerStep -= HandlePlayerStep;
    }

    private void HandlePlayerStep()
    {
        if (groundTilemap == null) return;

        Vector3 feetOffset = new Vector3(0, -0.6f, 0);
        Vector3Int cellPosition = groundTilemap.WorldToCell(transform.position + feetOffset);

        SolidRuleTile tile = groundTilemap.GetTile<SolidRuleTile>(cellPosition);

        if (tile != null)
        {
            switch (tile.surfaceType)
            {
                case SurfaceType.Grass:
                    AudioManager.Instance.PlaySFX(SoundType.WalkGrass);
                    break;
                case SurfaceType.Stone:
                    AudioManager.Instance.PlaySFX(SoundType.WalkStone);
                    break;
                case SurfaceType.None:
                default:
                    break;
            }
        }
    }
}