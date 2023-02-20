using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
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

        // Objects
        public GameObject MainObject;
        public GameObject HallObject;

        public Dictionary<RoomCategory, string[]> RoomCategories = new Dictionary<RoomCategory, string[]>()
        {
            { RoomCategory.Class,     new string[2] { "c", "Class"         } },
            { RoomCategory.Faculty,   new string[2] { "f", "Faculty"       } },
            { RoomCategory.Office,    new string[2] { "f", "Office"        } },
            { RoomCategory.FieldTrip, new string[2] { "o", "FieldTripExit" } },
            { RoomCategory.Closet,    new string[2] { "s", "Closet"        } },
            { RoomCategory.Mystery,   new string[2] { "m", "MysteryRoom"   } },
            { RoomCategory.Test,      new string[2] { "m", "MysteryRoom"   } }
        };

        public void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F6)) GenerateData();
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
                var roomCount = 0;

                // Initial data collection
                var currentTime = DateTime.Now.ToString().Replace("/", ".").Replace(":", ".");
                var mainPath = Path.Combine(Path.GetDirectoryName(typeof(Plugin).Assembly.Location), "Data", currentTime);
                Directory.CreateDirectory(mainPath);

                for (int i = 0; i < MainObject.transform.childCount; i++)
                {
                    var obj = MainObject.transform.GetChild(i);
                    if (obj.name.StartsWith("HallController("))
                    {
                        var str = "";
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
                        File.WriteAllText(Path.Combine(mainPath, "Halls.txt"), str);
                    }
                    else if (obj.name.StartsWith("RoomController("))
                    {
                        roomCount++;
                        var str = "";
                        var cat = RoomCategory.Null;
                        for (int iRoom = 0; iRoom < obj.childCount; iRoom++)
                        {
                            var room = obj.GetChild(iRoom);
                            if (room.TryGetComponent(out MeshFilter filter) && obj.TryGetComponent(out RoomController roomController))
                            {
                                cat = roomController.category;
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
                                    str += RoomCategories[roomController.category][0];
                                    if (iRoom != obj.childCount) str += "%";
                                }
                            }
                        }
                        File.WriteAllText(Path.Combine(mainPath, $"Room{roomCount}_{RoomCategories[cat][1]}.txt"), str);
                    }
                    else if (obj.name.StartsWith("Playground("))
                    {
                        var str = "";
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
                                    str += "o";
                                    if (iRoom != obj.childCount) str += "%";
                                }
                            }
                        }
                        File.WriteAllText(Path.Combine(mainPath, $"Playground.txt"), str);
                    }
                    else if (obj.name.StartsWith("Cafeteria("))
                    {
                        var str3 = "";
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
                        File.WriteAllText(Path.Combine(mainPath, $"Cafeteria.txt"), str3);
                    }
                    else if (obj.name.StartsWith("Library("))
                    {
                        var str3 = "";
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
                        File.WriteAllText(Path.Combine(mainPath, $"Library.txt"), str3);
                    }
                    else if (obj.name.StartsWith("FieldTripEntrance"))
                    {
                        if (obj.Find("RoomController") != null)
                        {
                            var str3 = "";
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
                            File.WriteAllText(Path.Combine(mainPath, $"FieldTripEntrance.txt"), str3);
                        }
                    }
                }

                Process.Start("explorer.exe", @mainPath);
            }
        }
    }
}
