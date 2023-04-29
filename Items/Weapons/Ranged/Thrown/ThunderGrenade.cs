using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Ranged.Thrown
{
    class ThunderGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A grenade that explodes into lightning if enemies are nearby" +
                "\nThe lightning will home into enemies and try to hit more than one enemy"); */
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.damage = 36;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 22;
            Item.shoot = ModContent.ProjectileType<ThunderGrenadeProjectile>();
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.countAsThrown = true;
        }
    }

    class ThunderGrenadeProjectile : ModProjectile
    {
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            damage = (int)(damage * 0.75f);
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
            Projectile.velocity.Y += 0.18f;
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(360 / Streams * i + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth)), DustType, direction, 0, default, DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Item89, Projectile.Center);
            DustExplosion(Projectile.Center, 0, 12, 7, 62, 2f, true);

            List<int> NPCsTargeted = new List<int>();
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                bool foundSomething = false;
                bool foundAnything = false;
                NPC closest = Main.npc[0];
                NPC preferred = Main.npc[0];
                float distance = 4000;
                float preferredDistance = distance;

                foreach (NPC NPC in Main.npc)
                {
                    if (!NPC.friendly && NPC.CanBeChasedBy() && Projectile.CanHitWithOwnBody(NPC))
                    {
                        if (NPCsTargeted.Contains(NPC.whoAmI))
                        {
                            if (NPC.Distance(Projectile.Center) < distance)
                            {
                                foundAnything = true;
                                distance = NPC.Distance(Projectile.Center);
                                closest = NPC;
                            }
                        }
                        else if (NPC.Distance(Projectile.Center) < preferredDistance)
                        {
                            foundAnything = true;
                            foundSomething = true;
                            preferredDistance = NPC.Distance(Projectile.Center);
                            preferred = NPC;
                        }
                    }
                }

                if (foundAnything)
                {
                    if (foundSomething)
                    {
                        NPCsTargeted.Add(preferred.whoAmI);
                        SpawnLightning(preferred.whoAmI);
                    }
                    else
                    {
                        NPCsTargeted.Add(closest.whoAmI);
                        SpawnLightning(closest.whoAmI);
                    }
                }
            }
        }

        public void SpawnLightning(int target)
        {
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.NextFloat(10f, 25f);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), Projectile.damage / 2, 0.5f, Projectile.owner);
            Main.projectile[proj].DamageType = DamageClass.Ranged;
            Main.projectile[proj].ai[0] = target;
        }
    }
}