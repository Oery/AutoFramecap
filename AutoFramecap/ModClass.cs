using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;

namespace AutoFramecap
{
    public class AutoFramecap : Mod, IMenuMod, ITogglableMod, IGlobalSettings<ModSettings>
    {
        internal static AutoFramecap Instance;

        public AutoFramecap() : base("Auto Framecap") {}


        private ModSettings modSettings = new();
        public void OnLoadGlobal(ModSettings data) => modSettings = data;
        public ModSettings OnSaveGlobal() => modSettings;

        private readonly string[] cappedScenes =
        [
            "GG_Engine", 
            "GG_Spa", 
            "GG_Unn", 
            "GG_Engine_Root", 
            "GG_Wyrm", 
            "GG_Sly",
            "GG_Hollow_Knight",
        ];

        public bool ToggleButtonInsideMenu => true;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;

            ModHooks.BeforeSceneLoadHook += OnSceneLoad;

            CheckScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public void Unload()
        {
            ModHooks.BeforeSceneLoadHook -= OnSceneLoad;

            Application.targetFrameRate = GameManager.instance.gameSettings.frameCapOn ? 400 : -1;
        }

        private string OnSceneLoad(string name) 
        {
            CheckScene(name);
            return name;
        } 

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return
            [
                (IMenuMod.MenuEntry)toggleButtonEntry,

                new() 
                {
                    Name = "Capped Framerate",
                    Description = "Max FPS when capped, Game Default: 400",
                    Values = [ "400", "300", "200", "100", "60", "30" ],
                    Saver = idx => {
                        modSettings.maxFramerate = idx switch
                            { 0 => 400, 1 => 300, 2 => 200, 3 => 100, 4 => 60, 5 => 30, _ => modSettings.maxFramerate
                        };

                        CheckScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                    },
                    Loader = () => modSettings.maxFramerate switch
                        {  400 => 0, 300 => 1, 200 => 2, 100 => 3, 60 => 4, 30 => 5, _ => 0, },
                }
            ];
        }

        private void CheckScene(string sceneName)
        {
            if (cappedScenes.Contains(sceneName))
            {
                Log("Capping framerate for scene " + sceneName);
                Application.targetFrameRate = modSettings.maxFramerate;
            }
            else
            {
                Log("Uncapping framerate for scene " + sceneName);
                Application.targetFrameRate = -1;
            }
        }
    }
}
