﻿using FargowiltasSouls.Content.Projectiles;
using FargowiltasSouls.Core.Globals;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Buffs.Masomode
{
    public class LightningRodBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Rod");
            // Description.SetDefault("You attract thunderbolts");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "避雷针");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "你将会吸引雷电");
        }

        private static void SpawnLightning(Entity obj, int type, int damage, IEntitySource source)
        {
            //tends to spawn in ceilings if the player goes indoors/underground
            Point tileCoordinates = obj.Top.ToTileCoordinates();

            tileCoordinates.X += Main.rand.Next(-25, 25);
            tileCoordinates.Y -= 15 + Main.rand.Next(-5, 5) - (type == ModContent.ProjectileType<LightningVortexHostile>() ? 20 : 0);

            for (int index = 0; index < 10 && !WorldGen.SolidTile(tileCoordinates.X, tileCoordinates.Y) && tileCoordinates.Y > 10; ++index) tileCoordinates.Y -= 1;

            Projectile.NewProjectile(source, tileCoordinates.X * 16 + 8, tileCoordinates.Y * 16 + 17,
                0f, 0f, type, damage, 2f, Main.myPlayer, 0f, obj.whoAmI);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //spawns lightning once per second
            player.FargoSouls().lightningRodTimer++;
            if (player.FargoSouls().lightningRodTimer >= 60)
            {
                player.FargoSouls().lightningRodTimer = 0;
                SpawnLightning(player, ModContent.ProjectileType<LightningVortexHostile>(), 60 / 4, player.GetSource_Buff(buffIndex));
            }

            //if (Main.rand.Next(60) == 1)
            // SpawnLightning(player, ModContent.ProjectileType<LightningVortexHostile>(), 0);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            FargoSoulsGlobalNPC fargoNPC = npc.FargoSouls();
            fargoNPC.lightningRodTimer++;
            if (fargoNPC.lightningRodTimer >= 60)
            {
                fargoNPC.lightningRodTimer = 0;
                SpawnLightning(npc, ModContent.ProjectileType<LightningVortex>(), 60, npc.GetSource_FromThis());
            }
        }
    }
}