//using HarmonyLib;
//using System.Collections.Generic;
//using UnityEngine;

//namespace SimpleUI.Patches
//{
//    [HarmonyPatch]
//    internal static class BuildingSystem
//    {
//        public delegate void BuildingSystemAction(IEnumerable<global::BuildingSystem.FurnitureInfo> availableFurnitures);
//        public static event BuildingSystemAction OnAvailableFurnitures;

//        public delegate void BuildingSystemObject(global::BuildingSystem.FurnitureInfo buildObject);
//        public static event BuildingSystemObject OnPreview;

//        [HarmonyPatch(declaringType: typeof(global::BuildingSystem),
//            methodName: nameof(global::BuildingSystem.AddFurniture),
//            argumentTypes: [typeof(Furniture), typeof(GameObject), typeof(GameObject), typeof(int), typeof(global::BuildingSystem.FurnitureInfo.Meta)],
//            argumentVariations: [ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal, ArgumentType.Normal]),
//            HarmonyPrefix]
//        static void AddFurniture_Pre(ref int __state) => __state = global::BuildingSystem.instance.availableFurnitures.Count;

//        [HarmonyPatch(declaringType: typeof(global::BuildingSystem),
//            methodName: nameof(global::BuildingSystem.AddFurniture),
//            argumentTypes: [typeof(Furniture), typeof(GameObject), typeof(GameObject), typeof(int), typeof(global::BuildingSystem.FurnitureInfo.Meta)],
//            argumentVariations: [ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal, ArgumentType.Normal]),
//            HarmonyPostfix]
//        static void AddFurniture_Post(int __state) => Check(__state);

//        [HarmonyPatch(typeof(global::BuildingSystem), nameof(global::BuildingSystem.RemoveFurniture)), HarmonyPrefix]
//        static void RemoveFurniture_Pre(ref int __state) => __state = global::BuildingSystem.instance.availableFurnitures.Count;

//        [HarmonyPatch(typeof(global::BuildingSystem), nameof(global::BuildingSystem.RemoveFurniture)), HarmonyPostfix]
//        static void RemoveFurniture_Post(int __state) => Check(__state);

//        static void Check(int previous)
//        {
//            if (previous == global::BuildingSystem.instance.availableFurnitures.Count || OnAvailableFurnitures == null) return;
//            var f = SimpleUI.Utility.Helpers.GetAvailableFurnitures();
//            OnAvailableFurnitures.Invoke(f);
//        }


//        #region Object
//        //[HarmonyPatch(typeof(global::BuildingSystemMenu), nameof(global::BuildingSystem.ChangeCurrentObject)), HarmonyPostfix]
//        //static void ChangeCurrentObject_Post(global::BuildingSystem.FurnitureInfo buildObject, ItemStack itemStack, int prefabAltIndex = 0)
//        //{
//        //    OnObject?.Invoke(buildObject);
//        //}

//        [HarmonyPatch(typeof(global::BuildingSystem), "InstantiatePreview"), HarmonyPostfix]
//        static void InstantiatePreview(global::BuildingSystem.FurnitureInfo buildObject) => OnPreview?.Invoke(buildObject);
//        #endregion

//    }
//}
