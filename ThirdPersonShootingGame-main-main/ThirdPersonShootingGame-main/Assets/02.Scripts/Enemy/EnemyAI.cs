using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { PATROL=1,TRACE,ATTACK,DIE}
    public State state = State.PATROL;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    [SerializeField] private MoveAgent moveAgent;
    [SerializeField] private EnemyFire enemyFire;
    public float attackDist = 5f;
    public float traceDist = 10f;
    public bool isDie = false;
    private WaitForSeconds ws;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("ForwardSpeed");
    private readonly int hashDie = Animator.StringToHash("DieTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashOffset = Animator.StringToHash("Offset");

    void Awake()
    {
        enemyFire = GetComponent<EnemyFire>();
        moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        tr = GetComponent<Transform>();
        ws = new WaitForSeconds(3f);
    }
    void OnEnable() //������Ʈ�� Ȱ��ȭ �ɶ� �ڵ� ȣ�� // ������Ʈ Ǯ��
    {
        animator.SetFloat(hashOffset, Random.Range(0.3f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 2f));
        StartCoroutine(EnemyState()); //�Ÿ��� �� ���� ���¸� �˷��ش�.
        StartCoroutine(EnemyAction());
    }
    IEnumerator EnemyState()
    {
        while (!isDie) //��� ������ ���̸� �ݺ� �ؼ� �������� ���� �ҷ��� 
        {
            float dist = Vector3.Distance(playerTr.position, tr.position);
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator EnemyAction()
    {
       
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.isPtrolling = true;
                    animator.SetBool(hashMove,true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if(enemyFire.isFire == false)
                       enemyFire.isFire= true;
                    break;
                case State.DIE:
                    Die();

                    break;
            }

        }

    }

    public void Die()
    {
        enemyFire.isFire = false;
        state = State.DIE;
        isDie = true;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        GetComponent<Rigidbody>().isKinematic = true; //���� ����
        GetComponent<CapsuleCollider>().enabled = false;// �ݶ��̴� ��Ȱ��ȭ
        moveAgent.Stop();
        StartCoroutine(PushPool());
    }
    IEnumerator PushPool()
    {
        yield return ws;

        state = State.PATROL;
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false; //���� ������
        GetComponent<CapsuleCollider>().enabled = true;// �ݶ���Ȱ��ȭ
        this.gameObject.SetActive(false);
       
    }
    void Start()
    {

    }
    
    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);

    }
    void OnDisable() //������Ʈ�� ��Ȱ��ȭ �ɶ� �ڵ� ȣ��
    {

    }
}