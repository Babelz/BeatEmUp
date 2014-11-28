using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp
{
    public static class StructHelpers
    {
        public static Vector2 SizeFromPercents(Vector2 parentSize, Vector2 percents) 
        {
            return new Vector2(parentSize.X / 100.0f * percents.X, parentSize.Y / 100.0f * percents.Y); 
        }

        public static Vector2 PercentsFromSize(Vector2 parentSize, Vector2 childSize)
        {
            return new Vector2(parentSize.X / childSize.X * 100.0f, parentSize.Y / childSize.X * 100.0f);
        }
    }
    public static class VectorHelper
    {
        public static float Distance(Vector2 a, Vector2 b)
        {
            double results = Math.Pow((double)(a.X - b.X), 2.0) + Math.Pow((double)(a.Y - b.Y), 2.0);
            return (float)Math.Sqrt(results);
        }
        public static float DotProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static Vector2 CalculateScale(Point targetSize, Point wantedSize)
        {
            return CalculateScale(targetSize.X, targetSize.Y, wantedSize.X, wantedSize.Y);
        }
        public static Vector2 CalculateScale(Vector2 targetSize, Point wantedSize)
        {
            return CalculateScale(targetSize.X, targetSize.Y, wantedSize.X, wantedSize.Y);
        }
        public static Vector2 CalculateScale(Point targetSize, Vector2 wantedSize)
        {
            return CalculateScale(targetSize.X, targetSize.Y, wantedSize.X, wantedSize.Y);
        }
        public static Vector2 CalculateScale(Vector2 targetSize, Vector2 wantedSize)
        {
            return CalculateScale(targetSize.X, targetSize.Y, wantedSize.X, wantedSize.Y);
        }

        /// <summary>
        /// Palauttaa skaala vectorin joka on halutusta koosta skaalattu targetin kokoon.
        /// </summary>
        /// <param name="targetWidth">Kohteen leveys josta skaalataan.</param>
        /// <param name="targetHeight">Kohteen korkeus josta skaalataan.</param>
        /// <param name="wantedWidth">Leveys johon skaalataan.</param>
        /// <param name="wantedHeight">Korekus johon skaalataan.</param>
        /// <returns>Skaala vektori.</returns>
        public static Vector2 CalculateScale(float targetWidth, float targetHeight, float wantedWidth, float wantedHeight)
        {
            float wScale = wantedWidth / targetWidth;
            float hScale = wantedHeight / targetHeight;

            return new Vector2(wScale, hScale);
        }
        public static Vector2 GetTextureSize(Texture2D texture)
        {
            return new Vector2((float)texture.Width, (float)texture.Height);
        }

        public static Vector2 TextSizeToPercents(Vector2 wantedSizeInPixels, Vector2 textSizeInPixels)
        {
            return new Vector2(wantedSizeInPixels.X / 100f * textSizeInPixels.X, wantedSizeInPixels.Y / 100f * textSizeInPixels.Y);
        }
    }
}
