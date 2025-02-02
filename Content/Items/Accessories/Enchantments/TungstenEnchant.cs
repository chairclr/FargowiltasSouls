using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Projectiles.BossWeapons;
using FargowiltasSouls.Content.Projectiles;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls.Content.Projectiles.ChallengerItems;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
	public class TungstenEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Tungsten Enchantment");

            string tooltip =
@"150% increased weapon size but reduces melee speed
Every half second a projectile will be doubled in size
Enlarged projectiles and non-projectile swords deal 10% more damage and have an additional chance to crit
'Bigger is always better'";
            // Tooltip.SetDefault(tooltip);
        }

        protected override Color nameColor => new(176, 210, 178);
        

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 40000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TungstenEffect(player, Item);
        }

        public static void TungstenEffect(Player player, Item item)
        {
            player.DisplayToggle("Tungsten");
            player.DisplayToggle("TungstenProj");
            FargoSoulsPlayer modPlayer = player.FargoSouls();

            modPlayer.TungstenEnchantItem = item;

            if (!modPlayer.TerrariaSoul && player.HeldItem.damage > 0 && player.HeldItem.CountsAsClass(DamageClass.Melee) && (!player.HeldItem.noMelee || FargoGlobalItem.TungstenAlwaysAffects.Contains(player.HeldItem.type)) && player.HeldItem.pick == 0 && player.HeldItem.axe == 0 && player.HeldItem.hammer == 0)
            {
                modPlayer.Player.GetAttackSpeed(DamageClass.Melee) -= 0.5f;
            }
        }

        public static float TungstenIncreaseWeaponSize(FargoSoulsPlayer modPlayer)
        {
            return 1f + (modPlayer.ForceEffect(modPlayer.TungstenEnchantItem.type) ? 2f : 1f);
        }

        public static bool TungstenAlwaysAffectProj(Projectile projectile)
        {
            return projectile.aiStyle == ProjAIStyleID.Spear
                || projectile.aiStyle == ProjAIStyleID.Yoyo
                || projectile.aiStyle == ProjAIStyleID.ShortSword
                || projectile.aiStyle == ProjAIStyleID.Flail
                || ProjectileID.Sets.IsAWhip[projectile.type]
                || projectile.type == ProjectileID.MonkStaffT2
                || projectile.type == ProjectileID.Arkhalis
                || projectile.type == ProjectileID.Terragrim
                || projectile.type == ProjectileID.JoustingLance
                || projectile.type == ProjectileID.HallowJoustingLance
                || projectile.type == ProjectileID.ShadowJoustingLance
                || projectile.type == ModContent.ProjectileType<PrismaRegaliaProj>()
                || projectile.type == ModContent.ProjectileType<BaronTuskShrapnel>();
        }
        public static bool TungstenNeverAffectsProj(Projectile projectile)
        {
            return projectile.type == ProjectileID.PiercingStarlight;
        }

        public static void TungstenIncreaseProjSize(Projectile projectile, FargoSoulsPlayer modPlayer, IEntitySource source)
        {
            if (TungstenNeverAffectsProj(projectile))
            {
                return;
            }
            bool canAffect = false;
            bool hasCD = true;
            if (TungstenAlwaysAffectProj(projectile))
            {
                canAffect = true;
                hasCD = false;
            }
            else if (FargoSoulsUtil.OnSpawnEnchCanAffectProjectile(projectile, false))
            {
                if (FargoSoulsUtil.IsProjSourceItemUseReal(projectile, source))
                {
                    if (modPlayer.TungstenCD == 0)
                        canAffect = true;
                }
                else if (source is EntitySource_Parent parent && parent.Entity is Projectile sourceProj)
                {
                    if (sourceProj.GetGlobalProjectile<FargoSoulsGlobalProjectile>().TungstenScale != 1)
                    {
                        canAffect = true;
                        hasCD = false;
                    }
                    else if (sourceProj.minion || sourceProj.sentry || ProjectileID.Sets.IsAWhip[sourceProj.type])
                    {
                        if (modPlayer.TungstenCD == 0)
                            canAffect = true;
                    }
                }
            }

            if (canAffect)
            {
                float scale = modPlayer.ForceEffect(modPlayer.TungstenEnchantItem.type) ? 3f : 2f;

                projectile.position = projectile.Center;
                projectile.scale *= scale;
                projectile.width = (int)(projectile.width * scale);
                projectile.height = (int)(projectile.height * scale);
                projectile.Center = projectile.position;
                FargoSoulsGlobalProjectile globalProjectile = projectile.GetGlobalProjectile<FargoSoulsGlobalProjectile>();
                globalProjectile.TungstenScale = scale;

                if (projectile.aiStyle == ProjAIStyleID.Spear || projectile.aiStyle == ProjAIStyleID.ShortSword)
                    projectile.velocity *= scale;

                if (hasCD)
                {
                    modPlayer.TungstenCD = 40;

                    if (modPlayer.Eternity)
                        modPlayer.TungstenCD = 0;
                    else if (modPlayer.ForceEffect(modPlayer.TungstenEnchantItem.type))
                        modPlayer.TungstenCD /= 2;
                }
            }
        }

        public static void TungstenModifyDamage(Player player, ref NPC.HitModifiers modifiers, DamageClass damageClass)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();

            bool forceBuff = modPlayer.ForceEffect(modPlayer.TungstenEnchantItem.type);

            modifiers.FinalDamage *= forceBuff ? 1.15f : 1.1f;

            int max = forceBuff ? 2 : 1;
            for (int i = 0; i < max; i++)
            {
                // TODO: performance I guess
                // if (crit)
                    // break;

                if (Main.rand.Next(0, 100) <= player.ActualClassCrit(damageClass))
                {
                    modifiers.SetCrit();
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenHelmet)
                .AddIngredient(ItemID.TungstenChainmail)
                .AddIngredient(ItemID.TungstenGreaves)
                .AddIngredient(ItemID.TungstenBroadsword)
                .AddIngredient(ItemID.Ruler)
                .AddIngredient(ItemID.CandyCaneSword)

                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
