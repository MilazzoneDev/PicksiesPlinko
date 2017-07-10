using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSetupScript : MonoBehaviour {

    [Header("Prefab")]
    public GameObject PegPrefab;

    [Header("Board Size")]
    public Vector2 BoardSize;
    public Vector3 Center;

    [Header("Shape Selection")]
    public float xPadding;
    public float yPadding;
    public bool drawAsCircle;
    public bool drawAsSprial;

    [Header("Lines")]
    public int numRows;
    public int numCols;
    public bool alternatePegs;

    [Header("Circle")]
    public int cirRingDensity;
    public float cirOffset;
    public int cirRings;
    public bool cirAltOffset;

    [Header("Spiral")]
    public int spiArms;
    public float spiOffset;
    public int spiSteps;


    //private
    Transform pegsTransform;

    // Use this for initialization
    void Start ()
    {
        initializeBoard();
    }

    void makePeg(Vector3 pos)
    {
        GameObject peg = Instantiate(PegPrefab, pegsTransform);
        peg.transform.localPosition = pos;
        peg.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float map (float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    public bool IsPointInRectangle(float x, float y, Vector2 min, Vector2 max)
    {
        return (x < max.x && x > min.x && y < max.y && y > min.y);
    }

    public void initializeBoard()
    {
        GameObject board = this.gameObject.transform.Find("Board").gameObject;
        GameObject pegs = this.gameObject.transform.Find("Pegs").gameObject;
        pegsTransform = pegs.transform;

        float xBoardSize = board.transform.localScale.x / 2;
        float yBoardSize = board.transform.localScale.y / 2;

        float xBoardMin = (xBoardSize * -1) + xPadding;
        float xBoardMax = xBoardSize - xPadding;

        float yBoardMin = (yBoardSize * -1) + yPadding;
        float yBoardMax = yBoardSize - yPadding;

        if (!board || !pegs) { return; }
        foreach (Transform t in board.transform) { Destroy(t.gameObject); }
        foreach (Transform t in pegs.transform) { Destroy(t.gameObject); }

        if (drawAsCircle)
        {
            float maxArea = xBoardMax < yBoardMax ? xBoardMax + (xPadding / 2.0f) : yBoardMax + (yPadding / 2.0f);
            Vector2 BoardMin = new Vector2(xBoardMin - (xPadding / 2.0f), yBoardMin - (yPadding / 2.0f));
            Vector2 BoardMax = new Vector2(xBoardMax + (xPadding / 2.0f), yBoardMax + (yPadding / 2.0f));

            bool hasAddedPoints = true;
            for (int i = 0; i < cirRings || hasAddedPoints; i++)
            {
                hasAddedPoints = false;
                for (int j = 0; i == 0 ? j < 1 : j < cirRingDensity * i; j++)
                {
                    if (i == 0)
                    {
                        makePeg(Center);
                    }
                    else
                    {
                        float angle = 0;
                        if (cirAltOffset)
                        {
                            angle = (float)j * ((Mathf.PI * 2.0f) / ((float)i * cirRingDensity)) + ((float)(i%2) * cirOffset);
                        }
                        else
                        {
                            angle = (float)j * ((Mathf.PI * 2.0f) / ((float)i * cirRingDensity)) + ((float)i * cirOffset);
                        }
                        float newX = Mathf.Cos((float)angle) * ((float)i * (maxArea / (cirRings - 1)));
                        float newY = Mathf.Sin((float)angle) * ((float)i * (maxArea / (cirRings - 1)));
                        if (i < cirRings || IsPointInRectangle(newX, newY, BoardMin, BoardMax))
                        {
                            makePeg(new Vector3(newX, newY, 0) + Center);
                            hasAddedPoints = true;
                        }
                    }
                }
            }
        }
        else if (drawAsSprial)
        {
            float maxArea = xBoardMax < yBoardMax ? xBoardMax + (xPadding / 2.0f) : yBoardMax + (yPadding / 2.0f);
            Vector2 BoardMin = new Vector2(xBoardMin - (xPadding / 2.0f), yBoardMin - (yPadding / 2.0f));
            Vector2 BoardMax = new Vector2(xBoardMax + (xPadding / 2.0f), yBoardMax + (yPadding / 2.0f));

            bool hasAddedPoints = true;
            for (int i = 0; i < spiSteps || hasAddedPoints; i++)
            {
                hasAddedPoints = false;
                for (int j = 0; j < spiArms; j++)
                {
                    if (i == 0)
                    {
                        makePeg(Center);
                        j = spiArms;
                    }
                    else
                    {

                        float angle = (float)j * ((Mathf.PI * 2.0f) / spiArms) + ((float)i * spiOffset);
                        float newX = Mathf.Cos((float)angle) * ((float)i * (maxArea / (spiSteps - 1)));
                        float newY = Mathf.Sin((float)angle) * ((float)i * (maxArea / (spiSteps - 1)));
                        if (i < spiSteps || IsPointInRectangle(newX, newY, BoardMin, BoardMax))
                        {
                            makePeg(new Vector3(newX, newY, 0) + Center);
                            hasAddedPoints = true;
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < numCols; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    float newX;
                    float newY = map(0, 1, yBoardMin, yBoardMax, (float)y / (float)(numRows - 1));
                    if (y % 2 == 1 && alternatePegs)
                    {
                        newX = map(0, 1, xBoardMin, xBoardMax, (float)(x + 0.5f) / (float)(numCols - 1));
                        //skip the last col if it's odd
                        if (x >= numCols - 1) { continue; }
                    }
                    else
                    {
                        newX = map(0, 1, xBoardMin, xBoardMax, (float)x / (float)(numCols - 1));
                    }
                    makePeg(new Vector3(newX, newY, 0) + Center);
                }
            }
        }

    }


    ///////////////////Board setup changes///////////////////////
    public void SetCircleBoard()
    {
        drawAsCircle = true;
        drawAsSprial = false;
    }
    public void SetSpiralBoard()
    {
        drawAsCircle = false;
        drawAsSprial = true;
    }
    public void SetLinesBoard()
    {
        drawAsCircle = false;
        drawAsSprial = false;
    }
    // Line accessors
    public void SetLineRows(int newRows)
    {
        numRows = newRows;
    }
    public void SetLineCols(int newCols)
    {
        numCols = newCols;
    }
    public void SetLineAlt(bool newAlt)
    {
        alternatePegs = newAlt;
    }
    // Spiral accessors
    public void SetSpiOffset(float newOffset)
    {
        spiOffset = newOffset;
    }
    public void SetSpiArms(int newArms)
    {
        spiArms = newArms;
    }
    public void SetSpiSteps(int newSteps)
    {
        spiSteps = newSteps;
    }
    // Circle accessors
    public void SetCirRingDensity(int newDensity)
    {
        cirRingDensity = newDensity;
    }
    public void SetCirOffset(float newOffset)
    {
        cirOffset = newOffset;
    }
    public void SetCirRings(int newRings)
    {
        cirRings = newRings;
    }
    public void SetCirAltOffset(bool newAltOffset)
    {
        cirAltOffset = newAltOffset;
    }

}
