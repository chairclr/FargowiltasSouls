﻿using FargowiltasSouls.Content.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
	public class PalmWoodEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Palm Wood Enchantment");
            /* Tooltip.SetDefault(
@"Double tap down to spawn a palm tree sentry that throws nuts at enemies
'Alarmingly calm'"); */

            //attack rate and damage increased, you can spawn 2 additional trees
        }

        protected override Color nameColor => new(183, 141, 86);
        

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = 10000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PalmEffect(player, Item);
        }
        public override void Load()
        {
            On_Player.KeyDoubleTap += new On_Player.hook_KeyDoubleTap(ActivatePalmwoodSentry);
        }
        public void ActivatePalmwoodSentry(On_Player.orig_KeyDoubleTap orig, Player player, int keyDir)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            orig.Invoke(player, keyDir);
            if (keyDir == (Main.ReversedUpDownArmorSetBonuses ? 1 : 0))
            {
                if (modPlayer.PalmEnchantItem != null)
                {
                    if (player.GetToggleValue("Palm") && player.whoAmI == Main.myPlayer)
                    {
                        Vector2 mouse = Main.MouseWorld;

                        int maxSpawn = 1;

                        if (modPlayer.ForceEffect(modPlayer.PalmEnchantItem.type))
                        {
                            maxSpawn = 2;
                        }

                        if (player.ownedProjectileCounts[ModContent.ProjectileType<PalmTreeSentry>()] > maxSpawn - 1)
                        {
                            for (int i = 0; i < Main.maxProjectiles; i++)
                            {
                                Projectile proj = Main.projectile[i];

                                if (proj.active && proj.type == ModContent.ProjectileType<PalmTreeSentry>() && proj.owner == player.whoAmI)
                                {
                                    proj.Kill();
                                    break;
                                }
                            }
                        }


                        FargoSoulsUtil.NewSummonProjectile(player.GetSource_Misc(""), mouse - 10 * Vector2.UnitY, Vector2.Zero, ModContent.ProjectileType<PalmTreeSentry>(), modPlayer.ForceEffect(modPlayer.PalmEnchantItem.type) ? 100 : 15, 0f, player.whoAmI);
                    }
                }
            }

        }
        public static void PalmEffect(Player player, Item item)
        {
            player.DisplayToggle("Palm");

            FargoSoulsPlayer modPlayer = player.FargoSouls();
            modPlayer.PalmEnchantItem = item;
            
            
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PalmWoodHelmet)
                .AddIngredient(ItemID.PalmWoodBreastplate)
                .AddIngredient(ItemID.PalmWoodGreaves)
                .AddIngredient(ItemID.Coral)
                .AddIngredient(ItemID.Banana)
                .AddIngredient(ItemID.Coconut)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
