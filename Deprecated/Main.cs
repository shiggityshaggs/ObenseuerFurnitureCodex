//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//// https://discord.com/channels/478514341176672267/522423354150486066/1399884788705525921
//// Nagiino — 00:42
//// Not sure if these things have been said, maybe make this a thread channel so it's easier to keep track of previous suggestions?
//// Easier UI to look through the placable furniture you have. I love some of the small items that you can get/see in random units, but it's quite frustrating try to find it in a very long list of stuff. (No I don't sell or break down much lol)
//// Maybe have an income tab, you can see if improving a unit will actually net you more rent money, and make it a little clearer when you get paid/how much you'll get.
//// Have a collections tab to show what blueprints/recipes are all available. I have no idea if I am remotely close and it can be a little overwhelming with everything on page. (same for furniture you can obtain. For instance I have no idea if I can actually purchase a copy machine, game boy, or soap dispenser, I just know where they exist in the world.)
//// Just a few thoughts after 65 hours 😛 

//namespace SimpleUI.UI
//{
//    internal class Main : MonoBehaviour
//    {
//        Rect windowRect;
//        readonly int windowId = "SimpleUI.UI.Main".GetHashCode();
//        Vector2 scrollPos;
//        int scrollDistance = 25;
//        bool show;

//        float categoryWidth = 125;
//        float furnitureWidth = 200;

//        Furniture.Category CurrentCategory = Furniture.Category.All;
//        global::BuildingSystem.FurnitureInfo CurrentItem;
//        bool menuIsOpen = false;

//        IEnumerable<global::BuildingSystem.FurnitureInfo> availableFurnitures = [];
//        IEnumerable<BuildingSystem.FurnitureInfo> furnitureInfosByCategory = [];
//        Dictionary<Furniture.Category, int> FurnituresWithCount = [];
//        List<BuildingSystemSlot> slots;

//        IEnumerable<Furniture.Category> Categories = [];

//        void Awake()
//        {
//            SimpleUI.Patches.BuildingSystemMenu.OnCategory += BuildingSystemMenu_OnCategory;
//            SimpleUI.Patches.BuildingSystemMenu.OnToggle += BuildingSystemMenu_OnToggle;
//            SimpleUI.Patches.BuildingSystem.OnAvailableFurnitures += BuildingSystem_OnAvailableFurnitures;
//            SimpleUI.Patches.BuildingSystem.OnPreview += BuildingSystem_OnPreview;
//            SimpleUI.Patches.BuildingSystemMenu.OnFurnitureListUpdated += BuildingSystemMenu_OnFurnitureListUpdated;

//            Categories = Enum.GetValues(typeof(Furniture.Category))
//                .Cast<Furniture.Category>()
//                .Where(x => x != Furniture.Category.All)
//                .OrderBy(x => x.ToString());
//            Categories = [Furniture.Category.All, .. Categories];
//        }

//        private void BuildingSystemMenu_OnFurnitureListUpdated(List<BuildingSystemSlot> slots)
//        {
//            this.slots = slots;
//        }

//        private void BuildingSystem_OnPreview(BuildingSystem.FurnitureInfo buildObject)
//        {
//            this.CurrentItem = buildObject;
//            if (buildObject != null)
//            {
//                var list = furnitureInfosByCategory.ToList();
//                var idx = list.FindIndex(x => x == buildObject);
//                if (idx == -1) idx = list.FindIndex(x => x.itemStack == buildObject.itemStack);
//                scrollPos.y = Math.Max(0, idx * scrollDistance);
//            }
//        }

//        private void BuildingSystemMenu_OnCategory(Furniture.Category currentCategory)
//        {
//            this.CurrentCategory = currentCategory;

//            furnitureInfosByCategory
//                = (CurrentCategory == Furniture.Category.All)
//                ? availableFurnitures
//                : availableFurnitures.Where(x => x.furniture.category == CurrentCategory);
//        }

