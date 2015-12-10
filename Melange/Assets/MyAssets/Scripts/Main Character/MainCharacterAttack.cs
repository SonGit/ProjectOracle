using UnityEngine;
using System.Collections;

public partial class MainCharacter
{
    public ParticleSystem _slashParticle;
    private bool isAttacking;
    public bool _isAttacking
    {
        get
        {
            return isAttacking;
        }

        set
        {
            isAttacking = value;
            _animator.SetBool("isAttacking", value);
            _animator.SetInteger("AttackAnimType",Random.Range(0,2));
        }

    }

    public float _attackLength = 1f;

    public void OnAttack(GameObject gameObject)
    {
        if (_isAttacking)
            return;

        _isAttacking = true;
        StartCoroutine(WaitForEndOfAttackAnimation(gameObject));
    }

    IEnumerator WaitForEndOfAttackAnimation(GameObject go)
    {
        if(_isAttacking)
        {

            Enemy enemy = go.GetComponent<Enemy>();
            Vector3 enemyPosition = enemy.transform.position;

            if (enemy != null)
            {
                enemy.MarkedForDeath();
                enemy.Stop();
            }
            yield return new WaitForSeconds(_attackLength/2);

            if(enemy != null)
            {
                //_slashParticle.Play();
                enemy.Hit();
            }

            yield return new WaitForSeconds(2f);
            _isAttacking = false;

            if (_pendingDash)
                _pendingDash = false;

            LookAt(enemyPosition);
        }
    }


}
