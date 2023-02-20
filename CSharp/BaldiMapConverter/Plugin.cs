using BepInEx;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaldiMapConverter
{
    [BepInPlugin("com.dev9998.gorillatag.baldimapconverter", "BaldiMapConverter", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public bool Visible = false;
        public GameObject MainObject;
        public GameObject HallObject;

        public void Awake()
        {
            Instance = this;
        }

        public void OnGUI()
        {
            if (Visible)
            {
                if (GUI.Button(new Rect(25, 25, 124, 20), "Generate"))
                {
                    GenerateData();
                }
            }
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F6))
            {
                Visible = !Visible;
                Cursor.lockState = Visible ? CursorLockMode.None : (SceneManager.GetActiveScene().name.ToLower() == "game" ? CursorLockMode.Locked : CursorLockMode.None);
                Cursor.visible = Visible;
            }
        }

        public void GenerateData()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name.ToLower() == "game")
            {
                var objects = FindObjectsOfType<EnvironmentController>();
                var filteredObjects = objects.Where(a => a.name.Contains("Main")).ToArray();
                if (filteredObjects.Length != 1) return;

                MainObject = filteredObjects[0].gameObject;

                var str = "";
                var str2 = "";
                var str3 = "";
                for (int i = 0; i < MainObject.transform.childCount; i++)
                {
                    var obj = MainObject.transform.GetChild(i);
                    if (obj.name.StartsWith("HallController("))
                    {
                        for (int iRoom = 0; iRoom < obj.childCount; iRoom++)
                        {
                            var room = obj.GetChild(iRoom);
                            if (room.TryGetComponent(out MeshFilter filter))
                            {
                                if (room.name.StartsWith("Tile"))
                                {
                                    str += room.transform.position.x;
                                    str += "$";
                                    str += room.transform.position.y;
                                    str += "$";
                                    str += room.transform.position.z;
                                    str += "$";
                                    str += room.transform.eulerAngles.x;
                                    str += "$";
                                    str += room.transform.eulerAngles.y;
                                    str += "$";
                                    str += room.transform.eulerAngles.z;
                                    str += "$";
                                    str += filter.mesh.name.Replace(" Instance", "");
                                    str += "$";
                                    str += "h";
                                    if (iRoom != obj.childCount) str += "%";
                                }
                            }
                        }
                    }
                    else if (obj.name.StartsWith("RoomController("))
                    {
                        for (int iRoom = 0; iRoom < obj.childCount; iRoom++)
                        {
                            var room = obj.GetChild(iRoom);
                            if (room.TryGetComponent(out MeshFilter filter) && obj.TryGetComponent(out RoomController roomController))
                            {
                                if (room.name.StartsWith("Tile"))
                                {
                                    str2 += room.transform.position.x;
                                    str2 += "$";
                                    str2 += room.transform.position.y;
                                    str2 += "$";
                                    str2 += room.transform.position.z;
                                    str2 += "$";
                                    str2 += room.transform.eulerAngles.x;
                                    str2 += "$";
                                    str2 += room.transform.eulerAngles.y;
                                    str2 += "$";
                                    str2 += room.transform.eulerAngles.z;
                                    str2 += "$";
                                    str2 += filter.mesh.name.Replace(" Instance", "");
                                    str2 += "$";
                                    // Could've used switch here but decided not to
                                    if (roomController.category == RoomCategory.Faculty) str2 += "f";
                                    else if (roomController.category == RoomCategory.Office) str2 += "f";
                                    else if (roomController.category == RoomCategory.Closet) str2 += "s";
                                    else str2 += "c";
                                    if (iRoom != obj.childCount) str2 += "%";
                                }
                            }
                        }
                    }
                    else if (obj.name.StartsWith("Playground("))
                    {
                        for (int iRoom = 0; iRoom < obj.childCount; iRoom++)
                        {
                            var room = obj.GetChild(iRoom);
                            if (room.TryGetComponent(out MeshFilter filter))
                            {
                                if (room.name.StartsWith("Tile"))
                                {
                                    str3 += room.transform.position.x;
                                    str3 += "$";
                                    str3 += room.transform.position.y;
                                    str3 += "$";
                                    str3 += room.transform.position.z;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.x;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.y;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.z;
                                    str3 += "$";
                                    str3 += filter.mesh.name.Replace(" Instance", "");
                                    str3 += "$";
                                    str3 += "o";
                                    if (iRoom != obj.childCount) str3 += "%";
                                }
                            }
                        }
                    }
                    else if (obj.name.StartsWith("Cafeteria("))
                    {
                        for (int iRoom = 0; iRoom < obj.childCount; iRoom++)
                        {
                            var room = obj.GetChild(iRoom);
                            if (room.TryGetComponent(out MeshFilter filter))
                            {
                                if (room.name.StartsWith("Tile"))
                                {
                                    str3 += room.transform.position.x;
                                    str3 += "$";
                                    str3 += room.transform.position.y;
                                    str3 += "$";
                                    str3 += room.transform.position.z;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.x;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.y;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.z;
                                    str3 += "$";
                                    str3 += filter.mesh.name.Replace(" Instance", "");
                                    str3 += "$";
                                    str3 += "ca";
                                    if (iRoom != obj.childCount) str3 += "%";
                                }
                            }
                        }
                    }
                    else if (obj.name.StartsWith("Library("))
                    {
                        for (int iRoom = 0; iRoom < obj.childCount; iRoom++)
                        {
                            var room = obj.GetChild(iRoom);
                            if (room.TryGetComponent(out MeshFilter filter))
                            {
                                if (room.name.StartsWith("Tile"))
                                {
                                    str3 += room.transform.position.x;
                                    str3 += "$";
                                    str3 += room.transform.position.y;
                                    str3 += "$";
                                    str3 += room.transform.position.z;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.x;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.y;
                                    str3 += "$";
                                    str3 += room.transform.eulerAngles.z;
                                    str3 += "$";
                                    str3 += filter.mesh.name.Replace(" Instance", "");
                                    str3 += "$";
                                    str3 += "l";
                                    if (iRoom != obj.childCount) str3 += "%";
                                }
                            }
                        }
                    }
                    else if (obj.name.StartsWith("FieldTripEntrance"))
                    {
                        if (obj.Find("RoomController") != null)
                        {
                            var mainRoom = obj.Find("RoomController");
                            for (int iRoom = 0; iRoom < mainRoom.childCount; iRoom++)
                            {
                                var room = mainRoom.GetChild(iRoom);
                                if (room.TryGetComponent(out MeshFilter filter))
                                {
                                    if (room.name.StartsWith("Tile"))
                                    {
                                        str3 += room.transform.position.x;
                                        str3 += "$";
                                        str3 += room.transform.position.y;
                                        str3 += "$";
                                        str3 += room.transform.position.z;
                                        str3 += "$";
                                        str3 += room.transform.eulerAngles.x;
                                        str3 += "$";
                                        str3 += room.transform.eulerAngles.y;
                                        str3 += "$";
                                        str3 += room.transform.eulerAngles.z;
                                        str3 += "$";
                                        str3 += filter.mesh.name.Replace(" Instance", "");
                                        str3 += "$";
                                        str3 += "o";
                                        if (iRoom != mainRoom.childCount) str3 += "%";
                                    }
                                }
                            }
                        }
                    }
                }

                // Initial data collection
                var currentTime = DateTime.Now.ToString().Replace("/", ".").Replace(":", ".");
                var mainPath = Path.Combine(Path.GetDirectoryName(typeof(Plugin).Assembly.Location), "Data", currentTime);
                Directory.CreateDirectory(mainPath);

                // Save data collection
                File.WriteAllText(Path.Combine(mainPath, "Areas.txt"), str3.ToString());
                File.WriteAllText(Path.Combine(mainPath, "Halls.txt"), str.ToString());
                File.WriteAllText(Path.Combine(mainPath, "Rooms.txt"), str2.ToString());
            }
        }
    }
}
