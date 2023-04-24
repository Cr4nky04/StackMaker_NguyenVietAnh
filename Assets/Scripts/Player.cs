using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

public class Player : MonoBehaviour
{
    [SerializeField] private BrickCount brickCount;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask unbrickLayer;
    [SerializeField] private LayerMask rightuperlayer;   
    [SerializeField] private LayerMask leftlowerlayer;
    [SerializeField] private GameObject EdibleBrick;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject EatenBrick;
    [SerializeField] private GameObject Ground;
    [SerializeField] private GameObject UnbrickBrick;
    [SerializeField] private GameObject UnbrickgGround;
    [SerializeField] private Push rightupper_push;
    [SerializeField] private Push leftlower_push;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject Result;
    

    private bool isPushing = false;
    private bool isMoving = false;
    private Vector2 swipeStartPos;
    private float deltaDistance;
    private string direction;
    Vector3 dir = Vector3.zero;
    float h_vector;
    float v_vector;
    public List<GameObject> EatBrick = new List<GameObject>();
    public List<GameObject> RemoveBrick = new List<GameObject>();
    void Start()
    {
        rb.GetComponent<Rigidbody>();
        Oninit();
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isMoving)
            {

                isMoving = true;
                Vector2 swipeEndPos = Input.mousePosition;
                h_vector = swipeEndPos.x - swipeStartPos.x;
                v_vector = swipeEndPos.y - swipeStartPos.y;
                float ratio = h_vector / v_vector;
                if (v_vector == 0 && h_vector ==0)
                {
                    isMoving=false;
                }
                if (v_vector < 0 && Math.Abs(v_vector) > Math.Abs(h_vector))
                {
                    direction = "down";
                    dir = new Vector3(0f, 0f, v_vector);
                    deltaDistance = 0.5f;
                }
                if (v_vector > 0 && Math.Abs(v_vector) > Math.Abs(h_vector))
                {
                    direction = "up";
                    dir = new Vector3(0f, 0f, v_vector);
                    deltaDistance = -0.5f;
                }
                if (h_vector < 0 && Math.Abs(h_vector) > Math.Abs(v_vector))
                {
                    direction = "left";
                    dir = new Vector3(h_vector, 0f, 0f);
                    deltaDistance = 0.5f;
                }
                if (h_vector > 0 && Math.Abs(h_vector) > Math.Abs(v_vector))
                {
                    direction = "right";
                    dir = new Vector3(h_vector, 0f, 0f);
                    deltaDistance = -0.5f;
                }

            }
        }
        
            RightUpperPush();
            LeftLowerPush();

        Debug.Log(isMoving);
        if (isMoving)
        {
            Move();
            Unbrick();
            
        }
        


        Debug.Log("Count: " + EatBrick.Count);


    }

    private void Move()
    {
        Debug.Log(2);
        RaycastHit hit;

        Vector3 raycastPosition = new Vector3(transform.position.x, -2f, transform.position.z);

        if (Physics.Raycast(raycastPosition, dir * 100f, out hit, 100f, wallLayer))
        {
            GameObject gameObject = hit.collider.gameObject;
            Vector3 target = Vector3.zero;
            if (gameObject.tag == "Win")
            {
                Debug.Log("Win");
                target = new Vector3(gameObject.transform.position.x, transform.position.y, gameObject.transform.position.z-2f);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                EarnBrick();
                DestroyBrick();
                if (Vector3.Distance(transform.position, target) < 0.01f)
                {
                    isMoving = false;
                    for(int i=0;i<EatBrick.Count;i++)
                    {
                        Destroy(EatBrick[i]);
                        transform.position += Vector3.down * 0.3f;
                    }
                    Result.SetActive(true);
                    
                }
                
            }
            else
            {
                
                if (Math.Abs(v_vector) > Math.Abs(h_vector))
                {
                    target = new Vector3(hit.point.x, transform.position.y, hit.point.z + deltaDistance);
                }

                if (Math.Abs(h_vector) > Math.Abs(v_vector))
                {
                    target = new Vector3(hit.point.x + deltaDistance, transform.position.y, hit.point.z);
                }
                Debug.Log(target);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                EarnBrick();
                DestroyBrick();
                if (Vector3.Distance(transform.position, target) < 0.01f)
                {
                    isMoving = false;
                }
            }
            Debug.Log(target);
        }
    }

    private void DestroyBrick()
    {
        RaycastHit raycast;
        if (Physics.Raycast(transform.position, Vector3.down, out raycast, 1000f, brickLayer))
        {
            GameObject HitObject = raycast.collider.gameObject;
            if (HitObject != null)
            {
                Destroy(HitObject);
            }
        }
    }

    private void EarnBrick()
    {

        RaycastHit raycast;

        if (Physics.Raycast(transform.position, Vector3.down, out raycast, 1000f, brickLayer))
        {
            
            //Debug.Log(raycast.collider.gameObject.name);
            GameObject HitObject = raycast.collider.gameObject;

            if (HitObject != null)
            {
                GameObject temp = Instantiate(EdibleBrick, respawnPoint.position += Vector3.down * 0.3f, Quaternion.identity, transform);
                transform.position += Vector3.up * 0.3f;
                EatBrick.Add(temp);
                brickCount.ShowScore(EatBrick.Count);
            }
        }
    }
    private void Unbrick()
    {
        RaycastHit raycast;
        if (Physics.Raycast(transform.position, Vector3.down, out raycast, 1000f, unbrickLayer))
        {
            GameObject HitObject = raycast.collider.gameObject;

            if (HitObject != null)
            {


                RemoveBrick.Add(EatBrick[EatBrick.Count - 1]);
                Destroy(EatBrick[EatBrick.Count - 1]);
                EatBrick.Remove(EatBrick[EatBrick.Count - 1]);
                respawnPoint.position += Vector3.up * 0.3f;
                Instantiate(UnbrickBrick, HitObject.transform.position + Vector3.down * 0.3f, Quaternion.identity);
                Destroy(HitObject.gameObject);
                transform.position += Vector3.down * 0.3f;
                brickCount.ShowScore(EatBrick.Count);

            }

        }
    }



    private void RightUpperPush()
    {
        
        Debug.Log("isPushing: " + isPushing);
        RaycastHit hitPush;
        
        if (Physics.Raycast(transform.position, Vector3.down * 1000f, out hitPush, 1000f, rightuperlayer))
        {
            GameObject HitObject = hitPush.collider.gameObject;
            Vector3 newtarget = new Vector3(HitObject.transform.position.x, transform.position.y, HitObject.transform.position.z);
            if(Vector3.Distance(transform.position,newtarget)<0.1f)
            {
                isPushing = true;
                isMoving = true;
                rightupper_push = hitPush.collider.GetComponent<Push>();

                Debug.Log("Push");
                if (direction == "right" && isPushing)
                {

                    dir = Vector3.back;
                    deltaDistance = 0.5f;
                    h_vector = 1;
                    v_vector = 2;
                    isPushing = false;
                }
                if (direction == "up" && isPushing)
                {
                    dir = Vector3.left;
                    deltaDistance = 0.5f;
                    h_vector = 2;
                    v_vector = 1;
                    isPushing = false;
                }
            }
            
        }

    }
    private void LeftLowerPush()
    {

        Debug.Log("isPushing: " + isPushing);
        RaycastHit hitPush;
        if (Physics.Raycast(transform.position, Vector3.down * 1000f, out hitPush, 1000f, leftlowerlayer))
        {
            Debug.Log("Push");
            GameObject HitObject = hitPush.collider.gameObject;
            Vector3 newtarget = new Vector3(HitObject.transform.position.x, transform.position.y, HitObject.transform.position.z);
            if (Vector3.Distance(transform.position, newtarget) < 0.1f)
            {
                isPushing = true;
                isMoving = true;
                leftlower_push = hitPush.collider.GetComponent<Push>();

                
                if (direction == "down" && isPushing)
                {

                    dir = Vector3.right;
                    deltaDistance = -0.5f;
                    h_vector = 2;
                    v_vector = 1;
                    isPushing = false;
                }
                if (direction == "left" && isPushing)
                {
                    Debug.Log("forward");
                    dir = Vector3.forward;
                    deltaDistance = -0.5f;
                    h_vector = 1;
                    v_vector = 2;
                    isPushing = false;
                }
            }
                
        }

    }

    private void Oninit()
    {
        transform.position = spawnPoint.transform.position;
        EatBrick.Clear();
        RemoveBrick.Clear();
    }



}
