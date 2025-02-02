﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.Hallow
{
	public class Pixie : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.Pixie);

        public int Counter;

        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);

            npc.noTileCollide = true;
        }

        public override void AI(NPC npc)
        {
            base.AI(npc);

            if (npc.HasPlayerTarget)
            {
                if (npc.velocity.Y < 0f && npc.position.Y < Main.player[npc.target].position.Y)
                    npc.velocity.Y = 0f;
                if (Vector2.Distance(Main.player[npc.target].Center, npc.Center) < 200)
                    Counter++;
            }
            if (Counter >= 60)
            {
                if (!Main.dedServ)
                    SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Navi") { Pitch = 0.5f }, npc.Center);
                Counter = 0;
            }
            EModeGlobalNPC.Aura(npc, 100, ModContent.BuffType<SqueakyToyBuff>());
        }

        public override void OnHitNPC(NPC npc, NPC target, NPC.HitInfo hit)
        {
            base.OnHitNPC(npc, target, hit);

            target.AddBuff(ModContent.BuffType<UnluckyBuff>(), 60 * 30);
        }
    }
}
