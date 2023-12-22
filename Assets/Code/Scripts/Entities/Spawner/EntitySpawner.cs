// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 18/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntitySpawner : MonoBehaviour
{
    private BoxCollider2D _roomCollider;
    private Tilemap _floorTilemap;

    public List<CapsuleCollider2D> PatrolAreas => _patrolAreas;
    private List<CapsuleCollider2D> _patrolAreas;

    private void Awake()
    {
        if (_waves == null)
            return;

        if (_roomCollider == null)
            _roomCollider = GetComponentInParent<BoxCollider2D>();
        if (_floorTilemap == null)
            _floorTilemap = GameObject.Find("Floor").GetComponent<Tilemap>();
        if (_patrolAreas == null)
            _patrolAreas = new List<CapsuleCollider2D>(transform.parent.GetComponentsInChildren<CapsuleCollider2D>());
    }

    public Vector3 GetRandomPositionInRoom()
    {
        while (true)
        {
            var roomBounds = _roomCollider.bounds;
            var x = Random.Range(roomBounds.min.x, roomBounds.max.x);
            var y = Random.Range(roomBounds.min.y, roomBounds.max.y);

            Vector3 value = new Vector3(x, y, 9);

            bool IsValidPosition()
            {
                var tile = _floorTilemap.GetTile(_floorTilemap.WorldToCell(value));
                Player player = GameManager.Instance.Player;
                // Check also if the player is not too close
                if (player == null)
                    return tile != null;
            
                var distance = Vector3.Distance(player.transform.position, value);
                
                
                return tile != null && distance > 2;
            }

            if (!IsValidPosition()) continue;

            return value;
        }
    }

    public bool IsEnded => _currentWaveIndex > _waves.Count;

    public List<SOWave> Waves => _waves;
    [SerializeField, InlineEditor] private List<SOWave> _waves;

    public int CurrentWaveIndex
    {
        get => _currentWaveIndex;
        set => _currentWaveIndex = value;
    }

    private int _currentWaveIndex = 0;

    private Coroutine _spawnRoutine;

    public bool IsInFight => _spawnRoutine != null;

    public void SpawnWave()
    {
        _spawnRoutine = StartCoroutine(RoomWaves());

        IEnumerator RoomWaves()
        {
            foreach (var w in _waves)
            {
                yield return w.RunWave(this);
            }

            _spawnRoutine = null;
        }
    }
}