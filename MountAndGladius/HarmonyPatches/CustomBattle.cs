using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.CustomBattle;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MountAndGladius.HarmonyPatches
{
    public class CustomBattle
    {
        public static string[] AllowedFactions = new string[] { "empire", "sturgia", "aserai", "vlandia", "battania", "khuzait", "romanempire", "gallicempire" };

        [HarmonyPatch(typeof(CustomBattleMenuVM), MethodType.Constructor, new Type[] { typeof(CustomBattleState) })]
        internal class CustomBattleMenuVM_Patch
        {
            private static void Postfix(CustomBattleMenuVM __instance)
            {
                InformationManager.DisplayMessage(new InformationMessage("[Debug] CustomBattleMenuVM.CustomBattleMenuVM patch triggered", Color.FromUint(4282569842U)));

                List<BasicCultureObject> factionList = new List<BasicCultureObject>();
                foreach(string factionName in CustomBattle.AllowedFactions)
                {
                    factionList.Add(Game.Current.ObjectManager.GetObject<BasicCultureObject>(factionName));
                }

                FieldInfo factionListField = GetReflectedField(__instance, "_factionList");
                factionListField.SetValue(__instance, factionList);

                MethodInfo onEnemySelectionMethod = __instance.GetType().GetMethod("OnEnemyFactionSelection", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo onPlayerSelectionMethod = __instance.GetType().GetMethod("OnPlayerFactionSelection", BindingFlags.NonPublic | BindingFlags.Instance);

                Action<SelectorVM<SelectorItemVM>> onEnemySelectionAction = (Action<SelectorVM<SelectorItemVM>>)Delegate.CreateDelegate(typeof(Action<SelectorVM<SelectorItemVM>>), __instance, onEnemySelectionMethod);
                Action<SelectorVM<SelectorItemVM>> onPlayerSelectionAction = (Action<SelectorVM<SelectorItemVM>>)Delegate.CreateDelegate(typeof(Action<SelectorVM<SelectorItemVM>>), __instance, onPlayerSelectionMethod);

                List<string> factionsStr = ((List<BasicCultureObject>)factionListField.GetValue(__instance)).Select(f => f.Name.ToString()).ToList();

                __instance.EnemySide.FactionSelectionGroup.Refresh(factionsStr, 0, onEnemySelectionAction);
                __instance.PlayerSide.FactionSelectionGroup.Refresh(factionsStr, 0, onPlayerSelectionAction);

            }

        }


        [HarmonyPatch(typeof(CustomBattleState.Helper))]
        [HarmonyPatch("GetDefaultTroopOfFormationForFaction")]
        class GetDefaultTroopOfFormationForFaction
        {
            static void Postfix(ref BasicCharacterObject __result, BasicCultureObject culture, FormationClass formation)
            {
                InformationManager.DisplayMessage(new InformationMessage("[Debug] CustomBattleState.Helper.GetDefaultTroopOfFormationForFaction patch triggered", Color.FromUint(4282569842U)));

                if (culture.StringId.ToLower() == "romanempire")
                {
                    switch (formation)
                    {
                        case FormationClass.Infantry:
                            __result = GetTroopFromId("imperial_veteran_infantryman");
                            return;
                        case FormationClass.Ranged:
                            __result = GetTroopFromId("imperial_archer");
                            return;
                        case FormationClass.Cavalry:
                            __result = GetTroopFromId("imperial_heavy_horseman");
                            return;
                        case FormationClass.HorseArcher:
                            __result = GetTroopFromId("bucellarii");
                            return;
                    }
                }
                if (culture.StringId.ToLower() == "gallicempire")
                {
                    switch (formation)
                    {
                        case FormationClass.Infantry:
                            __result = GetTroopFromId("battanian_picked_warrior");
                            return;
                        case FormationClass.Ranged:
                            __result = GetTroopFromId("battanian_hero");
                            return;
                        case FormationClass.Cavalry:
                            __result = GetTroopFromId("battanian_scout");
                            return;
                        case FormationClass.HorseArcher:
                            __result = GetTroopFromId("battanian_scout");
                            return;
                    }
                }
            }
        }



        public static BasicCharacterObject GetTroopFromId(string troopId)
        {
            return MBObjectManager.Instance.GetObject<BasicCharacterObject>(troopId);
        }

        internal static FieldInfo GetReflectedField(object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        }
    }
}
