using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
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
                var strs = new List<string>();

                // Initial data collection
                var currentTime = DateTime.Now.ToString().Replace("/", ".").Replace(":", ".");
                var mainPath = Path.Combine(Path.GetDirectoryName(typeof(Plugin).Assembly.Location), "Data", currentTime);
                Directory.CreateDirectory(mainPath);

                var str_Door = "";
                foreach(var door in MainObject.transform.GetComponentsInChildren<StandardDoor>())
                {
                    if (door.transform.GetComponentInParent<RoomController>() != null)
                    {
                        str_Door += door.transform.GetChild(0).position.x;
                        str_Door += "$";
                        str_Door += door.transform.GetChild(0).position.y;
                        str_Door += "$";
                        str_Door += door.transform.GetChild(0).position.z;
                        str_Door += "$";
                        str_Door += door.transform.rotation.eulerAngles.y;
                        str_Door += "$";
                        str_Door += "st"; // st for Standard, sw for Swing
                        str_Door += "$";
                        str_Door += RoomCategories[door.transform.GetComponentInParent<RoomController>().category][0];
                        str_Door += "$";
                        var crt = door.transform.GetComponentInParent<RoomController>();
                        if (crt.connectedRooms.Count == 0 || door.bTile.transform.GetComponentInParent<RoomController>().category == RoomCategory.Hall || door.bTile.transform.GetComponentInParent<RoomController>().category == RoomCategory.Test) str_Door += "0";
                        else str_Door += crt.connectedRooms.Where(a => a != crt).ToList().Where(a => a.category != RoomCategory.Test).ToArray().Length;
                        str_Door += "%";
                    }
                }
                strs.Add(str_Door);
                //File.WriteAllText(Path.Combine(mainPath, "StardardDoors.txt"), str_Door);

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
                        strs.Add(str);
                    }
                    else if (obj.name.StartsWith("RoomController("))
                    {
                        roomCount++;
                        var str = "";
                        var cat = RoomCategory.Null;

                        if (obj.transform.Find("Object"))
                        {
                            var o = obj.transform.Find("Object");
                            for (int eye = 0; eye < o.childCount; eye++)
                            {
                                var eyye = o.GetChild(eye);
                                if (eyye.name.StartsWith("Table_Tes"))
                                {
                                    str += eyye.transform.position.x;
                                    str += "$";
                                    str += eyye.transform.position.y;
                                    str += "$";
                                    str += eyye.transform.position.z;
                                    str += "$";
                                    str += eyye.transform.eulerAngles.y;
                                    str += "$";
                                    str += "bt";
                                    str += "%";
                                }
                                if (eyye.name.StartsWith("Chair_Tes"))
                                {
                                    str += eyye.transform.position.x;
                                    str += "$";
                                    str += eyye.transform.position.y;
                                    str += "$";
                                    str += eyye.transform.position.z;
                                    str += "$";
                                    str += eyye.transform.eulerAngles.y;
                                    str += "$";
                                    str += "bc";
                                    str += "%";
                                }
                                if (eyye.name.StartsWith("BigDes"))
                                {
                                    str += eyye.transform.position.x;
                                    str += "$";
                                    str += eyye.transform.position.y;
                                    str += "$";
                                    str += eyye.transform.position.z;
                                    str += "$";
                                    str += eyye.transform.eulerAngles.y;
                                    str += "$";
                                    str += "td";
                                    str += "%";
                                }
                            }
                        }


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
                        strs.Add(str);
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
                        strs.Add(str);
                    }
                    else if (obj.name.StartsWith("Cafeteria("))
                    {
                        var str3 = "";
                        var str4 = "";
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

                            if (room.name.StartsWith("CafeteriaTable"))
                            {
                                str4 += room.transform.position.x;
                                str4 += "$";
                                str4 += room.transform.position.y;
                                str4 += "$";
                                str4 += room.transform.position.z;
                                str4 += "$";
                                str4 += room.transform.eulerAngles.y;
                                str4 += "$";
                                str4 += "ct";
                                str4 += "%";

                            }
                        }
                        strs.Add(str3);
                        strs.Add(str4);
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
                        strs.Add(str3);
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
                            strs.Add(str3);
                        }
                    }
                }

                var iter = 0;
                for (int i_ = 0; i_ < strs.Count; i_++)
                {
                    iter++;
                    var finalStr = strs[i_];
                    File.WriteAllText(Path.Combine(mainPath, $"Room{iter}.log"), finalStr);
                }

                Process.Start("explorer.exe", @mainPath);
            }
        }
    }
}
