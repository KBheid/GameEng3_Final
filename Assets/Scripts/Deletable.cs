using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deletable : MonoBehaviour
{
    ILiquidEmitter emitter;
    ILiquidReceiver receiver;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out emitter);
        TryGetComponent(out receiver);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
			{
                List<ILiquidEmitter> last = null;
                List<ILiquidReceiver> next = null;

                if (receiver != null)
                    last = receiver.GetEmitters();
                
                if (emitter != null)
                    next = emitter.GetReceivers();

                if (last != null)
                    DeleteLast(last);

                if (next != null)
                    DeleteNext(next);


                Destroy(gameObject);
                
			}
        }
    }

	private void DeleteNext(List<ILiquidReceiver> next)
	{
        foreach (ILiquidReceiver receiver in next)
		{
            if (receiver == null)
                continue;

            if ( (receiver as MonoBehaviour).TryGetComponent(out Pipe p) )
			{
                List<ILiquidReceiver> toDelete = (p as ILiquidEmitter).GetReceivers();
                if (toDelete != null)
                    DeleteNext(toDelete);

                Destroy(p.gameObject);
			}
            
            else if ( (receiver as MonoBehaviour).TryGetComponent(out Switch s) )
            {
                List<ILiquidReceiver> toDelete = (s as ILiquidEmitter).GetReceivers();
                if (toDelete != null)
                    DeleteNext(toDelete);

                Destroy(s.gameObject);
            }

            // Just set its last to null
            else
			{
                receiver.SetEmitter(null);
                if ((receiver as MonoBehaviour).TryGetComponent(out LiquidIOSetter setter)) {
                    setter.ResetInput();
				}

            }
		}
	}

	private void DeleteLast(List<ILiquidEmitter> last)
	{
        foreach (ILiquidEmitter emitter in last)
        {
            if (emitter == null)
                continue;

            if ((emitter as MonoBehaviour).TryGetComponent(out Pipe p))
			{
                List<ILiquidEmitter> toDelete = (p as ILiquidReceiver).GetEmitters();
                if (toDelete != null)
                    DeleteLast(toDelete);

                Destroy(p.gameObject);
			}

            else if ((emitter as MonoBehaviour).TryGetComponent(out Switch s))
            {
                List<ILiquidEmitter> toDelete = (s as ILiquidReceiver).GetEmitters();
                if (toDelete != null)
                    DeleteLast(toDelete);

                Destroy(s.gameObject);
            }

            // Just set its next to null
            else
			{
                emitter.SetReceiver(null);
                if ((emitter as MonoBehaviour).TryGetComponent(out LiquidIOSetter setter))
                {
                    setter.ResetOutput();
                }
            }
		}
        
	}
}
