using HarmonyLib;
using System;
using System.Windows.Forms;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MountAndGladius
{
    public class MGSubModule : MBSubModuleBase
    {
        public const string VERSION_NUMBER = "0.0.1";

        protected override void OnSubModuleLoad()
        {
            //Confirm the module has been loaded
            base.OnSubModuleLoad();
            try
            {
                var harmony = new Harmony("mod.bannerlord.mountandgladius");
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error patching:\n{ex.Message} \n\n{ex.InnerException?.Message}");
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            InformationManager.DisplayMessage(new InformationMessage($"Loaded Mount And Gladius II Version {VERSION_NUMBER}.", Color.FromUint(4282569842U)));
        }
    }
}
