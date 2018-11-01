using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quaternion2Humanoid {
    public struct BoneQaternion {
        public HumanBodyBones Bone { set; get; }
        public Quaternion Quaternion { set; get; }
        public BoneQaternion(HumanBodyBones b, Quaternion q) {
            Bone = b;
            Quaternion = q;
        }
    }
}
