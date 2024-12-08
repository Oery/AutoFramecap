﻿using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;

namespace AutoFramecap
{
    public class AutoFramecap : Mod
    {
        internal static AutoFramecap Instance;

        public AutoFramecap() : base("Auto Framecap") {}

        public override string GetVersion() => "0.1";

        private readonly string[] cappedScenes =
        [
            "GG_Engine", 
            "GG_Spa", 
            "GG_Unn", 
            "GG_Engine_Root", 
            "GG_Wyrm", 
            "GG_Sly"
        ];

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;

            ModHooks.BeforeSceneLoadHook += OnSceneLoad;
        }

        private string OnSceneLoad(string name) 
        {
            if (cappedScenes.Contains(name))
            {
                Log("Capping framerate for scene " + name);
                Application.targetFrameRate = 400;
            }
            else 
            {
                Log("Uncapping framerate for scene " + name);
                Application.targetFrameRate = -1;
            }

            return name;
        } 
    }
}