//        private void BuildingSystemMenu_OnToggle(bool menuIsOpen)
//        {
//            if (!availableFurnitures.Any()) BuildingSystem_OnAvailableFurnitures();
//            this.menuIsOpen = menuIsOpen;
//        }

//        private void BuildingSystem_OnAvailableFurnitures(IEnumerable<BuildingSystem.FurnitureInfo> availableFurnitures = null)
//        {
//            if (availableFurnitures == null) availableFurnitures = SimpleUI.Utility.Helpers.GetAvailableFurnitures();

//            this.availableFurnitures = availableFurnitures;
//                //.OrderBy(x => x.furniture.title);

//            BuildingSystemMenu_OnCategory(CurrentCategory);

//            FurnituresWithCount = new() { { Furniture.Category.All, availableFurnitures.Count() } };
//            foreach (var category in Categories.Where(x => x != Furniture.Category.All))
//            {
//                int count = availableFurnitures.Count(x => x.furniture.category == category);
//                FurnituresWithCount.Add(category, count);
//            }

//        }

//        void OnGUI()
//        {
//            if (!show) return;
//            windowRect = GUILayout.Window(windowId, windowRect, WindowFunction, "Furniture");
//        }

//        void Update()
//        {
//            show = menuIsOpen && !global::BuildingSystemMenu.instance.moveSlotSelected && !global::BuildingSystemMenu.instance.takeSlotSelected;
//            if (menuIsOpen)
//            {
//                if (Input.mouseScrollDelta.y == 1) BuildingSystemMenu.instance.Move(false);
//                else if (Input.mouseScrollDelta.y == -1) BuildingSystemMenu.instance.Move(true);
//            }
//        }

//        private void WindowFunction(int id)
//        {
//            GUI.skin.label.wordWrap = false;

//            using (new GUILayout.HorizontalScope())
//            {
//                CategoryView();
//                FurnitureView();
//            }

//            if (!GUI.changed) GUI.DragWindow();
//        }

//        void CategoryView()
//        {
//            using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.MinWidth(categoryWidth)))
//            {
//                foreach (var kvp in FurnituresWithCount)
//                {
//                    string displayText = $"{kvp.Key}";
//                    if (kvp.Value > 0) displayText += $" ({kvp.Value})";

//                    if (kvp.Value == 0) GUI.color = Color.gray;
//                    else GUI.color = (kvp.Key == CurrentCategory) ? Color.green : Color.white;

//                    GUILayout.Label(displayText);
//                }
//            }
//        }

//        void FurnitureView()
//        {
//            GUI.color = Color.white;

//            using (var scrollView = new GUILayout.ScrollViewScope(scrollPos, GUI.skin.box, GUILayout.MinWidth(furnitureWidth)))
//            {
//                scrollPos = scrollView.scrollPosition;

//                //foreach (var slot in slots)
//                //{
//                //    if (slot.slotType != BuildingSystemSlot.SlotType.Place) continue;

//                //    GUI.color = (CurrentItem.itemStack == slot.itemStack) || (CurrentItem == slot.buildObject) ? Color.green : Color.white;
                    
//                //    string name = slot.buildObject?.furniture?.title ?? slot.itemStack?.itemReference?.Item?.Title ?? "null";
//                //    GUILayout.Label(name);
//                //}

//                foreach (var item in furnitureInfosByCategory)
//                {
//                    if (CurrentItem != null)
//                    {
//                        if (item.furniture.category == Furniture.Category.Items)
//                        {
//                            GUI.color = (item.itemStack == CurrentItem.itemStack) ? Color.green : Color.white;
//                            GUILayout.Label($"{item.furniture.title}");
//                        }
//                        else
//                        {
//                            GUI.color = (item == CurrentItem) ? Color.green : Color.white;
//                            GUILayout.Label(item.furniture.title);
//                        }
//                    }
//                    //Sprite img = furni.furniture.image;
//                }
//            }
//        }
//    }
//}
