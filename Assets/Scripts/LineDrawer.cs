using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] Pipe RToU; // -1, 1
    [SerializeField] Pipe LToU; // 1, 1
    [SerializeField] Pipe DToU; // 0, 1

    [SerializeField] Pipe RToD; // -1, -1
    [SerializeField] Pipe LToD; // 1, -1
    [SerializeField] Pipe UToD; // 0, -1

    [SerializeField] Pipe LToR; // 1, 0
    [SerializeField] Pipe DToR; // 1, 1
    [SerializeField] Pipe UToR; // 1, -1
    
    [SerializeField] Pipe RToL; // -1, 0
    [SerializeField] Pipe DToL; // -1, 1
    [SerializeField] Pipe UToL; // -1, -1

    private Vector2 endPoint;
    private Vector2 startPoint;
    private bool drawing;
    private List<Pipe> drawedPipes;
    private Pipe lastDrawnType;
    private ILiquidEmitter startEmitter;
    private ILiquidReceiver endReceiver;

    // Start is called before the first frame update
    void Start()
    {
        drawing = false;
        drawedPipes = new List<Pipe>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!drawing && Input.GetMouseButtonDown(0))
		{
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, distance: 100f, layerMask: ~LayerMask.GetMask("LiquidIO"));
            if (hit.collider != null && hit.collider.TryGetComponent(out ILiquidEmitter emitter))
			{
                drawing = true;
                startPoint = hit.collider.transform.position;
                startPoint.x = Mathf.Round(startPoint.x);
                startPoint.y = Mathf.Round(startPoint.y);

                startEmitter = emitter;
                
                StartCoroutine(nameof(DrawPipes));
			}
		}

        // Draw pipe
        else if (drawing)
		{
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPoint.x = Mathf.Round(endPoint.x);
            endPoint.y = Mathf.Round(endPoint.y);
        }

        if (drawing && Input.GetMouseButtonUp(0))
        {
            drawing = false;
            StopCoroutine(nameof(DrawPipes));

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, distance: 100f, layerMask: ~LayerMask.GetMask("LiquidIO"));

            if (hit.collider != null && hit.collider.TryGetComponent(out ILiquidReceiver receiver))
			{
                endReceiver = receiver;
                FinalizePipe();
            }
            else
			{
                CancelPipe();
			}

		}
    }

    IEnumerator DrawPipes()
    {
        Vector2 curPos = startPoint;
        lastDrawnType = null;
        Vector2 lastEndPoint = Vector2.zero;

        while (drawing)
        {
            yield return new WaitForSeconds(0.1f);

            if (endPoint == lastEndPoint)
                continue;

            Vector2 diff = curPos - endPoint;
            lastEndPoint = endPoint;

            if (lastDrawnType == null)
			{
                if (diff.x < 0)
                {
                    curPos += new Vector2(1, 0);
                    Pipe p = Instantiate(LToR);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = LToR;
                    continue;
				}
                if (diff.x > 0)
				{
                    curPos += new Vector2(-1, 0);
                    Pipe p = Instantiate(RToL);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = RToL;
                    continue;
				}
                if (diff.y < 0)
				{
                    curPos += new Vector2(0, 1);
                    Pipe p = Instantiate(DToU);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = DToU;
                    continue;
				}
                if (diff.y > 0)
				{
                    curPos += new Vector2(0, -1);
                    Pipe p = Instantiate(UToD);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = UToD;
                    continue;
				}
			}

            if (diff.x < 0)
			{
                if (LastFromRight(lastDrawnType))
                {
                    curPos += new Vector2(1, 0);
                    Pipe p = Instantiate(LToR);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = LToR;
                    continue;
                }
                else
				{
                    ReplaceLastWith(Direction.Right, lastDrawnType);
                    continue;
                }
			}
            if (diff.x > 0)
            {
                if (LastFromLeft(lastDrawnType))
                {
                    curPos += new Vector2(-1, 0);
                    Pipe p = Instantiate(RToL);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = RToL;
                    continue;
                }
                else
				{
                    ReplaceLastWith(Direction.Left, lastDrawnType);
                    continue;
                }
            }

            if (diff.y < 0)
			{
                if (LastFromUp(lastDrawnType))
                {
                    curPos += new Vector2(0, 1);
                    Pipe p = Instantiate(DToU);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = DToU;
                    continue;
                }
                else
				{
                    ReplaceLastWith(Direction.Up, lastDrawnType);
                    continue;
				}
            }

            if (diff.y > 0)
			{
                if (LastFromDown(lastDrawnType))
                {
                    curPos += new Vector2(0, -1);
                    Pipe p = Instantiate(UToD);
                    drawedPipes.Add(p);
                    p.transform.position = curPos;
                    lastDrawnType = UToD;
                    continue;
                }
                else
				{
                    ReplaceLastWith(Direction.Down, lastDrawnType);
                    continue;
				}
            }
        }
    }

    enum Direction
	{
        Left,
        Right,
        Up,
        Down
	}

    void ReplaceLastWith(Direction endingIn, Pipe lastPipeType)
	{
        Pipe replacementType = null;
		switch (endingIn)
		{
			case Direction.Left:
                if (lastPipeType == DToL || lastPipeType == DToR || lastPipeType == DToU)
                    replacementType = DToL;
                if (lastPipeType == UToD || lastPipeType == UToL || lastPipeType == UToR)
                    replacementType = UToL;
                if (lastPipeType == RToD || lastPipeType == RToL || lastPipeType == RToU)
                    replacementType = RToL;
                
                break;
			case Direction.Right:
                if (lastPipeType == DToL || lastPipeType == DToR || lastPipeType == DToU)
                    replacementType = DToR;
                if (lastPipeType == UToD || lastPipeType == UToL || lastPipeType == UToR)
                    replacementType = UToR;
                if (lastPipeType == LToD || lastPipeType == LToR || lastPipeType == LToU)
                    replacementType = LToR;

                break;
			case Direction.Up:
                if (lastPipeType == DToL || lastPipeType == DToR || lastPipeType == DToU)
                    replacementType = DToU;
                if (lastPipeType == LToD || lastPipeType == LToU || lastPipeType == LToR)
                    replacementType = LToU;
                if (lastPipeType == RToD || lastPipeType == RToL || lastPipeType == RToU)
                    replacementType = RToU;

                break;
			case Direction.Down:
                if (lastPipeType == LToR || lastPipeType == LToR || lastPipeType == LToU)
                    replacementType = LToD;
                if (lastPipeType == UToD || lastPipeType == UToL || lastPipeType == UToR)
                    replacementType = UToD;
                if (lastPipeType == RToD || lastPipeType == RToL || lastPipeType == RToU)
                    replacementType = RToD;

                break;
		}

        if (replacementType != null && drawedPipes.Count > 0)
		{
            Pipe p = drawedPipes[drawedPipes.Count - 1];
            drawedPipes.RemoveAt(drawedPipes.Count - 1);

            Pipe go = Instantiate(replacementType);
            go.transform.position = p.transform.position;
            // RECONNECT

            drawedPipes.Add(go);
            Destroy(p.gameObject);

            lastDrawnType = replacementType;
        }

        lastDrawnType = null;
    }

    void FinalizePipe()
	{
        if (drawedPipes.Count <= 2)
            return;

        startEmitter.SetReceiver(drawedPipes[0]);
        drawedPipes[0].Emitter = startEmitter as Object;
        for (int i=0; i<=drawedPipes.Count-2; i++)
		{
            drawedPipes[i].Receiver = drawedPipes[i + 1];
            drawedPipes[i + 1].Emitter = drawedPipes[i];
		}
        drawedPipes[drawedPipes.Count - 1].Emitter = drawedPipes[drawedPipes.Count - 2];
        drawedPipes[drawedPipes.Count - 1].Receiver = endReceiver as Object;
        endReceiver.SetEmitter(drawedPipes[drawedPipes.Count-1]);

        drawedPipes = new List<Pipe>();
	}

    void CancelPipe()
	{
        foreach (Pipe p in drawedPipes)
		{
            Destroy(p.gameObject);
		}
        drawedPipes = new List<Pipe>();
	}


    bool LastFromRight(Pipe p)
	{
        return p == LToR || p == UToR || p == DToR;
	}

    bool LastFromLeft(Pipe p)
	{
        return p == DToL || p == RToL || p == UToL;
	}

    bool LastFromDown(Pipe p)
	{
        return p == LToD || p == RToD || p == UToD;
	}

    bool LastFromUp(Pipe p)
	{
        return p == DToU || p == LToU || p == RToU;
	}
}
