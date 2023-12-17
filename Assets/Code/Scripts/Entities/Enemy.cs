// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 13/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using System.Linq;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected SOEntity _baseData;

    private void Awake()
    {
        if (_baseData == null)
        {
            // NOT TESTED
            if (GameManager.Instance.EntityList.List.ContainsKey(name))
                _baseData = GameManager.Instance.EntityList.List[name];
            else
                _baseData = GameManager.Instance.EntityList.List.First().Value;
        }

        _health = _baseData.MaxHealth;
        _maxHealth = _baseData.MaxHealth;
        _damage = _baseData.Damage;
        _movementSpeed = _baseData.MovementSpeed;
        _attackSpeed = _baseData.AttackSpeed;
        _attackRange = _baseData.AttackRange;
        _visionRange = _baseData.VisionRange;
    }

    public float DistFromPlayer => _distFromPlayer;
    protected float _distFromPlayer;

    private Vector3 GetPlayerPos()
    {
        return GameManager.Instance.Player.position;
    }

    protected float CalculateDistFromPlayer()
    {
        return Vector2.Distance(GetPlayerPos(), transform.position);
    }
}