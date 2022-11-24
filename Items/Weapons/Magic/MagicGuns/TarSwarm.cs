using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace TerrorbornMod.Items.Weapons.Magic.MagicGuns
{
    class TarSwarm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Raplidly fires antlion larvae." +
                "\nSlightly ignores enemy defense.");
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.15f;
            Item.damage = 8;
            Item.noMelee = true;
            Item.width = 52;
            Item.height = 40;
            Item.useTime = 5;
            Item.scale = 0.8f;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AntlionLarva>();
            Item.shootSpeed = 15f;
            Item.mana = 4;
            Item.DamageType = DamageClass.Magic; ;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 rotatedVelocity = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(7));
                Projectile.NewProjectile(source, position, rotatedVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeeGun)
                .AddIngredient(ItemID.BeeWax, 10)
                .AddIngredient<Materials.TarOfHunger>(85)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    class AntlionLarva : ModProjectile
    {
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 5;
        }
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 4;
            //ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
            Main.projFrames[Projectile.type] = 3;
        }
        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 1;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }
        //public override bool PreDraw(ref Color lightColor)
        //{
        //    //Thanks to Seraph for afterimage code.
        //    Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
        //    for (int i = 0; i < Projectile.oldPos.Length; i++)
        //    {
        //        Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
        //        Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
        //        spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
        //    }
        //    return false;
        //}
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }
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
            return false;
        }
        public override void AI()
        {
            FindFrame(Projectile.height);
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(90);
            NPC targetNPC = Main.npc[0];
            float Distance = 375; //max distance away
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
                Vector2 direction = Projectile.DirectionTo(targetNPC.Center);
                Projectile.velocity += speed * direction;
                Projectile.velocity *= 0.98f;
            }
        }
    }
}
