using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace TerrorbornMod.TwilightMode.NPCs
{
    class Vultures : TwilightNPCChange
    {
        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.Vulture;
        }

        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        int vultureAI_projectileCooldown = 30;
        int vultureAI_projectileCounter = 0;
        public override void NewAI(NPC NPC)
        {
            if (NPC.aiStyle == 17 && NPC.type == NPCID.Vulture)
            {
                if (NPC.ai[0] == 1 && !Main.player[NPC.target].dead)
                {
                    vultureAI_projectileCooldown--;
                    if (vultureAI_projectileCooldown <= 0)
                    {
                        vultureAI_projectileCooldown = 360;
                        vultureAI_projectileCounter = 60;
                        SoundEngine.PlaySound(SoundID.Item46, NPC.Center);

                    }
                }

                if (vultureAI_projectileCounter > 0)
                {
                    vultureAI_projectileCounter--;
                    NPC.velocity -= NPC.DirectionTo(Main.player[NPC.target].Center) * 14f / 60f;
                    if (vultureAI_projectileCounter <= 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item102, NPC.Center);
                        float spread = MathHelper.ToRadians(90f);
                        float amount = 5f;
                        if (Main.masterMode)
                        {
                            amount = 7f;
                        }
                        for (float i = -0.5f; i <= 0.5f; i += 1f / (amount - 1f))
                        {
                            Vector2 velocity = NPC.DirectionTo(Main.player[NPC.target].Center).RotatedBy(spread * i) * 7.5f;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<Projectiles.VultureFeather>(), NPC.damage / 6, 0f);
                        }
                        NPC.velocity -= NPC.DirectionTo(Main.player[NPC.target].Center) * 5f;
                    }
                }
                else
                {

                }
            }
        }

        public override bool RemoveOriginalAI(NPC npc)
        {
            return false;
        }
    }
}
