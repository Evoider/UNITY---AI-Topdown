// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 13/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    #region Fields
    public bool IsNpc => _isNpc;
    [SerializeField] protected bool _isNpc = true;

    public float Health => _health;
    [SerializeField] protected float _health;
    public float MaxHealth => _maxHealth;
    [SerializeField] protected float _maxHealth;

    public float Damage => _damage;
    [SerializeField] protected float _damage;

    public float MovementSpeed => _movementSpeed;
    [SerializeField] protected float _movementSpeed;

    public float AttackSpeed => _attackSpeed;
    [SerializeField] protected float _attackSpeed;
    public float AttackRange => _attackRange;
    [SerializeField] protected float _attackRange;

    public float VisionRange => _visionRange;
    [SerializeField] protected float _visionRange;

    [SerializeField] protected GameObject _fxHit;

    public bool IsAlive => _health > 0;
    public bool IsDead => _health <= 0;
    #endregion Fields

    #region Events
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnHit;
    #endregion Events

    public virtual void Heal(float healAmount)
    {
        _health = Mathf.Clamp(_health + healAmount, 0, _maxHealth);

        OnHealthChanged?.Invoke(Health);
    }

    public virtual void TakeDamage(float damage)
    {
        OnHit();
        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
        
        OnHealthChanged?.Invoke(Health);
        OnHit?.Invoke();
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public virtual void Attack(Entity target)
    {
        target.TakeDamage(_damage);
    }

    public void OnHit()
    {
        if (IsAlive && _fxHit != null)
        {
            GameObject go = Instantiate(_fxHit, transform.position, Quaternion.identity);
            // decrease fx siez because they are too big
            go.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }
}