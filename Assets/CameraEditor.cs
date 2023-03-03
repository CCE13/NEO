#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using Cinemachine;
using System.Reflection;

namespace Cameras
{
    [ExecuteInEditMode]
    public class CameraEditor : MonoBehaviour
    {
        /// <summary>
        /// Camera editor to easilty edit the camera, this script will not run in game
        /// 
        /// 
        /// ----Controls----
        /// Press 1 to set to awake mode
        ///     (Basically a 'just in case reset button' do not have to press this often only when somehow you cannot switch states)
        /// 
        /// Press 3 for polygon editing mode 
        /// 
        ///     (In this mode for defult you can click anywhere in the scene view to place a polygon point, 
        ///     The index of the polygon collider will increase each time you set a point,
        ///     To prescisely change an point, you can use the scroll wheel to change index and simply click on the scene view to adjust its point)
        /// 
        /// Press 4 for camera creation
        /// 
        ///     (This mode will just duplicate a camera and move the selected object to the new camera in the hierarchy )
        /// 
        /// Right Click to activate the camera
        /// 
        ///     (In this mode you can see the preview of the camera in the game view)
        ///     
        ///     Press 5 and 6 to adjust camera size
        ///     
        ///     (This mode can only work if the camera is activated, once activate press 5 to increase camera size and press 6 to decrease camera size)
        /// 
        ///  ----Controls----
        /// 
        /// </summary>

        public enum CamEditorStates {Sleeping, Awake, PolyEditor, PointEditor, CameraSpwaner, CameraEditor }
        public CamEditorStates CamState;
        public List<Vector2> polygonSize;
        public int index;
        public GameObject roomPrefab;


        private bool playOnce;
        private GameObject thisRoomVirtualCam;
        private GameObject ParentController;

        private bool _start;


        private void Update()
        {
            if (Application.isPlaying)
            {
                
                
                this.enabled = false;
                return;

            }

            if (!_start)
            {

                

                if (CamState == CamEditorStates.Awake)
                {
                    SelectionFunction(this.gameObject);
                    //Debug.Log("awoken");
                }

                //if it is the main camera reference
                if(this.transform.GetSiblingIndex() != 0)
                {

                    PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(gameObject, PrefabUnpackMode.Completely);
                }
                

                ParentController = this.gameObject.transform.parent.gameObject;
                this.gameObject.name = "Room" + this.transform.GetSiblingIndex();
                roomPrefab = ParentController.transform.GetChild(0).gameObject;

                thisRoomVirtualCam = transform.GetChild(0).gameObject;
                //PointIndicator = transform.GetChild(1).gameObject;
                thisRoomVirtualCam.SetActive(false);
                _start = true;
                
            }

            
            
            if (Selection.activeGameObject != this.gameObject)
            {
                
                CamState = CamEditorStates.Sleeping;
            }

            if (CamState == CamEditorStates.Sleeping && Selection.activeGameObject == this.gameObject)
            {
                //Debug.Log(Event.current.type);
                SelectionFunction(gameObject);
                CamState = CamEditorStates.Awake;

            }

        }

        public void ChangeCamSize(float size)
        {
            var virtualCam = thisRoomVirtualCam.GetComponent<CinemachineVirtualCamera>();
            var test =  virtualCam.m_Lens.OrthographicSize + size;
            virtualCam.m_Lens.OrthographicSize = test;
            //GameObject camPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(roomPrefab);
            //PrefabUtility.SetPropertyModifications(camPrefab, PrefabUtility.GetPropertyModifications(this.gameObject));
            
        }

        public void SelectionFunction(GameObject GB)
        {
            
            Selection.activeGameObject = GB;
        }

      



        #region editor
        [CustomEditor(typeof(CameraEditor))]
        [System.Serializable]
        public class CameraEditorEditor : Editor
        {
            
            
            void OnSceneGUI()
            {

                CameraEditor cam = (CameraEditor)target;

                if (Application.isPlaying)
                {
                    cam.enabled = false;
                    return;

                }

                if (cam.CamState != CamEditorStates.Sleeping)
                {
                    //Debug.Log(Event.current.type);
                    cam.SelectionFunction(cam.gameObject);

                }

                //Mouse Position in editor calculations
                Vector3 mousePosition = Event.current.mousePosition;
                Ray mouseray = HandleUtility.GUIPointToWorldRay(mousePosition);
                var newPos = mouseray.origin;
                //cam.point.transform.position = newPos;



                switch (cam.CamState)
                {
                    case CamEditorStates.Sleeping:
                        //cam.PointIndicator.SetActive(false);
                        break;
                    case CamEditorStates.Awake:
                        //cam.PointIndicator.SetActive(false);
                        break;
                    case CamEditorStates.PolyEditor:
                        PolygonEditor(newPos);
                        break;
                    case CamEditorStates.PointEditor:
                        PointEditing();
                        break;
                    case CamEditorStates.CameraSpwaner:
                        SpawnCamera(newPos);
                        break;
                    case CamEditorStates.CameraEditor:
                        ChangeCameraSize();
                        break;
                    default:
                        break;
                }

               

                //When mouse leave window
                if (Event.current.type == EventType.MouseLeaveWindow)
                {
                    cam._start = false;
                }

                //On press 1 on the keyboard
                if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha1)
                {
                    cam.CamState = CamEditorStates.Awake;
                }

