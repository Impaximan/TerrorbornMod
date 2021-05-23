using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class VBMG : ModItem
    {
        int UntilBlast;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("V.B.M.G");
            Tooltip.SetDefault("Fires a piercing vein burst every few shots that heals you upon striking enemies\n'Vein Bursting Machine Gun'");
        }
        public override void SetDefaults()
        {
            item.damage = 6;
            item.ranged = true;
            item.noMelee = true;
            item.width = 38;
            item.height = 18;
            item.useTime = 8;
            item.useAnimation = 8;
            item.shoot = 10;
            item.useStyle = 5;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            UntilBlast--;
            if (UntilBlast <= 0)
            {
                UntilBlast = 4;
                Main.PlaySound(SoundID.Item41, position);
                Projectile.NewProjectile(position.X, position.Y, speedX / 5, speedY / 5, mod.ProjectileType("VeinBurst"), damage, knockBack, player.whoAmI);
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 8);
            recipe.AddIngredient(ItemID.TissueSample, 5);
            recipe.AddIngredient(mod.ItemType("SanguineFang"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.DemoniteBar, 8);
            recipe2.AddIngredient(ItemID.ShadowScale, 5);
            recipe2.AddIngredient(mod.ItemType("SanguineFang"), 12);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }

    class VeinBurst : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.hide = true;
            projectile.timeLeft = 100;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 1.5f;
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
            //NPC targetNPC = Main.npc[0];
            //float Distance = 500; //max distance away
            //bool Targeted = false;
            //for (int i = 0; i < 200; i++)
            //{
            //    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && projectile.CanHit(Main.npc[i]))
            //    {
            //        targetNPC = Main.npc[i];
            //        Distance = Main.npc[i].Distance(projectile.Center);
            //        Targeted = true;
            //    }
            //}
            //if (Targeted)
            //{
            //    //HOME IN
            //    float speed = .5f;
            //    Vector2 move = targetNPC.Center - projectile.Center;
            //    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            //    move *= speed / magnitude;
            //    projectile.velocity += move;
            //}
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            int healAmount = Main.rand.Next(1, 3);
            player.HealEffect(healAmount);
            player.statLife += healAmount;
        }
    }
}

