﻿using FargowiltasSouls.Content.Buffs;
using FargowiltasSouls.Content.Projectiles.Minions;
using FargowiltasSouls.Content.Projectiles.Souls;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
	public class GladiatorEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Gladiator Enchantment");
            /* Tooltip.SetDefault(
@"Spears will rain down on struck enemies
Grants knockback immunity when you are facing the attack
'Are you not entertained?'"); */
            //             DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "角斗士魔石");
            //             Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese,
            // @"长矛将倾泄在被攻击的敌人身上
            // '难道你不高兴吗？'");
        }

        protected override Color nameColor => new(156, 146, 78);
        

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = 40000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            GladiatorEffect(player);
        }
        public override void Load()
        {
            On_Player.KeyDoubleTap += new On_Player.hook_KeyDoubleTap(ActivateGladiatorBanner);
        }
        public void ActivateGladiatorBanner(On_Player.orig_KeyDoubleTap orig, Player player, int keyDir)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            orig.Invoke(player, keyDir);
            if (keyDir == (Main.ReversedUpDownArmorSetBonuses ? 1 : 0))
            {
                if (player.whoAmI == Main.myPlayer && modPlayer.GladiatorEnchantActive)
                {
                    int GladiatorStandard = ModContent.ProjectileType<GladiatorStandard>();
                    if (player.ownedProjectileCounts[GladiatorStandard] < 1)
                    {
                        Projectile.NewProjectile(player.GetSource_Misc(""), player.Top, Vector2.UnitY * 25, GladiatorStandard, modPlayer.ForceEffect(ModContent.ItemType<GladiatorEnchant>()) ? 300 : 100, 3f, player.whoAmI);
                    }
                }
            }

        }
        public static void GladiatorEffect(Player player)
        {
            player.DisplayToggle("Gladiator");
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            modPlayer.GladiatorEnchantActive = true;

            if (modPlayer.GladiatorCD > 0)
            {
                modPlayer.GladiatorCD--;
            }
        }

        public static void GladiatorSpearDrop(FargoSoulsPlayer modPlayer, NPC target, int damage)
        {
            Player player = modPlayer.Player;
            bool buff = player.HasBuff<GladiatorBuff>();
            int spearDamage = damage / (buff ? 3 : 5);

            if (spearDamage > 0)
            {
                if (!modPlayer.TerrariaSoul)
                    spearDamage = Math.Min(spearDamage, FargoSoulsUtil.HighestDamageTypeScaling(player, 300));

                for (int i = 0; i < 3; i++)
                {
                    const int arrivalTime = 15;
                    Vector2 spawn = new(target.Center.X + Main.rand.NextFloat(-300, 300), target.Center.Y - Main.rand.Next(600, 801));
                    Vector2 aim = target.Center + (target.velocity * arrivalTime * Main.rand.NextFloat(0.7f, 1.3f));
                    float speed = (aim - spawn).Length() / arrivalTime * Main.rand.NextFloat(0.8f, 1.2f);
                    //Vector2 speed = target.Center + target.velocity * i * 5 * Main.rand.NextFloat(0.5f, 1.5f) - spawn;
                    //speed.Normalize();
                    //speed *= 15f * Main.rand.NextFloat(0.8f, 1.2f);

                    Projectile.NewProjectile(player.GetSource_Misc(""), spawn, Vector2.Normalize(aim - spawn).RotatedByRandom(MathHelper.Pi / 20) * speed, ModContent.ProjectileType<GladiatorJavelin>(), spearDamage, 4f, Main.myPlayer);
                }

                modPlayer.GladiatorCD = modPlayer.ForceEffect(ModContent.ItemType<GladiatorEnchant>()) ? 10 : 30;
                modPlayer.GladiatorCD = buff ? modPlayer.GladiatorCD : (int)Math.Round(modPlayer.GladiatorCD * 1.5f);
            }
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GladiatorHelmet)
                .AddIngredient(ItemID.GladiatorBreastplate)
                .AddIngredient(ItemID.GladiatorLeggings)
                .AddIngredient(ItemID.Spear)
                .AddIngredient(ItemID.Gladius)
                .AddIngredient(ItemID.BoneJavelin, 300)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
