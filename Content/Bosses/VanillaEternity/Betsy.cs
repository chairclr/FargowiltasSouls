using System.IO;
using Terraria.ModLoader.IO;
using FargowiltasSouls.Content.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Content.Projectiles;
using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.Systems;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Common.Utilities;
using FargowiltasSouls.Core.NPCMatching;
using System.Linq;

namespace FargowiltasSouls.Content.Bosses.VanillaEternity
{
	public class Betsy : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.DD2Betsy);

        public int EntranceTimer = 0;

        public int FuryRingTimer;
        public int FuryRingShotRotationCounter;

        public bool DoFuryRingAttack;
        public bool InFuryRingAttackCooldown;
        public bool InPhase2;

        public bool DroppedSummon;


        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write7BitEncodedInt(FuryRingTimer);
            binaryWriter.Write7BitEncodedInt(FuryRingShotRotationCounter);
            bitWriter.WriteBit(DoFuryRingAttack);
            bitWriter.WriteBit(InFuryRingAttackCooldown);
            bitWriter.WriteBit(InPhase2);
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            FuryRingTimer = binaryReader.Read7BitEncodedInt();
            FuryRingShotRotationCounter = binaryReader.Read7BitEncodedInt();
            DoFuryRingAttack = bitReader.ReadBit();
            InFuryRingAttackCooldown = bitReader.ReadBit();
            InPhase2 = bitReader.ReadBit();
        }

        public override void SetDefaults(NPC npc)
        {
            npc.boss = true;
            npc.lifeMax = (int)Math.Round(npc.lifeMax * 4.0 / 3.0);
        }

        public override bool SafePreAI(NPC npc)
        {
            EModeGlobalNPC.betsyBoss = npc.whoAmI;

            if (WorldSavingSystem.SwarmActive)
                return true;

            //npc.boss = npc.HasPlayerTarget || !DD2Event.Ongoing; //allow players to respawn in mp if everyone is dead during event

            if (EntranceTimer == 0)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyScream, npc.Center);
            }
            if (EntranceTimer < 60 * 2)
            {
                EntranceTimer++;
                npc.dontTakeDamage = true;
                npc.TargetClosest(false);
                npc.spriteDirection = Math.Sign(npc.DirectionTo(Main.player[npc.target].Center).X);
                npc.rotation = npc.DirectionTo(Main.player[npc.target].Center).ToRotation();
                if (npc.spriteDirection == -1)
                {
                    npc.rotation += MathHelper.Pi;
                }
                return false;
            }
            if (EntranceTimer == 60 * 2)
            {
                npc.dontTakeDamage = false;
            }

            if (WorldSavingSystem.MasochistModeReal)
            {
                for (int i = 0; i < 3; i++)
                {
                    Rectangle rectangle = new((int)Main.screenPosition.X + Main.screenWidth / 3, (int)Main.screenPosition.Y + Main.screenHeight / 3, Main.screenWidth / 3, Main.screenHeight / 3);
                    CombatText.NewText(rectangle, new Color(100 + Main.rand.Next(150), 100 + Main.rand.Next(150), 100 + Main.rand.Next(150)), Main.rand.Next(new List<string> {
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy1"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy2"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy3"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy4"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy5"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy6"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy7"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy8"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy9"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy10"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy11"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy12"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy13"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy14"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy15"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy16"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy17"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy18"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy19"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy20"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy21"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy22"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy23"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy24"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy25"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy26") + $"{Main.rand.Next(10)}{Main.rand.Next(10)}{Main.rand.Next(10)}{Main.rand.Next(10)}{Main.rand.Next(10)}" + Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy27"),
                    Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy26") + $"{Main.rand.Next(10)}{Main.rand.Next(10)}{Main.rand.Next(10)}{Main.rand.Next(10)}{Main.rand.Next(10)}" + Language.GetTextValue("Mods.FargowiltasSouls.EternityMode.Betsy28"),
                    }), Main.rand.NextBool(), Main.rand.NextBool());
                }

                if (Main.rand.NextBool(30) && npc.HasPlayerTarget)
                {
                    switch (Main.rand.Next(12))
                    {
                        case 0:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Thunder"), Main.player[npc.target].Center);
                            break;

                        case 1:
                            SoundEngine.PlaySound(SoundID.ScaryScream, Main.player[npc.target].Center); //arte scream
                            break;

                        case 2:
                            SoundEngine.PlaySound(SoundID.Roar, Main.player[npc.target].Center);
                            break;

                        case 3:
                            SoundEngine.PlaySound(SoundID.ForceRoarPitched, Main.player[npc.target].Center); //eoc roar
                            break;

                        case 4:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Monster94"), Main.player[npc.target].Center);
                            break;

                        case 5:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Monster5") { Volume = 1.5f }, Main.player[npc.target].Center);
                            break;

                        case 6:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Thunder") { Volume = 1.5f, Pitch = 1.5f }, Main.player[npc.target].Center);
                            break;

                        case 7:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Zombie_104"), Main.player[npc.target].Center);
                            break;

                        case 8:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Monster70"), Main.player[npc.target].Center);
                            break;

                        case 9:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Railgun"), Main.player[npc.target].Center);
                            break;

                        case 10:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/Navi"), Main.player[npc.target].Center);
                            break;

                        case 11:
                            if (!Main.dedServ)
                                SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/ZaWarudo") { Volume = 1.5f }, Main.player[npc.target].Center);
                            break;

                        default:
                            SoundEngine.PlaySound(SoundID.NPCDeath10, Main.player[npc.target].Center);
                            break;
                    }
                }
            }

            if (!InPhase2 && npc.life < npc.lifeMax / 2)
            {
                InPhase2 = true;
                SoundEngine.PlaySound(SoundID.Roar, npc.Center);
            }

            if (npc.ai[0] == 6f) //when approaching for roar
            {
                if (npc.ai[1] == 0f)
                {
                    npc.position += npc.velocity;
                }
                else if (npc.ai[1] == 1f)
                {
                    DoFuryRingAttack = true;
                }
            }

            if (DoFuryRingAttack)
            {
                npc.velocity = Vector2.Zero;

                if (FuryRingTimer == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<GlowRingHollow>(), FargoSoulsUtil.ScaledProjectileDamage(npc.damage, 4f / 3), 0f, Main.myPlayer, 4);

                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        if (NPC.CountNPCS(NPCID.DD2DarkMageT3) < 3)
                            FargoSoulsUtil.NewNPCEasy(npc.GetSource_FromAI(), npc.Center, NPCID.DD2DarkMageT3, target: npc.target);
                    }
                }

                FuryRingTimer++;
                if (FuryRingTimer % 2 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float rotation = FuryRingShotRotationCounter;
                        if (WorldSavingSystem.MasochistModeReal && FuryRingTimer >= 30 && FuryRingTimer <= 60)
                            rotation += 1; //staggers each wave instead of lining them up behind each other
                        Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, -Vector2.UnitY.RotatedBy(2 * Math.PI / 30 * rotation), ModContent.ProjectileType<BetsyFury>(), FargoSoulsUtil.ScaledProjectileDamage(npc.damage, 4f / 3), 0f, Main.myPlayer, npc.target);
                        Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, -Vector2.UnitY.RotatedBy(2 * Math.PI / 30 * -rotation), ModContent.ProjectileType<BetsyFury>(), FargoSoulsUtil.ScaledProjectileDamage(npc.damage, 4f / 3), 0f, Main.myPlayer, npc.target);
                    }
                    FuryRingShotRotationCounter++;
                }
                if (FuryRingTimer > (InPhase2 ? 90 : 30) + 2)
                {
                    DoFuryRingAttack = false;
                    InFuryRingAttackCooldown = true;
                    FuryRingTimer = 0;
                    FuryRingShotRotationCounter = 0;
                }

                EModeGlobalNPC.Aura(npc, 1200, BuffID.WitheredWeapon, true, 226);
                EModeGlobalNPC.Aura(npc, 1200, BuffID.WitheredArmor, true, 226);
            }

            if (InFuryRingAttackCooldown)
            {
                EModeGlobalNPC.Aura(npc, 1200, BuffID.WitheredWeapon, true, 226);
                EModeGlobalNPC.Aura(npc, 1200, BuffID.WitheredArmor, true, 226);

                if (++FuryRingShotRotationCounter > 90)
                {
                    InFuryRingAttackCooldown = false;
                    FuryRingTimer = 0;
                    FuryRingShotRotationCounter = 0;
                }
                npc.position -= npc.velocity * 0.5f;
                if (FuryRingTimer % 2 == 0)
                    return false;
            }

            if (!DD2Event.Ongoing && npc.HasPlayerTarget && (!Main.player[npc.target].active || Main.player[npc.target].dead || npc.Distance(Main.player[npc.target].Center) > 3000))
            {
                int p = Player.FindClosest(npc.Center, 0, 0); //extra despawn code for when summoned outside event
                if (p < 0 || !Main.player[p].active || Main.player[p].dead || npc.Distance(Main.player[p].Center) > 3000)
                    npc.active = false;
            }

            EModeUtils.DropSummon(npc, "BetsyEgg", WorldSavingSystem.DownedBetsy, ref DroppedSummon, NPC.downedGolemBoss);

            return true;
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.WitheredArmor, 600);
            target.AddBuff(BuffID.WitheredWeapon, 600);
            target.AddBuff(ModContent.BuffType<MutantNibbleBuff>(), 600);
        }

        public override bool SpecialOnKill(NPC npc)
        {
            npc.boss = false;

            return base.SpecialOnKill(npc);
        }

        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            WorldSavingSystem.DownedBetsy = true;
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasNPCTarget)
            {
                modifiers.Null();
                SoundEngine.PlaySound(SoundID.NPCHit4, npc.Center);
            }
            base.ModifyIncomingHit(npc, ref modifiers);
        }
        public override void LoadSprites(NPC npc, bool recolor)
        {
            base.LoadSprites(npc, recolor);

            LoadNPCSprite(recolor, NPCID.DD2Betsy);
            LoadBossHeadSprite(recolor, 34);
            LoadGoreRange(recolor, 1079, 1086);
            LoadExtra(recolor, 81);
            LoadExtra(recolor, 82);
        }
    }
}
