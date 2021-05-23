using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusGodblade : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe.AddIngredient(ItemID.CobaltBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe2.AddIngredient(ItemID.PalladiumBar, 15);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Critical hits will strike twice\nTrue melee damage does twice as much damage");
        }
        public override void SetDefaults()
        {
            item.damage = 32;
            item.melee = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("IncendiusSlash");
            item.shootSpeed = 15;
            item.crit = 7;
            item.autoReuse = true;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            damage = (int)(damage * 2);
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (crit)
            {
                target.StrikeNPC(damage, 0, 0);
            }
        }
    }
    class IncendiusSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 26;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
            
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 25, 15, DustID.Fire, DustScale: 1.5f, NoGravity: true);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
            if (crit)
            {
                target.StrikeNPC(damage, 0, 0);
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
    }
}
