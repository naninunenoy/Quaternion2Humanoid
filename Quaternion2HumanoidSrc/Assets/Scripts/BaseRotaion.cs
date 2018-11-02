using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Quaternion2Humanoid.Scripts {
    // x方向を基準とした方向を独自に定義
    public static class BaseRotation {
        public static readonly Quaternion Right = Quaternion.Euler(-90.0F, 0.0F, 0.0F);
        public static readonly Quaternion Left = Quaternion.Euler(-90.0F, 0.0F, 180.0F);
        public static readonly Quaternion Up = Quaternion.Euler(0.0F, 0.0F, 90.0F);
        public static readonly Quaternion Down = Quaternion.Euler(0.0F, 0.0F, -90.0F);
        public static readonly Quaternion Front = Quaternion.Euler(-90.0F, 0.0F, -90.0F);
        public static readonly Quaternion Back = Quaternion.Euler(-90.0F, 0.0F, 90.0F);
    }
}