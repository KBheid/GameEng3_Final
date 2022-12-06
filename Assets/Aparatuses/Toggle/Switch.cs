using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, ILiquidReceiver, ILiquidEmitter
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


    [SerializeField] Animator anim;

    private bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        SetState();
    }

	private void Update()
	{
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isOn = !isOn;
                SetState();
            }
        }

    }

	void SetState()
	{
        anim.SetBool("Toggle", isOn);
	}

	public bool BeginReceiving(Liquid l)
	{
        if (isOn)
        {
            return StartEmitting(l);
        }
        return false;
	}

	public bool StartEmitting(Liquid l)
    {

        if (next == null)
            return false;

        return next.BeginReceiving(l);
	}

	public void SetEmitter(ILiquidEmitter emitter)
	{
        Emitter = emitter as Object;
	}

	public void SetReceiver(ILiquidReceiver receiver)
    {
        Receiver = receiver as Object;
    }

    public List<ILiquidEmitter> GetEmitters()
    {
        return (last == null) ? null : new List<ILiquidEmitter>
        {
            last
        };
    }

    public List<ILiquidReceiver> GetReceivers()
    {
        return (next == null) ? null : new List<ILiquidReceiver>
        {
            next
        };
    }
}
