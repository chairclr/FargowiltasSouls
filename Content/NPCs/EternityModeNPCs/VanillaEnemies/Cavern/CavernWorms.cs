﻿using Terraria;
using Terraria.ID;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.Cavern
{
	public class CavernWorms : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchTypeRange(
            NPCID.GiantWormHead,
            NPCID.DiggerHead
        );

        public override void OnFirstTick(NPC npc)
        {
            base.OnFirstTick(npc);

            if (Main.rand.NextBool(4))
                EModeGlobalNPC.Horde(npc, Main.rand.Next(6) + 1);
        }
    }
}
