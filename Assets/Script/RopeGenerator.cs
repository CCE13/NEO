using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    [SerializeField] private Rigidbody2D hook;
    [SerializeField] private GameObject ropeSegment;
    [SerializeField] private float distanceBetweenChains;
    public List<Transform> links;
    public List<Vector3> linkPos;
    private RopeCutter player;
    public bool isCut;
    private GameObject replacement;



    // Start is called before the first frame update
    private void Awake()
    {
        PauseMenuController.Restarting += ResetRope;
        PlayerManager.PlayerDied += ResetRope;
    }
    void Start()
    {
        player = FindObjectOfType<RopeCutter>();
        links.Clear();
        foreach (Transform child in transform)
        {
            links.Add(child);
        }
        foreach (Transform link in links)
        {
            linkPos.Add(link.position);
        }
        GenerateRope();
        replacement = Instantiate(gameObject, transform.position, transform.rotation);
        replacement.SetActive(false);
    }

    public void OnDestroy()
    {
        PauseMenuController.Restarting -= ResetRope;
        PlayerManager.PlayerDied -= ResetRope;
    }


    public void GenerateRope()
    {
        player = FindObjectOfType<RopeCutter>();
        Rigidbody2D previousRb = hook;
        foreach(Transform link in links)
        {
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRb;
            joint.connectedAnchor = new Vector2(0f, -distanceBetweenChains);
            previousRb = link.GetComponent<Rigidbody2D>();
            previousRb.velocity = Vector3.zero;
            previousRb.rotation = 0f;
            previousRb.angularVelocity = 0f;
        }
       Rigidbody2D rb =  transform.Find("Weight").GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        
    }



    private void ResetRope()
    {
        if (!isCut) { return; }

        //foreach (Transform links in links)
        //{
        //    HingeJoint2D joint;
        //    if (links.GetComponent<HingeJoint2D>() == null)
        //    {
        //        joint = links.gameObject.AddComponent<HingeJoint2D>();
        //        joint.anchor = new Vector2(0, 0.08f);
        //        joint.autoConfigureConnectedAnchor = false;
        //    }
        //    for (int i = 0; i < linkPos.Count; i++)
        //    {
        //        links.position = linkPos[i];
        //    }
        //}
        //isCut = false;

        replacement.SetActive(true);
        replacement.GetComponent<RopeGenerator>().GenerateRope();
        Destroy(gameObject);
    }
}
