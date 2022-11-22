using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, ILiquidReceiver, ILiquidEmitter
{
	#region Interface override
    [RequireInterface(typeof(ILiquidEmitter))]
    public Object Emitter;
    [SerializeField]
    [RequireInterface(typeof(ILiquidReceiver))]
    public Object Receiver;
    #endregion

    public ILiquidEmitter last => Emitter as ILiquidEmitter;
    public ILiquidReceiver next => Receiver as ILiquidReceiver;

    [SerializeField] float liquidSendDuration;
    [SerializeField] int stages;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer liquidRenderer;

    private float curDuration;
    private int curStage = 0;
    [SerializeField] private bool isStarted = false;
    private bool isPaused = false;
    private bool emitted = false;

    private Liquid liquid;


    // Update is called once per frame
    void Update()
    {
        if (isStarted)
		{
            if (!isPaused)
                curDuration += Time.deltaTime;

            int lastStage = curStage;

            // Filled at half stage
            curStage = Mathf.FloorToInt(curDuration / (liquidSendDuration*stages/2));

            if (lastStage != curStage)
                SetStage(curStage);

            if (curStage == 3 && !emitted)
                StartEmitting(liquid);
            
            if (curStage > stages)
                End();
		}
    }

    void SetStage(int stageNum)
	{
        if (stageNum > stages)
            return;

        anim.SetInteger("State", stageNum);
	}
    
    void End()
    {
        emitted = false;
        isStarted = false;
        curDuration = 0f;
        curStage = 0;
        SetStage(0);
    }

	public bool BeginReceiving(Liquid l)
    {
        if (isStarted)
            return false;

        liquid = l;
        liquidRenderer.color = l.color;

        isStarted = true;
        return true;
    }

	public bool StartEmitting(Liquid l)
    {
        if (next.BeginReceiving(l))
		{
            emitted = true;
            isPaused = false;
            return true;
		}
        isPaused = true;
        return false;
    }

	public void SetEmitter(ILiquidEmitter emitter)
	{
        this.Emitter = emitter as Object;
	}

	public void SetReceiver(ILiquidReceiver receiver)
	{
        this.Receiver = receiver as Object;
	}
}