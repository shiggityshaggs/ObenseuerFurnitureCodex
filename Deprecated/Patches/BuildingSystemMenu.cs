//using HarmonyLib;
//using System.Collections.Generic;
//using System.Linq;
//using static SimpleUI.Patches.BuildingSystemMenu;

//namespace SimpleUI.Patches
//{
//    [HarmonyPatch]
//    internal static class BuildingSystemMenu
//    {
//        public delegate void BuildingSystemMenuToggle(bool menuIsOpen);
//        public static event BuildingSystemMenuToggle OnToggle;

//        public delegate void BuildingSystemMenuInteraction();
//        public static event BuildingSystemMenuInteraction OnPress;
//        public static event BuildingSystemMenuInteraction OnSecondaryPress;

//        public delegate void BuildingSystemMenuSlotAction(global::BuildingSystemSlot slot);
//        public static event BuildingSystemMenuSlotAction OnCurrentSlot;
//        public static event BuildingSystemMenuSlotAction OnCurrentVisibleSlot;

//        public delegate void BuildingSystemMenuCategoryAction(Furniture.Category currentCategory);
//        public static event BuildingSystemMenuCategoryAction OnCategory;

//        public delegate void FurnitureListUpdated(List<global::BuildingSystemSlot> slots);
//        public static event FurnitureListUpdated OnFurnitureListUpdated;

//        #region Menu
//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "OpenMenu"), HarmonyPostfix]
//        static void OpenMenu() => OnToggle?.Invoke(true);

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "CloseMenu"), HarmonyPostfix]
//        static void CloseMenu() => OnToggle?.Invoke(false);
//        #endregion

//        #region Interaction
//        [HarmonyPatch(typeof(global::BuildingSystemMenu), nameof(global::BuildingSystemMenu.Press)), HarmonyPrefix]
//        static void Press_Pre(Furniture.Category ___currentCategory, ref Furniture.Category __state) => __state = ___currentCategory;

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), nameof(global::BuildingSystemMenu.Press)), HarmonyPostfix]
//        static void Press_Post(Furniture.Category ___currentCategory, Furniture.Category __state)
//        {
//            CheckIfCategoryChanged(__state, ___currentCategory);
//            OnPress?.Invoke();
//        }

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), nameof(global::BuildingSystemMenu.SecondaryPress)), HarmonyPrefix]
//        static void SecondaryPress_Pre(Furniture.Category ___currentCategory, ref Furniture.Category __state) => __state = ___currentCategory;

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), nameof(global::BuildingSystemMenu.SecondaryPress)), HarmonyPostfix]
//        static void SecondaryPress_Post(Furniture.Category ___currentCategory, Furniture.Category __state)
//        {
//            CheckIfCategoryChanged(__state, ___currentCategory);
//            OnSecondaryPress?.Invoke();
//        }
//        #endregion

//        #region Slots
//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "ChangeSelection"), HarmonyPrefix]
//        static void ChangeSelection_Pre(ref int __state)
//        {
//            __state = global::BuildingSystemMenu.instance.currentSlot;
//        }

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "ChangeSelection"), HarmonyPostfix]
//        static void ChangeSelection_Post(List<global::BuildingSystemSlot> ___slots, int __state)
//        {
//            if (__state != global::BuildingSystemMenu.instance.currentSlot)
//            {
//                if (___slots.Count > global::BuildingSystemMenu.instance.currentSlot)
//                    OnCurrentSlot?.Invoke(___slots[global::BuildingSystemMenu.instance.currentSlot]);
//            }
            
//            if (global::BuildingSystemMenu.instance.lastSlot != global::BuildingSystemMenu.instance.currentVisibleSlot)
//            {
//                if (___slots.Count > global::BuildingSystemMenu.instance.currentVisibleSlot)
//                    OnCurrentVisibleSlot?.Invoke(___slots[global::BuildingSystemMenu.instance.currentVisibleSlot]);
//            }
//        }
//        #endregion

//        #region Category
//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "AddCaregorySlot"), HarmonyPrefix]
//        static void AddCaregorySlot_Pre(Furniture.Category ___currentCategory, ref Furniture.Category __state) => __state = ___currentCategory;

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "AddCaregorySlot"), HarmonyPostfix]
//        static void AddCaregorySlot_Post(Furniture.Category ___currentCategory, Furniture.Category __state) => CheckIfCategoryChanged(___currentCategory, __state);

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), nameof(global::BuildingSystemMenu.FindFurnitureAndSelectIt)), HarmonyPrefix]
//        static void FindFurnitureAndSelectIt_Pre(Furniture.Category ___currentCategory, ref Furniture.Category __state) => __state = ___currentCategory;

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), nameof(global::BuildingSystemMenu.FindFurnitureAndSelectIt)), HarmonyPostfix]
//        static void FindFurnitureAndSelectIt_Post(Furniture.Category ___currentCategory, Furniture.Category __state) => CheckIfCategoryChanged(___currentCategory, __state);

//        static void CheckIfCategoryChanged(Furniture.Category previous, Furniture.Category current)
//        {
//            if (previous != current) OnCategory?.Invoke(current);
//        }
//        #endregion

//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "GetCurrentCategotyFurnitures"), HarmonyPostfix]
//        static void GetCurrentCategotyFurnitures_Post(ref List<global::BuildingSystem.FurnitureInfo> __result) => __result = __result.OrderBy(x => x.furniture.title).ToList();

//        static bool AllSlotsSorted = false;
//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "CreateFurnitureList"), HarmonyPrefix]
//        static void CreateFurnitureList(Furniture.Category ___currentCategory)
//        {
//            // We want Inventory.instance.AllSlots to return sorted by title, since it dictate slot order
//            AllSlotsSorted = true;
//        }


//        [HarmonyPatch(typeof(global::BuildingSystemMenu), "CreateFurnitureList"), HarmonyPostfix]
//        static void CreateFurnitureList(ref List<global::BuildingSystemSlot> ___slots, Furniture.Category ___currentCategory)
//        {
//            OnFurnitureListUpdated?.Invoke(___slots);
//            return;

//            var reserved = ___slots.Take(3);
//            //var furnitureAndInventory = SimpleUI.Utility.Helpers.GetAvailableFurnitures();

//            var inventoryItems = ___slots
//                .Skip(3)
//                .Where(x => x?.itemStack != null && x.itemStack.itemId != -1)
//                .OrderBy(x => x.itemStack.itemReference.Item.Title);

//            if (___currentCategory == Furniture.Category.Items)
//            {
//                ___slots = [.. reserved, .. inventoryItems];
//            }
//            else if (___currentCategory == Furniture.Category.All)
//            {
//                var furniture = ___slots.Skip(3)
//                    .Where(x => x?.buildObject?.furniture != null)
//                    .OrderBy(x => x?.buildObject?.furniture?.GetTitle(x.buildObject.meta.currentSkin));

//                ___slots = [.. reserved, .. furniture, ..inventoryItems];
//            }

//            AllSlotsSorted = false;
//        }

//        [HarmonyPatch(typeof(Inventory), nameof(Inventory.AllSlots)), HarmonyPostfix]
//        static void Inventory_AllSlots(ref SlotController[] __result)
//        {
//            if (AllSlotsSorted)
//            {
//                __result = __result.OrderBy(x => x.itemStack?.itemReference?.Item?.Title).ToArray();
//            }
//        }
//    }
//}
