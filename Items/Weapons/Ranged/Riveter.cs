using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Riveter : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Holds up to 4 shots at a time, stop using to reload" +
                "\nUses rivets as ammo, which explode on contact with tiles" +
                "\nKnocks you backward slightly on use"); */
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.5f;
            Item.damage = 29;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 52;
            Item.height = 20;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.shoot = ModContent.ProjectileType<RivetProjectile>();
            Item.useAmmo = ModContent.ItemType<Rivet>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 25f;
            Item.crit = 20;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ModContent.RarityType<Rarities.Twilight>();
            Item.autoReuse = true;
            Item.shootSpeed = 16f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        int useCounter = 0;
        int shotsLeft = 0;
        public override bool CanUseItem(Player player)
        {
            return shotsLeft > 0 && !(useCounter > 0 && useCounter <= 10);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            useCounter = 70;
            shotsLeft--;
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/RiveterSound").Value.Play(Main.soundVolume * 1f, 0f, 0f);
            Vector2 EditedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(2f)) * Main.rand.NextFloat(1f, 1.1f);
            Projectile.NewProjectile(source, position, EditedSpeed, type, damage, knockback, player.whoAmI);
            TerrorbornSystem.ScreenShake(1.5f);
            player.velocity -= velocity.ToRotation().ToRotationVector2() * 2.5f;
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (useCounter > 0)
            {
                useCounter--;
                if (useCounter == 10)
                {
                    ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/RiveterDrawSound").Value.Play(Main.soundVolume * 1f, 0f, 0f);
                }
                if (useCounter <= 0)
                {
                    CombatText.NewText(player.getRect(), Color.White, "Reloaded");
                    TerrorbornSystem.ScreenShake(1f);
                }
            }
            else
            {
                shotsLeft = 4;
            }
        }
    }

    class Rivet : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 12;
            Item.height = 14;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.shootSpeed = 20;
            Item.rare = ModContent.RarityType<Rarities.Twilight>();
            Item.shoot = ModContent.ProjectileType<RivetProjectile>();
            Item.ammo = Item.type;
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Used by the riveter" +
                "\nSold by the merchant while the riveter is in your inventory"); */
        }
    }

    class RivetProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Rivet";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cartilage Round");
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            fallThrough = Main.MouseWorld.Y > Projectile.Center.Y;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 6;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity /= 4;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
        }

        int BouncesLeft = 5;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft != 0)
            {
                for (int i = 0; i < Main.rand.Next(25, 35); i++)
                {
                    float maxSpeed = 5f;
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(-maxSpeed, maxSpeed), Main.rand.NextFloat(-maxSpeed, maxSpeed));
                    int dust = Dust.NewDust(Projectile.Center, 1, 1, 6);
                    Main.dust[dust].velocity = velocity;
                    Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.5f);
                }
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    float Speed = Main.rand.Next(2, 4);
                    Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                    Gore.NewGore(Projectile.GetSource_Misc("RivetExplosion"), Projectile.Center, ProjectileSpeed, Main.rand.Next(825, 828));
                }
                TerrorbornSystem.ScreenShake(2f);
                SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
                float maxDistance = 80f;
                if (Main.LocalPlayer.Distance(Projectile.Center) <= maxDistance)
                {
                    Main.LocalPlayer.velocity = Main.LocalPlayer.DirectionFrom(Projectile.Center) * MathHelper.Lerp(26.5f, 3f, Main.LocalPlayer.Distance(Projectile.Center) / maxDistance);
                }
            }
        }
    }
}