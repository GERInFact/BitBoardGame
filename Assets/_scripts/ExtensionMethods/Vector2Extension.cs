using Assets._scripts.Model;
using UnityEngine;

namespace Assets._scripts.ExtensionMethods
{
    public static class Vector2Extension
    {
        public static int GetAsIndex(this Vector2 vec, GridInfo info)
        {
            return (int) (vec.x / info.CellSize.x) + (int) (vec.y / info.CellSize.y) * info.Width;
        }
    }
}