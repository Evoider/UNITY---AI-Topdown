// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 13/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }
    [SerializeField] private float _damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}