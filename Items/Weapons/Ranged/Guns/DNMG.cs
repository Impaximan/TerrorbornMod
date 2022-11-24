using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace TerrorbornMod.Items.Weapons.Ranged.Guns
{
    class DNMG : ModItem
    {
        int baseReuseDelay = 13;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("D.N.M.G");
            Tooltip.SetDefault("Fires faster the longer you use it" +
                "\nUpon reaching max speed it will fire a mini nuke that does not destroy tiles and reset its speed");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.width = 112;
            Item.height = 46;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.knockBack = 1f;
            Item.UseSound = new SoundStyle("TerrorbornMod/Sounds/Effects/CoolerMachineGun");

            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            Item.scale = 1f;
            Item.channel = true;
            Item.reuseDelay = baseReuseDelay;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, -2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DreadfulEssence>(), 3)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<Bonezooka>())
                .AddIngredient(ItemID.FragmentVortex, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                Item.reuseDelay = baseReuseDelay;
            }
        }

        bool nextShotIsNuke = false;
        public override bool CanUseItem(Player player)
        {
            TerrorbornSystem.ScreenShake(0.5f);
            Item.reuseDelay -= 2;
            if (Item.reuseDelay <= 0)
            {
                Item.reuseDelay = baseReuseDelay;
                nextShotIsNuke = true;
                TerrorbornSystem.ScreenShake(1.5f);
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (nextShotIsNuke)
            {
                nextShotIsNuke = false;
                SoundStyle style = new SoundStyle("TerrorbornMod/Sounds/Effects/Gunfire1");
                style.Volume *= 0.75f;
                style.PitchVariance = 0.15f;
                SoundEngine.PlaySound(style, player.Center);
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<MiniNuke>(), damage * 2, knockback * 5f, player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }

    class MiniNuke : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 34;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 50, 25, DustID.GoldFlame, DustScale: 1.5f, NoGravity: true);
            SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 200; i++)
            {
                NPC NPC = Main.npc[i];
                if (!NPC.friendly && Projectile.Distance(NPC.Center) <= 200 + (NPC.width + NPC.height) / 2 && !NPC.dontTakeDamage)
                {
                    NPC.StrikeNPC(Projectile.damage, 0, 0, Main.rand.Next(101) <= Main.player[Projectile.owner].GetCritChance(DamageClass.Ranged));
                    NPC.AddBuff(BuffID.Daybreak, 60 * 10);
                }
            }
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

                Dust dust = Dust.NewDustPerfect(position + new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth)), DustType, direction * Main.rand.NextFloat(0.8f, 1.2f), 0, default, DustScale);
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
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(90);
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
                float speed = 1.2f;
                Projectile.velocity += Projectile.DirectionTo(targetNPC.Center) * speed;
                Projectile.velocity *= 0.95f;
            }
            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GoldFlame, Vector2.Zero, 0, Scale: 1f);
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
