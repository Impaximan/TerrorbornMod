using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Bonezooka : ModItem
    {
        int UntilBlast;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses Rockets as ammo" +
                "\nFires homing skulls that explode on contact with enemies, dealing 70 extra damage to the hit enemy and" +
                "\nnearby enemies");
        }

        public override void SetDefaults()
        {
            Item.damage = 63;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 58;
            Item.height = 26;
            Item.useTime = 25;
            Item.shoot = ModContent.ProjectileType<BonezookaSkull>();
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Rocket;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<BonezookaSkull>();
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }

    class BonezookaSkull : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 36;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 45, 15, 172, DustScale: 1f, NoGravity: true);
            SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 200; i++)
            {
                NPC NPC = Main.npc[i];
                if (!NPC.friendly && Projectile.Distance(NPC.Center) <= 100 + ((NPC.width + NPC.height) / 2) && !NPC.dontTakeDamage)
                {
                    NPC.StrikeNPC(NPC.CalculateHitInfo(70, 0, Main.rand.Next(101) <= Main.player[Projectile.owner].GetCritChance(DamageClass.Ranged), 0f));
                }
            }
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void AI()
        {
            if (Projectile.velocity.X <= 0)
            {
                Projectile.spriteDirection = -1;
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(90);
            NPC targetNPC = Main.npc[0];
            float Distance = 500; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(Projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                float speed = .6f;
                Vector2 move = targetNPC.Center - Projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                Projectile.velocity += move;
                Projectile.velocity *= 0.98f;
            }
            Dust dust = Dust.NewDustPerfect(Projectile.Center, 172, Vector2.Zero, 0, Scale: 0.8f);
            dust.noGravity = true;
        }

        int bouncesLeft = 3;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            bouncesLeft--;
            if (bouncesLeft <= 0)
            {
                Projectile.timeLeft = 0;
            }
            return false;
        }
    }
}
