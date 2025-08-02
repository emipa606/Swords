using UnityEngine;
using Verse;

namespace BigChoppa;

public class CompProperties_BigChoppa : CompProperties
{
    public float angleAdjustmentEast;

    public float angleAdjustmentNorth;

    public float angleAdjustmentSouth;

    public float angleAdjustmentWest;

    public Vector3 chopeastOffset = new(0f, 0f, 0f);

    public Vector3 chopnorthOffset = new(0f, 0f, 0f);

    public Vector3 chopsouthOffset = new(0f, 0f, 0f);

    public Vector3 chopwestOffset = new(0f, 0f, 0f);

    public Vector3 eastOffset = new(0f, 0f, 0f);

    public GraphicData groundGraphic;

    public bool isDualWeapon;

    public Vector3 newtargeastOffset = new(0f, 0f, 0f);

    public Vector3 newtargnorthOffset = new(0f, 0f, 0f);

    public Vector3 newtargsouthOffset = new(0f, 0f, 0f);

    public Vector3 newtargwestOffset = new(0f, 0f, 0f);

    public Vector3 northOffset = new(0f, 0f, 0f);

    public Vector3 offset = new(0f, 0f, 0f);

    public Vector3 southOffset = new(0f, 0f, 0f);

    public bool verticalFlipNorth;

    public bool verticalFlipOutsideCombat;

    public Vector3 westOffset = new(0f, 0f, 0f);

    public CompProperties_BigChoppa()
    {
        compClass = typeof(CompBigChoppa);
    }
}