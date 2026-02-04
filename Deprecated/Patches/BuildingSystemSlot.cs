//using HarmonyLib;
//using System.Collections.Generic;
//using System.Linq;

//namespace SimpleUI.Patches
//{
//    [HarmonyPatch]
//    internal static class BuildingSystemSlot
//    {
//        [HarmonyPatch(typeof(global::BuildingSystemSlot),
//            nameof(global::BuildingSystemSlot.CreateCategoryList),
//            argumentTypes: [typeof(List<Furniture.Category>), typeof(List<object>)]),
//            HarmonyPostfix]
//        static void CreateCategoryList_Post(ref List<Furniture.Category> __result)
//        {
//            IEnumerable<Furniture.Category> Categories = __result
//                .Where(x => x != Furniture.Category.All)
//                .OrderBy(x => x.ToString());

//            __result = [Furniture.Category.All, .. Categories];
//        }
//    }
//}
