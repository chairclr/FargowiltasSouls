using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class MiniSaucer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Saucer");
            //ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 25;
            projectile.height = 25;
            projectile.scale = 1.5f;
            projectile.timeLeft *= 5;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.active && !player.dead && player.GetModPlayer<FargoPlayer>().MiniSaucer)
                projectile.timeLeft = 2;

            if (projectile.damage == 0)
                projectile.damage = (int)(80f * player.minionDamage);

            NPC minionAttackTargetNpc = projectile.OwnerMinionAttackTargetNPC;
            if (minionAttackTargetNpc != null && projectile.ai[0] != minionAttackTargetNpc.whoAmI && minionAttackTargetNpc.CanBeChasedBy(projectile))
            {
                projectile.ai[0] = minionAttackTargetNpc.whoAmI;
                projectile.netUpdate = true;
            }

            if (projectile.ai[0] >= 0 && projectile.ai[0] < 200) //has target
            {
                NPC npc = Main.npc[(int)projectile.ai[0]];

                if (npc.CanBeChasedBy(projectile))
                {
                    Vector2 distance = npc.Center - projectile.Center;
                    distance.Y -= 150;
                    float length = distance.Length();
                    if (length > 100f)
                    {
                        distance /= 16f;
                        projectile.velocity = (projectile.velocity * 23f + distance) / 24f;
                    }
                    else
                    {
                        if (projectile.velocity.Length() < 12f)
                            projectile.velocity *= 1.05f;
                    }

                    if (++projectile.localAI[0] > 30f)
                    {
                        projectile.localAI[0] = 0f;
                        if (player.whoAmI == Main.myPlayer)
                        {
                            Projectile.NewProjectile(projectile.Center, new Vector2(8f, 0f).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143),
                                mod.ProjectileType("SaucerRocket"), projectile.damage, projectile.knockBack * 2f, projectile.owner, npc.whoAmI, 20f);
                        }
                    }

                    if (++projectile.localAI[1] > 5f)
                    {
                        projectile.localAI[1] = 0f;
                        Vector2 vel = distance;
                        vel.Normalize();
                        vel *= 16f;
                        Main.PlaySound(SoundID.Item12, projectile.Center);
                        if (player.whoAmI == Main.myPlayer)
                        {
                            Projectile.NewProjectile(projectile.Center, vel.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143 / 3.0),
                                mod.ProjectileType("SaucerLaser"), projectile.damage / 2, projectile.knockBack / 2f, projectile.owner);
                        }
                    }
                }
                else //forget target
                {
                    projectile.ai[0] = -1f;
                    projectile.netUpdate = true;
                }
            }
            else //no target
            {
                Vector2 target = player.Center - projectile.Center;
                target.Y -= 100f;
                float length = target.Length();
                if (length > 2000f)
                {
                    projectile.Center = player.Center;
                    projectile.ai[0] = -1f;
                    projectile.netUpdate = true;
                }
                else if (length > 100f)
                {
                    target.Normalize();
                    target *= (length > 200f) ? 10f : 6f;
                    projectile.velocity = (projectile.velocity * 30f + target) / 31f;
                }

                projectile.localAI[1]++;
                if (projectile.localAI[1] > 6f)
                {
                    projectile.localAI[1] = 0f;

                    float maxDistance = 1000f;
                    int possibleTarget = -1;
                    for (int i = 0; i < 200; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy(projectile))// && Collision.CanHitLine(projectile.Center, 0, 0, npc.Center, 0, 0))
                        {
                            float npcDistance = player.Distance(npc.Center);
                            if (npcDistance < maxDistance)
                            {
                                maxDistance = npcDistance;
                                possibleTarget = i;
                            }
                        }
                    }

                    if (possibleTarget >= 0)
                    {
                        projectile.ai[0] = possibleTarget;
                        projectile.netUpdate = true;
                    }
                }
            }

            //projectile.rotation = projectile.velocity.ToRotation();
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Electrified, 360);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}