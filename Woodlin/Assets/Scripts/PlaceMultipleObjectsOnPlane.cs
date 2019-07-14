using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class PlaceMultipleObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    static public int T_Count = 0;
    static int cnt = 0;

    // Singleton Code
    private static PlaceMultipleObjectsOnPlane _Instance = null;
    public static PlaceMultipleObjectsOnPlane Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = (PlaceMultipleObjectsOnPlane)FindObjectOfType(typeof(PlaceMultipleObjectsOnPlane));
            }
            return _Instance;
        }
    }

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    public Text AROriginText;
    public Transform AROriginTransform;
    public Camera ARCamera;

    [HideInInspector]
    public int PlaneCount = 0;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }


    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARSessionOrigin m_SessionOrigin;

    static List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    //static List<Pose> trying = new List<Pose>();
    static Vector3[] trying = new Vector3[3];
    static int n = 0;
    static int count = 0;

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    void Update()
    {
        T_Count = Input.touchCount + T_Count;

        if (T_Count == 1 )
        {
            //AROriginText.text = T_Count.ToString();
            Touch touch = Input.GetTouch(0);
            if (m_SessionOrigin.Raycast(touch.position, Hits, TrackableType.PlaneWithinPolygon))
            {
                
                Pose hitPose = Hits[0].pose;
                trying[n] = hitPose.position;
                n++;
                for(int i = 0; i < n ; i++)
                {
                    if (hitPose.position != trying[i] && count > 0 )
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                        if (onPlacedObject != null)
                        {
                            onPlacedObject();

                        }
                    } 
                    else if(count == 0)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                        if (onPlacedObject != null)
                        {
                            onPlacedObject();

                        }
                    }
                    count++;
                }


                
            }
            AROriginText.text = T_Count.ToString();

            // Ray ray = ARCamera.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));

            // RaycastHit hitInfo;
            // if(Physics.Raycast(ray, out hitInfo, 10))
            // {
            //     AROriginText.text = "Ray Intersected With Tag : " + hitInfo.transform.tag;
            //     // hitInfo.poi

            //     if(hitInfo.transform.tag=="plane")
            //     {

            //         Instantiate(placedPrefab, hitInfo.point, Quaternion.identity);

            //     }
            //    // Instantiate(placedPrefab, hitInfo.point, Quaternion.identity);
            // }
            // else
            // {
            //     AROriginText.text = "Ray Intersection Test Failed";
            // }

            //if (touch.phase == TouchPhase.Began)
            // {

            // }
        }


        //if (m_SessionOrigin.Raycast(touch.position, Hits, TrackableType.All))
        // {

        //     Pose hitPose = Hits[0].pose;

        //         spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

        //     if (onPlacedObject != null)
        //     {
        //        onPlacedObject();

        //     }
        // }

        AROriginText.text = "ARCamera : " + ARCamera.transform.position.ToString();
    }
}
