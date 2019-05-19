using Assets._scripts.Model;
using UnityEngine;

namespace Assets._scripts.ExtensionMethods
{
    public static class IntegerExtension
    {
        public static Vector2 GetAsVector2(this int i, GridInfo info)
        {
            return new Vector2((i * info.CellSize.x % info.Width) + info.CellSize.x/2, (int)(i * info.CellSize.y / info.Width) + info.CellSize.y/2);
        }
    }
}