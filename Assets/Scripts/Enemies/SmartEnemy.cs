using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : Enemy {

    private FSM<SmartEnemy> fsm;
    public float rangeToAttackFrom;
    public float pulseSize;
    public float pulseTime;
    public int numPulses;
    public float attackSpeed;
    public float fleeDistance;
    public float fleeSpeed;

    protected override void Initialize()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/smartEnemy");
        AudioClip deathClip = Resources.Load("Sounds/sfx_deathscream_robot1") as AudioClip;
        SetDeathClip(deathClip);
        fsm = new FSM<SmartEnemy>(this);
        fsm.TransitionTo<Seeking>();
        base.Initialize();
    }

    protected override void SetValues()
    {
        base.SetValues();
        approachSpeed = 3f;
        rangeToAttackFrom = 10f;
        pulseSize = 2f;
        pulseTime = 0.5f;
        numPulses = 5;
        attackSpeed = 20f;
        startingHealth = 5;
        fleeDistance = 15f;
        fleeSpeed = 15f;
    }

    protected override void Move()
    {
    }

    protected override void SpecialUpdate()
    {
        fsm.Update();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Services.EventManager.Fire(new AttackPrepInterrupted(this));
    }

    protected override void Die()
    {
        base.Die();
    }

    private class SmartEnemyState : FSM<SmartEnemy>.State
    {
    }

    private class Seeking : SmartEnemyState
    {
        public override void Update()
        {
            Context.ApproachPlayer();
            if (Vector2.Distance(Services.GameManager.player.transform.position, Context.transform.position) < Context.rangeToAttackFrom)
            {
                TransitionTo<AttackPreparation>();
            }
        }
    }

    private class AttackPreparation : SmartEnemyState
    {
        private int pulsesFinished;
        private float timeSincePulseStart;
        private Vector3 initialSize;

        public override void OnEnter()
        {
            initialSize = Context.gameObject.transform.localScale;
            timeSincePulseStart = 0;
            pulsesFinished = 0;
            Services.EventManager.Register<AttackPrepInterrupted>(Interrupt);
        }

        public override void Update()
        {
            if (timeSincePulseStart <= Context.pulseTime)
            {
                Context.gameObject.transform.localScale = Vector3.Lerp(initialSize, Context.pulseSize * initialSize, 
                    Easing.QuadEaseOut(timeSincePulseStart / Context.pulseTime));
                timeSincePulseStart += Time.deltaTime;
            }
            else
            {
                pulsesFinished += 1;
                timeSincePulseStart = 0;
            }

            if (pulsesFinished == Context.numPulses)
            {
                TransitionTo<Attack>();
            }
        }

        private void Interrupt(AttackPrepInterrupted e)
        {
            if (e.enemy = Context)
            {
                TransitionTo<Fleeing>();
            }
        }

        public override void OnExit()
        {
            Context.gameObject.transform.localScale = initialSize;
            Services.EventManager.Unregister<AttackPrepInterrupted>(Interrupt);
        }
    }

    private class Attack : SmartEnemyState
    {
        private Vector3 target;
        private Vector3 startPos;
        private float timeElapsed;
        private float calculatedFlightTime;

        public override void OnEnter()
        {
            target = Services.GameManager.player.transform.position;
            startPos = Context.transform.position;
            calculatedFlightTime = Vector3.Distance(startPos, target) / Context.attackSpeed;
            timeElapsed = 0;
        }

        public override void Update()
        {
            if (Context.transform.position == target)
            {
                TransitionTo<Seeking>();
            }
            else
            {
                timeElapsed += Time.deltaTime;
                Context.transform.position = Vector3.Lerp(startPos, target, timeElapsed / calculatedFlightTime);
            }
        }
    }

    private class Fleeing : SmartEnemyState
    {
        private Vector3 fleeTarget;
        private Vector3 startPos;
        private float timeElapsed;
        private float calculatedFlightTime;

        public override void OnEnter()
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector3 playerLocation = Services.GameManager.player.transform.position;
            Vector3 fleeDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            fleeTarget = playerLocation + (Context.fleeDistance * fleeDirection);
            startPos = Context.transform.position;
            timeElapsed = 0;
            calculatedFlightTime = Vector3.Distance(startPos, fleeTarget) / Context.fleeSpeed;
        }

        public override void Update()
        {
            if (Context.transform.position == fleeTarget)
            {
                TransitionTo<Seeking>();
            }
            else
            {
                timeElapsed += Time.deltaTime;
                Context.transform.position = Vector3.Lerp(startPos, fleeTarget, timeElapsed / calculatedFlightTime);
            }
        }
    }
}
