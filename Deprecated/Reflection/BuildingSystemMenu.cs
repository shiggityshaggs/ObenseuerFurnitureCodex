//using HarmonyLib;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEngine;

//namespace SimpleUI.Reflection
//{
//    internal class BuildingSystemMenu
//    {
//        #region CurrentCategory
//        static readonly FieldInfo currentCategoryField = AccessTools.Field(typeof(global::BuildingSystemMenu), "currentCategory");

//        public static Furniture.Category CurrentCategory
//        {
//            get => (Furniture.Category)currentCategoryField.GetValue(global::BuildingSystemMenu.instance);
//            set => currentCategoryField.SetValue(global::BuildingSystemMenu.instance, value);
//        }
//        #endregion

//        //#region CurrentCategoryFurnitures
//        //static readonly MethodInfo currentCategoryFurnituresMethod = AccessTools.Method(typeof(global::BuildingSystemMenu), "GetCurrentCategotyFurnitures");
//        //internal static List<BuildingSystem.FurnitureInfo> CurrentCategoryFurnitures => currentCategoryFurnituresMethod.Invoke(global::BuildingSystemMenu.instance, null) as List<BuildingSystem.FurnitureInfo>;
//        //#endregion

//        #region CategorySlot
//        static readonly FieldInfo categorySlotField = AccessTools.Field(typeof(global::BuildingSystemMenu), "categorySlot");
//        public static global::BuildingSystemSlot CategorySlot
//        {
//            get => (global::BuildingSystemSlot)categorySlotField.GetValue(global::BuildingSystemMenu.instance);
//            set => currentCategoryField.SetValue(global::BuildingSystemMenu.instance, value);
//        }
//        #endregion

//        #region CurrentSelectionName
//        static readonly FieldInfo currentSelectionName = AccessTools.Field(typeof(global::BuildingSystemMenu), "currentSelectionName");
//        public static string CurrentSelectionName
//        {
//            get => (string)currentSelectionName.GetValue(global::BuildingSystemMenu.instance);
//            set => currentSelectionName.SetValue(global::BuildingSystemMenu.instance, value);
//        }
//        #endregion

//        #region Slots
//        static readonly FieldInfo slots = AccessTools.Field(typeof(global::BuildingSystemMenu), "slots");
//        public static List<global::BuildingSystemSlot> Slots
//        {
//            get => currentSelectionName.GetValue(global::BuildingSystemMenu.instance) as List<BuildingSystemSlot>;
//        }

//        public static global::BuildingSystemSlot CurrentSlot => Slots[global::BuildingSystemMenu.instance.currentSlot];
//        #endregion

//        #region Prefab
//        static readonly FieldInfo buildingSlotPrefab = AccessTools.Field(typeof(global::BuildingSystemMenu), "buildingSlotPrefab");
//        public static GameObject BuildingSlotPrefab => (GameObject)buildingSlotPrefab.GetValue(global::BuildingSystemMenu.instance);
//        #endregion
//    }
//}