                //On press 3 on the keyboard
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha3)
                {
                    cam.playOnce = false;


                    if (cam.CamState == CamEditorStates.PointEditor)
                    {
                        cam.CamState = CamEditorStates.Awake;

                    }
                    else
                    {
                        cam.CamState = CamEditorStates.PointEditor;
                        
                    }
                    
                }

                //on press 4 on the keyboard
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha5)
                {

                    cam.CamState = CamEditorStates.CameraSpwaner;


                }

                
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha4)
                {

                    if (cam.CamState == CamEditorStates.PolyEditor)
                    {
                        cam.CamState = CamEditorStates.Awake;

                    }
                    else
                    {
                        cam.CamState = CamEditorStates.PolyEditor;
                        cam.index = 0;

                    }

                    
                    

                }

                if(cam.CamState != CamEditorStates.PointEditor)
                {
                    if (!cam.playOnce)
                    {
                        Tools.current = Tool.None;
                        cam.playOnce = true;
                    }
                    
                }

                //on RightCLick on the mouse
                if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
                {
                    if (cam.thisRoomVirtualCam.activeInHierarchy)
                    {
                        cam.thisRoomVirtualCam.SetActive(false);
                        cam.CamState = CamEditorStates.Awake;
                    }
                    else
                    {
                        cam.thisRoomVirtualCam.SetActive(true);
                        cam.CamState = CamEditorStates.CameraEditor;
                    }
                    

                }

                

                
            }

            private void ChangeCameraSize()
            {
                CameraEditor cam = (CameraEditor)target;
                if (Event.current.type == EventType.ScrollWheel)
                {
                    if (Event.current.delta.y > 0)
                    {
                        cam.ChangeCamSize(1f);

                    }

                    if (Event.current.delta.y < 0)
                    {
                        cam.ChangeCamSize(-1f);


                    }
                }

            }

            private void PointEditing()
            {
                CameraEditor cam = (CameraEditor)target;
                ToolManager.SetActiveTool(Assembly.Load("UnityEditor").GetType("UnityEditor.PolygonCollider2DTool"));





            }

            private void SpawnCamera(Vector3 newPos)
            {
                CameraEditor cam = (CameraEditor)target;
                Debug.Log("hello");
                GameObject camPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(cam.roomPrefab);
                camPrefab.transform.position = new Vector3(newPos.x, newPos.y, -10);
                Selection.activeGameObject = null;
                camPrefab.GetComponent<CameraEditor>().CamState = CamEditorStates.Awake;

                PrefabUtility.InstantiatePrefab(camPrefab, cam.ParentController.transform);

                cam.CamState = CamEditorStates.Sleeping;
                PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(cam.gameObject, PrefabUnpackMode.Completely);
                
            }

            private void PolygonEditor(Vector3 newPos)
            {
                CameraEditor cam = (CameraEditor)target;

                

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha6)
                {
                    cam.index = 0;
                    cam.polygonSize.Clear();
                    cam.GetComponent<PolygonCollider2D>().SetPath(0, cam.polygonSize);

                }
                //on left click down
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 )
                {
                    PolygonCollider2D poly = cam.GetComponent<PolygonCollider2D>();
     

                    if (cam.CamState == CamEditorStates.PolyEditor)
                    {
                        



                        if (cam.index < cam.polygonSize.Count && cam.polygonSize.Count != 0)
                        {
                            cam.polygonSize[cam.index] = cam.transform.InverseTransformPoint(newPos);
                            poly.SetPath(0, cam.polygonSize);
                        }
                        else
                        {
                            
                            cam.polygonSize.Add(cam.transform.InverseTransformPoint(newPos));
                            poly.SetPath(0, cam.polygonSize);
                            //AddToPolygon(poly);
                        }

                        cam.index += 1;

                            


                    }


                }

            }

        }
        #endregion




    }
}

#endif