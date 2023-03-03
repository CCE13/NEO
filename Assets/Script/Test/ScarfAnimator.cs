using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarfAnimator : MonoBehaviour
{
    public int length;
    public LineRenderer LineRend;
    public Vector3[] SegmentPoses;
    [Space]
    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    private Vector3[] SegmentV;
    public float TrailSpeed;
    public Rigidbody2D PlayerRB;
    [Space]
    public float WindStrength;
    public float WindOscillations;
    public Vector3 WindOffset;
    public float ScarfWeight;

    private Dashing _dashing;
    public Gradient dashColorMax;
    public Gradient dashColorMid;
    public Gradient dashColorMin;

    void Start()
    {
        _dashing = GetComponent<Dashing>();
        LineRend.positionCount = length;
        SegmentPoses = new Vector3[length];
        SegmentV = new Vector3[length];

        for (int i = 0; i < SegmentPoses.Length; i++)
        {
            SegmentPoses[i] = targetDir.position;
        }
    }

    void Update()
    {
        if (ScarfWeight < 0) { ScarfWeight = 0.01f; };

        SegmentPoses[0] = targetDir.position;
        
        for (int i = 1; i < SegmentPoses.Length; i++)
        {
            if (ScarfWeight < 0) { ScarfWeight = 0.01f; };
            SegmentPoses[i] = Vector3.SmoothDamp(
                SegmentPoses[i],
                (SegmentPoses[i - 1] + (Vector3.down + new Vector3 (-PlayerRB.velocity.x/ ScarfWeight, -PlayerRB.velocity.y/ ScarfWeight, 0f)) * targetDist) + ((Vector3.left * WindStrength) * (((Mathf.Sin(Time.time * WindOscillations)) + 1) / 2) + WindOffset),
                ref SegmentV[i],
                smoothSpeed + i / TrailSpeed
                );
        }

        LineRend.SetPositions(SegmentPoses);

        if(_dashing)
        {
            Gradient dashColor = new Gradient();

            switch(_dashing._dashCount)
            {
                case 2:
                    dashColor = dashColorMax;
                    break;
                case 1:
                    dashColor = dashColorMid;
                    break;
                case 0:
                    dashColor = dashColorMin;
                break;
            }

            LineRend.colorGradient = dashColor;
        }
    }
}
