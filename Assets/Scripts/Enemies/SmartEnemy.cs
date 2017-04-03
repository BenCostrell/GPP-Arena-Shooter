using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : Enemy {

    //private FSM<SmartEnemy> fsm;
    private Tree<SmartEnemy> behaviorTree;
    public float rangeToAttackFrom;
    public float pulseSize;
    public float pulseTime;
    public int numPulses;
    public float attackSpeed;
    public float fleeDistance;
    public float fleeSpeed;
    public enum State { Seeking, Preparing, ReadyToAttack, ReadyToPrepare, Attacking, Fleeing};
    public State currentState;
    public bool hitThisFrame;
    public float closeEnough;
    public Vector3 defaultSize;

    protected override void Initialize()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/smartEnemy");
        AudioClip deathClip = Resources.Load("Sounds/sfx_deathscream_robot1") as AudioClip;
        SetDeathClip(deathClip);
        /*fsm = new FSM<SmartEnemy>(this);
        fsm.TransitionTo<Seeking>();*/

        DefineBehaviorTree();

        base.Initialize();
    }

    void DefineBehaviorTree()
    {
        behaviorTree = new Tree<SmartEnemy>(new Selector<SmartEnemy>(
            new Sequence<SmartEnemy>(
                new IsFleeing(),
                new Flee()),
            new Sequence<SmartEnemy>(
                new IsAttackingOrReadyToAttack(),
                new AttackPlayer()),
            new Sequence<SmartEnemy>(
                new IsPreparingOrReadyToPrepareAttack(),
                new Selector<SmartEnemy>(
                    new Sequence<SmartEnemy> (
                        new HitThisFrame(),
                        new Flee()),
                    new PrepareToAttack())),
            new Sequence<SmartEnemy>(
                new IsPlayerInRange(),
                new StartAttackPrep()),
            new Seek()
            ));
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
        closeEnough = 0.1f;
        defaultSize = transform.localScale;
    }

    protected override void Move()
    {
    }

    public void MoveTowardsPlayer()
    {
        ApproachPlayer();
    }

    protected override void SpecialUpdate()
    {
        //fsm.Update();
        if ((currentState != State.Preparing) && hitThisFrame)
        {
            hitThisFrame = false;
        }
        behaviorTree.Update(this);
        Debug.Log(currentState.ToString());
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        hitThisFrame = true;
        //Services.EventManager.Fire(new AttackPrepInterrupted(this));
    }

    protected override void Die()
    {
        base.Die();
    }

    //////////////////
    // NODES
    //////////////////

    //////////////////
    // CONDITIONS
    //////////////////
    
    private class IsPlayerInRange : Node<SmartEnemy>
    {
        public override bool Update(SmartEnemy context)
        {
            return Vector3.Distance(Services.GameManager.player.transform.position, context.transform.position) < context.rangeToAttackFrom;
        }
    }

    private class IsPreparingOrReadyToPrepareAttack : Node<SmartEnemy>
    {
        public override bool Update(SmartEnemy context)
        {
            return (context.currentState == SmartEnemy.State.ReadyToPrepare || context.currentState == SmartEnemy.State.Preparing);
        }
    }

    private class IsAttackingOrReadyToAttack : Node<SmartEnemy>
    {
        public override bool Update(SmartEnemy context)
        {
            return (context.currentState == SmartEnemy.State.ReadyToAttack || context.currentState == SmartEnemy.State.Attacking);
        }
    }

    private class IsFleeing : Node<SmartEnemy>
    {
        public override bool Update(SmartEnemy context)
        {
            return context.currentState == SmartEnemy.State.Fleeing;
        }
    }

    private class HitThisFrame : Node<SmartEnemy>
    {
        public override bool Update(SmartEnemy context)
        {
            return context.hitThisFrame;
        }
    }

    //////////////////
    // ACTIONS
    //////////////////
    private class Seek : Node<SmartEnemy>
    {
        public override bool Update(SmartEnemy context)
        {
            context.currentState = SmartEnemy.State.Seeking;
            context.MoveTowardsPlayer();
            return true;
        }
    }

    private class StartAttackPrep : Node<SmartEnemy>
    {
        public override bool Update(SmartEnemy context)
        {
            context.currentState = SmartEnemy.State.ReadyToPrepare;
            return true;
        }
    }

    private class PrepareToAttack : Node<SmartEnemy>
    {
        private int pulsesFinished;
        private float timeSincePulseStart;

        void OnEnter(SmartEnemy context)
        {
            timeSincePulseStart = 0;
            pulsesFinished = 0;
        }
            
        public override bool Update(SmartEnemy context)
        {
            if (context.currentState != SmartEnemy.State.Preparing)
            {
                OnEnter(context);
                context.currentState = SmartEnemy.State.Preparing;
            }
            if (timeSincePulseStart <= context.pulseTime)
            {
                context.gameObject.transform.localScale = Vector3.Lerp(context.defaultSize, context.pulseSize * context.defaultSize,
                    Easing.QuadEaseOut(timeSincePulseStart / context.pulseTime));
                timeSincePulseStart += Time.deltaTime;
            }
            else
            {
                pulsesFinished += 1;
                timeSincePulseStart = 0;
            }

            if (pulsesFinished == context.numPulses)
            {
                context.gameObject.transform.localScale = context.defaultSize;
                context.currentState = SmartEnemy.State.ReadyToAttack;
            }
            return true;
        }
    }

    private class AttackPlayer : Node<SmartEnemy>
    {
        private Vector3 target;

        public override bool Update(SmartEnemy context)
        {
            if (context.currentState != SmartEnemy.State.Attacking)
            {
                context.currentState = SmartEnemy.State.Attacking;
                target = Services.GameManager.player.transform.position;
            }

            context.transform.position = Vector3.MoveTowards(context.transform.position, target, context.attackSpeed * Time.deltaTime);

            if (Vector3.Distance(context.transform.position,target) < context.closeEnough)
            {
                context.currentState = SmartEnemy.State.Seeking;
            }

            return true;
        }
    }

    private class Flee : Node<SmartEnemy>
    {
        private Vector3 target;

        public override bool Update(SmartEnemy context)
        {
            if (context.currentState != SmartEnemy.State.Fleeing)
            {
                context.currentState = SmartEnemy.State.Fleeing;
                float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
                Vector3 playerLocation = Services.GameManager.player.transform.position;
                Vector3 fleeDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                target = playerLocation + (context.fleeDistance * fleeDirection);
                context.transform.localScale = context.defaultSize;
            }

            context.transform.position = Vector3.MoveTowards(context.transform.position, target, context.attackSpeed * Time.deltaTime);

            if (Vector3.Distance(context.transform.position, target) < context.closeEnough)
            {
                context.currentState = SmartEnemy.State.Seeking;
            }

            return true;
        }
    }

    /// <summary>
    /// FSM STATES
    /// </summary>
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
            float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
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
