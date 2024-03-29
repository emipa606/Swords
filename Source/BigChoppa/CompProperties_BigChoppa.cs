using UnityEngine;
using Verse;

namespace BigChoppa;

public class CompProperties_BigChoppa : CompProperties
{
    public float angleAdjustmentEast;

    public float angleAdjustmentNorth;

    public float angleAdjustmentSouth;

    public float angleAdjustmentWest;

    public Vector3 chopeastOffset = new Vector3(0f, 0f, 0f);

    public Vector3 chopnorthOffset = new Vector3(0f, 0f, 0f);

    public Vector3 chopsouthOffset = new Vector3(0f, 0f, 0f);

    public Vector3 chopwestOffset = new Vector3(0f, 0f, 0f);

    public Vector3 eastOffset = new Vector3(0f, 0f, 0f);

    public GraphicData groundGraphic;

    public bool isDualWeapon;

    public Vector3 newtargeastOffset = new Vector3(0f, 0f, 0f);

    public Vector3 newtargnorthOffset = new Vector3(0f, 0f, 0f);

    public Vector3 newtargsouthOffset = new Vector3(0f, 0f, 0f);

    public Vector3 newtargwestOffset = new Vector3(0f, 0f, 0f);

    public Vector3 northOffset = new Vector3(0f, 0f, 0f);

    public Vector3 offset = new Vector3(0f, 0f, 0f);

    public Vector3 southOffset = new Vector3(0f, 0f, 0f);

    public bool verticalFlipNorth;

    public bool verticalFlipOutsideCombat;

    public Vector3 westOffset = new Vector3(0f, 0f, 0f);

    public CompProperties_BigChoppa()
    {
        compClass = typeof(CompBigChoppa);
    }
}