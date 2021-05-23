using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class GaussStriker : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to fire an accelerating, high knockback shockwave." +
                "\nHitting enemies with this shockwave will make them temporarily weaker to your bolts.");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MythrilBar);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddIngredient(ItemID.SpaceGun);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.OrichalcumBar);
            recipe2.AddIngredient(ItemID.SoulofNight, 15);
            recipe2.AddIngredient(ItemID.SpaceGun);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetDefaults()
        {
            item.damage = 30;
            item.noMelee = true;
            item.width = 48;
            item.height = 26;
            item.useTime = 8;
            item.shoot = 10;
            item.useAnimation = 8;
            item.useStyle = 5;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item12;
            item.autoReuse = true;
            item.shootSpeed = 25f;
            item.shoot = mod.ProjectileType("GaussBolt");
            item.mana = 3;
            item.magic = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.reuseDelay = 20;
                item.shoot = mod.ProjectileType("GaussShockwave");
                item.shootSpeed = 1f;
            }
            else
            {
                item.shoot = mod.ProjectileType("GaussBolt");
                item.shootSpeed = 25f;
                item.reuseDelay = 0;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                damage *= 2;
                knockBack = 13;
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }
    class GaussBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 30;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 400;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 5;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.HasBuff(ModContent.BuffType<GaussWeakness>()))
            {
                damage = (int)(damage * 1.75f);
            }
        }
        public override void AI()
        {
            FindFrame(projectile.height);
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
        }
    }
    class GaussShockwave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 22;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 400;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<GaussWeakness>(), 60 * 5);
        }
        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 5;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        public override void AI()
        {
            FindFrame(projectile.height);
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 1f;
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
        }
    }
    class GaussWeakness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Gauss Weakness");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 21, 0, 0, Scale: 2);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = npc.velocity;
        }
    }

}


