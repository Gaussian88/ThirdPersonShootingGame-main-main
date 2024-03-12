using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwatAI : MonoBehaviour
{
    public enum State { PATROL=1,TRACE,ATTACK,DIE}
    public State state = State.PATROL;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform swatTr;
    [SerializeField] private SwatAgent swatAgent;
    [SerializeField] private SwatFire swatFire;

    public float attackDist = 5f;
    public float traceDist = 10f;
    private WaitForSeconds ws;
    public bool isDie = false;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("FowardSpeed");
    private readonly int hashDie = Animator.StringToHash("DieTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDieTrigger");
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerTr = GameObject.FindWithTag("Player").transform;
        swatTr = GetComponent<Transform>();
        swatAgent = GetComponent<SwatAgent>();
        swatFire = GetComponent<SwatFire>();
        ws = new WaitForSeconds(3f);
    }
    private void OnEnable()
    {
        Damage.OnPlayerDie += OnPlayerDie;
        BarrelCtrl.OnEnemyDie += Die;
        animator.SetFloat(hashOffset, Random.Range(0.3f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 2f));
        StartCoroutine(CheckState());
        StartCoroutine(StateAction());
    }
    IEnumerator CheckState()
    {
        while(!isDie)
        {
            float dist = (playerTr.position - swatTr.position).magnitude;
            if (dist <= attackDist)
                state = State.ATTACK;
            else if (dist <= traceDist)
                state = State.TRACE;
            else
                state = State.PATROL;
            yield return ws;
        }
    }
    IEnumerator StateAction()
    {
        while (!isDie)
        {
            yield return ws;
            switch(state)
            {
                case State.PATROL:
                    swatFire.isFire = false;
                    swatAgent.IsPatrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    swatFire.isFire = false;
                    swatAgent.tracetarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    if (isDie) yield break;
                    swatAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if (swatFire.isFire == false)
                        swatFire.isFire = true;
                    break;
                case State.DIE:
                    
                    Die();
                    break;
            }

        }
    }
    void OnPlayerDie()
    {
        swatFire.isFire = false;
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }
    public void Die()
    {
        if (isDie) return;
        swatAgent.Stop();
        isDie = true;
        swatFire.isFire = false;
        state = State.DIE;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(PushPool());
        GameManager.Instance.KillScoreNumber(1);
    }
    IEnumerator PushPool()
    {
        yield return ws;

        state = State.PATROL;
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false; //물리 있음음
        GetComponent<CapsuleCollider>().enabled = true;// 콜라이더 활성화
        this.gameObject.SetActive(false);

    }
    void Update()
    {
        animator.SetFloat(hashSpeed, swatAgent.speed);

    }
    private void OnDisable()
    {
        Damage.OnPlayerDie -= OnPlayerDie;
        BarrelCtrl.OnEnemyDie -= Die;
    }
}
