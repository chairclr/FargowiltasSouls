using System.IO;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.Dungeon
{
    public class Paladin : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.Paladin);

        public int Counter;
        public bool IsSmallPaladin;
        public bool FinishedSpawning;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            base.SendExtraAI(npc, bitWriter, binaryWriter);

            bitWriter.WriteBit(IsSmallPaladin);
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            base.ReceiveExtraAI(npc, bitReader, binaryReader);

            IsSmallPaladin = bitReader.ReadBit();
        }

        public override void AI(NPC npc)
        {
            base.AI(npc);

            if (IsSmallPaladin && Main.netMode == NetmodeID.Server && ++Counter <= 65 && Counter % 15 == 5) //mp sync
            {
                npc.netUpdate = true;
                NetSync(npc);
            }

            if (IsSmallPaladin && !FinishedSpawning)
            {
                FinishedSpawning = true;

                npc.Center = npc.Bottom;

                npc.width = (int)(npc.width * .65f);
                npc.height = (int)(npc.height * .65f);
                npc.scale = .65f;
                npc.lifeMax /= 2;
                if (npc.life > npc.lifeMax)
                    npc.life = npc.lifeMax;

                npc.Bottom = npc.Center;
            }

            EModeGlobalNPC.Aura(npc, 800f, BuffID.BrokenArmor, false, 246);
            foreach (NPC n in Main.npc.Where(n => n.active && !n.friendly && n.type != NPCID.Paladin && n.Distance(npc.Center) < 800f))
            {
                n.Eternity().PaladinsShield = true;
                if (Main.rand.NextBool())
                {
                    int d = Dust.NewDust(n.position, n.width, n.height, DustID.GoldCoin, 0f, -1.5f, 0, new Color());
                    Main.dust[d].velocity *= 0.5f;
                    Main.dust[d].noLight = true;
                }
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);

            target.AddBuff(ModContent.BuffType<LethargicBuff>(), 600);
        }
    }
}
