﻿using FargowiltasSouls.Content.Items.Accessories.Masomode;
using FargowiltasSouls.Content.Items.Armor;
using FargowiltasSouls.Content.Items.Materials;
using Terraria.ModLoader;

namespace FargowiltasSouls.Core.Toggler.Content
{
    public class MasoToggles : ToggleCollection
    {
        public override string Mod => "Terraria";
        public override string SortCategory => "Maso";
        public override int Priority => 2;
        public override bool Active => true;

        public int MasoHeader2 = ModContent.ItemType<DeviatingEnergy>();
        public string DeerSinewDash;
        public string MasoAeolus;
        public string MasoAeolusFlower;
        public string MasoAeolusFrog;
        public string MasoIcon;
        public string MasoIconDrops;
        public string MasoGraze;
        public string MasoGrazeRing;
        public string MasoDevianttHearts;
        public string PrecisionSealHurtbox;
        public string MasoFishron;

        public int SupremeFairyHeader = ModContent.ItemType<SupremeDeathbringerFairy>();
        public string MasoSlime;
        public string SlimeFalling;
        public string MasoEye;
        public string MasoEyeInstall;
        public string MasoSkele;
        public string MasoSkeleSpin;

        public int BionomicHeader = ModContent.ItemType<BionomicCluster>();
        public string MasoConcoction;
        public string MasoCarrot;
        public string MasoRainbow;
        public string MasoHealingPotion;
        public string MasoNymph;
        public string MasoSqueak;
        public string MasoPouch;
        public string MasoClipped;
        public string MasoGrav2;
        public string TribalCharmClickBonus;

        public int DubiousHeader = ModContent.ItemType<DubiousCircuitry>();
        public string FusedLensInstall;
        public string MasoLightning;
        public string MasoProbe;

        public int PureHeartHeader = ModContent.ItemType<PureHeart>();
        public string MasoEater;
        public string MasoBrain;

        public int LumpofFleshHeader = ModContent.ItemType<LumpOfFlesh>();
        public string MasoPungentCursor;
        public string MasoPugent;
        public string Deerclawps;
        public string DreadShellParry;

        public int ChaliceHeader = ModContent.ItemType<ChaliceoftheMoon>();
        public string MasoCultist;
        public string MasoPlant;
        public string MasoGolem;
        public string MasoBoulder;
        public string MasoCelest;
        public string MasoVision;

        public int HeartHeader = ModContent.ItemType<HeartoftheMasochist>();
        public string MasoPump;
        public string IceQueensCrown;
        public string MasoUfo;
        public string MasoGrav;
        public string MasoTrueEye;

        public int MutantArmorHeader = ModContent.ItemType<MutantBody>();
        public string MasoAbom;
        public string MasoRing;
        public string MasoReviveDeathray;
    }
}
