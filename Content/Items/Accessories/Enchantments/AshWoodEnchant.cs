﻿using FargowiltasSouls.Content.Buffs.Masomode;

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
	public class AshWoodEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        protected override Color nameColor => new(139, 116, 100);


        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = 10000;
        }
        public static void PassiveEffect(Player player)
        {
            player.FargoSouls().fireNoDamage = true;
        }
        public override void UpdateInventory(Player player) => PassiveEffect(player);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
            AshwoodEffect(player, Item);
        }

        public static void AshwoodEffect(Player player, Item item)
        {
            player.DisplayToggle("AshWood");
            PassiveEffect(player);
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            modPlayer.AshWoodEnchantItem = item;
            player.buffImmune[ModContent.BuffType<OiledBuff>()] = true;
            player.ashWoodBonus = true;

            if (modPlayer.AshwoodCD > 0)
                modPlayer.AshwoodCD--;
        }

        public static void AshwoodFireball(FargoSoulsPlayer modPlayer, int damage)
        {
            Player player = modPlayer.Player;

            Vector2 vel = Vector2.Normalize(Main.MouseWorld - player.Center) * 17f;
            vel = vel.RotatedByRandom(Math.PI / 10);
            int fireballDamage = damage;
            if (!modPlayer.TerrariaSoul)
                fireballDamage = Math.Min(fireballDamage, FargoSoulsUtil.HighestDamageTypeScaling(player, 60));
            Projectile.NewProjectile(player.GetSource_Accessory(modPlayer.AshWoodEnchantItem), player.Center, vel, ProjectileID.BallofFire, fireballDamage, 1, Main.myPlayer);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.AshWoodHelmet)
            .AddIngredient(ItemID.AshWoodBreastplate)
            .AddIngredient(ItemID.AshWoodGreaves)
            .AddIngredient(ItemID.Fireblossom)
            .AddIngredient(ItemID.SpicyPepper)
            .AddIngredient(ItemID.Gel, 50)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
