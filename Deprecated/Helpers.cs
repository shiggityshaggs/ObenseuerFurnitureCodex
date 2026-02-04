//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace SimpleUI.Utility
//{
//    internal static class Helpers
//    {
//        internal static IEnumerable<global::BuildingSystem.FurnitureInfo> GetAvailableFurnitures()
//        {
//            List<global::BuildingSystem.FurnitureInfo> availableFurnitures = [.. global::BuildingSystem.instance.availableFurnitures];
//            List<global::BuildingSystem.FurnitureInfo> inventoryFurnitures = [];
//            var slots = Inventory.instance.Slots.Where(x => x.itemStack?.itemId != -1);

//            foreach (var slot in slots)
//            {
//                global::BuildingSystem.FurnitureInfo furnitureInfo = new(furniture: (Furniture)ScriptableObject.CreateInstance(typeof(Furniture)),
//                                                                         meta: new global::BuildingSystem.FurnitureInfo.Meta(),
//                                                                         taskItem: null,
//                                                                         gameObject: null,
//                                                                         amount: slot.itemStack.itemAmount,
//                                                                         itemStack: slot.itemStack);

//                furnitureInfo.furniture.category = Furniture.Category.Items;
//                furnitureInfo.furniture.title = slot.itemStack.itemReference.Item.Title;
//                furnitureInfo.furniture.prefab = slot.itemStack.itemReference.Item.Prefab;
//                furnitureInfo.furniture.previewPrefab = slot.itemStack.itemReference.Item.Prefab;
//                inventoryFurnitures.Add(furnitureInfo);
//            }

//            var orderedFurnitures = availableFurnitures
//                .OrderBy(x => x.furniture != null)
//                .ThenBy(x => x.furniture.title);

//            var orderedInventoryFurnitures = inventoryFurnitures
//                .OrderBy(x => x.furniture.title);

//            return orderedFurnitures.Concat(orderedInventoryFurnitures);
//        }
//    }
//}
