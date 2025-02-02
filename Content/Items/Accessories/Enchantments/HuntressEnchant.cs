﻿using FargowiltasSouls.Content.Projectiles;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
	public class HuntressEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Huntress Enchantment");
            /* Tooltip.SetDefault(
@"Attacks ignore 10 enemy defense and deal 5 flat extra damage
Each successive attack ignores an additional 10 defense and deals 5 more damage
This stacks up to 10 times
Homing and minion attacks do not increase these bonuses
Missing any attack will reset these bonuses
'Accuracy brings power'"); */
        }

        protected override Color nameColor => new(122, 192, 76);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Yellow;
            Item.value = 200000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            HuntressEffect(player);
        }

        public static void HuntressEffect(Player player)
        {
            player.DisplayToggle("Huntress");
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            modPlayer.HuntressEnchantActive = true;

            if (modPlayer.HuntressCD > 0)
            {
                modPlayer.HuntressCD--;
            }
        }

        public static void HuntressBonus(FargoSoulsPlayer modPlayer, Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            proj.FargoSouls().HuntressProj = 2;

            if (modPlayer.HuntressCD == 0)
            {
                modPlayer.HuntressStage++;

                if (modPlayer.HuntressStage >= 10)
                {
                    modPlayer.HuntressStage = 10;

                    if (modPlayer.RedRidingEnchantItem != null && modPlayer.RedRidingArrowCD == 0)
                    {
                        RedRidingEnchant.SpawnArrowRain(modPlayer.Player, target);
                    }
                }

                modPlayer.HuntressCD = 30;
            }
            int bonus = modPlayer.ForceEffect(ModContent.ItemType<HuntressEnchant>()) || modPlayer.RedRidingEnchantItem != null ? 5 : 3;
            proj.ArmorPenetration = bonus * 2 * modPlayer.HuntressStage;
            modifiers.SourceDamage.Flat += bonus * modPlayer.HuntressStage;
        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemID.HuntressWig)
            .AddIngredient(ItemID.HuntressJerkin)
            .AddIngredient(ItemID.HuntressPants)
            .AddIngredient(ItemID.IceBow)
            .AddIngredient(ItemID.ShadowFlameBow)
            .AddIngredient(ItemID.DD2PhoenixBow)

            .AddTile(TileID.CrystalBall)
            .Register();
        }
    }
}
