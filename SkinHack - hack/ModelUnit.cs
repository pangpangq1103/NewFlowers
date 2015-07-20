﻿using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace SkinHack
{
    internal class ModelUnit
    {
        public List<string> AdditionalObjects = new List<string>();
        public List<string> IgnoredModels = new List<string>();
        public string Model;
        public int SkinIndex;
        public List<Obj_AI_Base> SpawnedUnits = new List<Obj_AI_Base>();
        public Obj_AI_Hero Unit;

        public ModelUnit(Obj_AI_Hero unit)
        {
            Unit = unit;
            Model = CharData.BaseSkinName;
            SkinIndex = unit.BaseSkinId;
            Utility.DelayAction.Add(300, () => Game.OnUpdate += Game_OnUpdate);
        }

        public CharacterData CharData
        {
            get { return Unit.CharData; }
        }

        #region UpdateAdditionalObjects

        private void UpdateAdditionalObjects()
        {
            var ChampionName = Unit.ChampionName;
            if (ChampionName.Equals("Lulu"))
            {
                AdditionalObjects.Add("LuluCupcake");
                AdditionalObjects.Add("LuluDragon");
                AdditionalObjects.Add("LuluFaerie");
                AdditionalObjects.Add("LuluKitty");
                AdditionalObjects.Add("LuluLadybug");
                AdditionalObjects.Add("LuluPig");
                AdditionalObjects.Add("LuluSnowman");
                AdditionalObjects.Add("LuluSquill");
                return;
            }

            if (ChampionName.Equals("Rammus"))
            {
                AdditionalObjects.Add("RammusDBC");
                IgnoredModels.Add("RammusPB");
                return;
            }

            if (ChampionName.Equals("Udyr"))
            {
                IgnoredModels.Add("UdyrPhoenix");
                IgnoredModels.Add("UdyrPhoenixUlt");
                IgnoredModels.Add("UdyrTiger");
                IgnoredModels.Add("UdyrTigerUlt");
                IgnoredModels.Add("UdyrTurtle");
                IgnoredModels.Add("UdyrTurtleUlt");
                IgnoredModels.Add("UdyrUlt");
                return;
            }
            //

            if (ChampionName.Equals("Anivia"))
            {
                IgnoredModels.Add("AniviaEgg");
                AdditionalObjects.Add("AniviaIceblock");
                return;
            }

            if (ChampionName.Equals("Annie"))
            {
                AdditionalObjects.Add("AnnieTibbers");
                return;
            }

            if (ChampionName.Equals("Azir"))
            {
                AdditionalObjects.Add("AzirSoldier");
                AdditionalObjects.Add("AzirSunDisc");
                AdditionalObjects.Add("AzirUltSoldier");
                return;
            }

            if (ChampionName.Equals("Bard"))
            {
                AdditionalObjects.Add("BardFollower");
                AdditionalObjects.Add("BardHealthShrine");
                AdditionalObjects.Add("BardPickup");
                AdditionalObjects.Add("BardPickupNoIcon");
                return;
            }

            if (ChampionName.Equals("Caitlyn"))
            {
                AdditionalObjects.Add("CaitlynTrap");
                return;
            }

            if (ChampionName.Equals("Cassiopeia"))
            {
                IgnoredModels.Add("Cassiopeia_Death");
                return;
            }

            if (ChampionName.Equals("Elise"))
            {
                AdditionalObjects.Add("EliseSpiderling");
                IgnoredModels.Add("EliseSpider");
                return;
            }

            if (ChampionName.Equals("Fizz"))
            {
                AdditionalObjects.Add("FizzBait");
                AdditionalObjects.Add("FizzShark");
                return;
            }

            if (ChampionName.Equals("Gnar"))
            {
                IgnoredModels.Add("GnarBig");
                return;
            }

            if (ChampionName.Equals("Heimerdinger"))
            {
                AdditionalObjects.Add("HeimerTBlue");
                AdditionalObjects.Add("HeimerTYellow");
                return;
            }

            if (ChampionName.Equals("JarvanIV"))
            {
                AdditionalObjects.Add("JarvanIVStandard");
                AdditionalObjects.Add("JarvanIVWall");
                return;
            }

            if (ChampionName.Equals("Jinx"))
            {
                AdditionalObjects.Add("JinxMine");
                return;
            }

            if (ChampionName.Equals("KogMaw"))
            {
                IgnoredModels.Add("KogMawDead");
                return;
            }

            if (ChampionName.Equals("Lulu"))
            {
                AdditionalObjects.Add("RobotBuddy");
                return;
            }

            if (ChampionName.Equals("Malzahar"))
            {
                AdditionalObjects.Add("MalzaharVoidling");
                return;
            }

            if (ChampionName.Equals("Maokai"))
            {
                AdditionalObjects.Add("MaokaiSproutling");
                return;
            }

            if (ChampionName.Equals("MonkeyKing"))
            {
                AdditionalObjects.Add("MonkeyKingClone");
                IgnoredModels.Add("MonkeyKingFlying");
                return;
            }

            if (ChampionName.Equals("Nasus"))
            {
                IgnoredModels.Add("NasusUlt");
                return;
            }

            if (ChampionName.Equals("Olaf"))
            {
                AdditionalObjects.Add("OlafAxe");
                return;
            }

            if (ChampionName.Equals("Reksai"))
            {
                AdditionalObjects.Add("RekSaiTunnel");
                return;
            }

            if (ChampionName.Equals("Shaco"))
            {
                AdditionalObjects.Add("ShacoBox");
                return;
            }

            if (ChampionName.Equals("Shyvana"))
            {
                IgnoredModels.Add("ShyvanaDragon");
                return;
            }

            if (ChampionName.Equals("Syndra"))
            {
                AdditionalObjects.Add("SyndraSphere");
                AdditionalObjects.Add("SyndraOrbs");
                return;
            }

            if (ChampionName.Equals("Teemo"))
            {
                AdditionalObjects.Add("TeemoMushroom");
                return;
            }

            if (ChampionName.Equals("Thresh"))
            {
                AdditionalObjects.Add("ThreshLantern");
                return;
            }

            if (ChampionName.Equals("Trundle"))
            {
                AdditionalObjects.Add("TrundleWall");
                return;
            }

            if (ChampionName.Equals("Viktor"))
            {
                AdditionalObjects.Add("ViktorSingularity");
                return;
            }

            if (ChampionName.Equals("Xerath"))
            {
                AdditionalObjects.Add("XerathArcaneBarrageLauncher");
                return;
            }

            if (ChampionName.Equals("Yorick"))
            {
                AdditionalObjects.Add("YorickDecayedGhoul");
                AdditionalObjects.Add("YorickRavenousGhoul");
                AdditionalObjects.Add("YorickSpectralGhoul");
                return;
            }

            if (ChampionName.Equals("Zac"))
            {
                IgnoredModels.Add("ZacRebirthBloblet");
                return;
            }

            if (ChampionName.Equals("Zed"))
            {
                AdditionalObjects.Add("ZedShadow");
                return;
            }

            if (ChampionName.Equals("Zyra"))
            {
                AdditionalObjects.Add("ZyraGraspingPlant");
                AdditionalObjects.Add("ZyraSeed");
                AdditionalObjects.Add("ZyraThornPlant");
                IgnoredModels.Add("ZyraPassive");
            }
        }

        #endregion

        private void UpdateSpawnedUnits()
        {
            SpawnedUnits.RemoveAll(obj => !obj.IsValid);

            if (Unit.AI_LastPetSpawnedID == 0)
            {
                return;
            }

            var unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(Unit.AI_LastPetSpawnedID);

            if (unit != null && unit.IsValid && !SpawnedUnits.Contains(unit))
            {
                SpawnedUnits.Add(unit);
            }
        }

        private void Game_OnUpdate(EventArgs args)
        {

            if (Unit.IsDead)
            {
                return;
            }

            var model = IgnoredModels.Contains(CharData.BaseSkinName) ? CharData.BaseSkinName : Model;
            var skin = SkinIndex;

            if (Program.Config.Item("Champions").IsActive() &&
                (!CharData.BaseSkinName.Equals(model) || !Unit.BaseSkinId.Equals(skin)))
            {
                Console.WriteLine(
                    "[CHAMP] {0} {1} => {2}  {3} => {4}", Unit.ChampionName, CharData.BaseSkinName, model,
                    Unit.BaseSkinId, skin);
                Unit.SetSkin(model, skin, 250);
            }
        }

        public void SetModel(string model, int skin = 0)
        {
            if (!model.IsValidModel())
            {
                return;
            }


            Model = model;
            SkinIndex = skin;

            Console.WriteLine(
                "[CHAMP] {0} {1} => {2}  {3} => {4}", Unit.ChampionName, CharData.BaseSkinName, model, Unit.BaseSkinId,
                skin);

            Game_OnUpdate(new EventArgs());
        }
    }
}