//using UnityEngine;

//namespace SimpleUI.UI
//{
//    internal static class BorderTexture
//    {
//        public static Texture2D Create(Rect rect, Color borderColor)
//        {
//            int width = (int)rect.width;
//            int height = (int)rect.height;

//            // Create a new texture with the specified dimensions
//            Texture2D texture = new Texture2D(width, height);

//            // Initialize all pixels to transparent
//            Color[] pixels = new Color[width * height];
//            for (int i = 0; i < pixels.Length; i++)
//            {
//                pixels[i] = Color.clear;
//            }

//            // Draw top border
//            for (int x = 0; x < width; x++)
//            {
//                pixels[x] = borderColor;
//            }

//            // Draw bottom border
//            for (int x = 0; x < width; x++)
//            {
//                pixels[x + (height - 1) * width] = borderColor;
//            }

//            // Draw left border
//            for (int y = 0; y < height; y++)
//            {
//                pixels[y * width] = borderColor;
//            }

//            // Draw right border
//            for (int y = 0; y < height; y++)
//            {
//                pixels[y * width + (width - 1)] = borderColor;
//            }

//            // Apply the pixel data to the texture
//            texture.SetPixels(pixels);
//            texture.Apply();

//            return texture;
//        }
//    }
//}
