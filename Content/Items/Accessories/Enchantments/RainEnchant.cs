﻿using FargowiltasSouls.Content.Buffs.Souls;
using FargowiltasSouls.Content.Projectiles.Souls;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
	public class RainEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Rain Enchantment");
            /* Tooltip.SetDefault(
@"Grants immunity to Wet
Spawns a miniature storm that follows your cursor
It only attacks if there is a clear line of sight between you
Effects of Inner Tube
'Come again some other day'"); */
        }

        protected override Color nameColor => new(255, 236, 0);
        

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.LightPurple;
            Item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            RainEffect(player, Item);
        }

        public static void RainEffect(Player player, Item item)
        {
            player.DisplayToggle("Rain");
            player.DisplayToggle("RainInnerTube");
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            player.buffImmune[BuffID.Wet] = true;
            
            modPlayer.RainEnchantItem = item;
            //modPlayer.AddMinion(item, player.GetToggleValue("Rain"), ModContent.ProjectileType<RainCloud>(), 24, 0);
            if (!player.HasBuff(ModContent.BuffType<RainCDBuff>()) && player.GetToggleValue("Rain"))
            {
                modPlayer.AddMinion(item, player.GetToggleValue("Rain"), ModContent.ProjectileType<RainUmbrella>(), 0, 0);

                if (!player.controlDown)
                {
                    player.slowFall = true;
                }
            }

            if (player.GetToggleValue("RainInnerTube"))
            {
                player.hasFloatingTube = true;
                player.canFloatInWater = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemID.RainHat)
            .AddIngredient(ItemID.RainCoat)
            .AddIngredient(ItemID.UmbrellaHat)
            .AddIngredient(ItemID.FloatingTube) //inner tube
            .AddIngredient(ItemID.Umbrella)
            .AddIngredient(ItemID.WaterGun)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
