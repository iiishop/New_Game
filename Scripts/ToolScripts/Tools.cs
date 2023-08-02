using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Tools
{
    public static class Tools
    {
        public static Vector3 Flip(this Vector3 v) => new Vector3(-v.x, v.y, v.z);
        /// <summary>
        /// 用以播放Spine动画
        /// </summary>
        /// <param name="SA"></param>
        /// <param name="animationName"></param>
        /// <param name="loop">是否循环播放</param>
        /// <param name="immediate">是否立即切换（或者在当前动画播放完成后播放）</param>
        public static void PlaySpineAnimation(this SkeletonAnimation SA, string animationName, bool loop = true, bool immediate = false)
        {
            if (immediate)
            {
                SA.AnimationState.SetAnimation(0, animationName, loop);
                return;
            }
            SA.AnimationState.AddAnimation(0, animationName, loop, 0);
        }
        /// <summary>
        /// 判断一个bool列表中的所有值是否都为某个值
        /// </summary>
        /// <param name="b">bool列表</param>
        /// <param name="Statu">状态</param>
        /// <returns>是否为</returns>
        public static bool IsAllBool(this List<bool> b, bool Statu)
        {
            foreach (var item in b)
            {
                if (item != Statu)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
