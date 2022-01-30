using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerrorbornMod.TBUtils;
using System;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    class BeetleShield : ModItem
    {
        int cooldown = 8 * 60;
        float knockback = 10f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback) + "\nParrying attacks summons beetles that will spin around you and damage enemies");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.defense = 10;
            item.knockBack = knockback;
            item.value = Item.sellPrice(0, 15, 0, 0);
            TerrorbornItem.modItem(item).parryShield = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BeetleHusk, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.MediumPurple;
            if (modPlayer.JustParried)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<OrbitingBeetle>(), 220, 0f, player.whoAmI);
                int proj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<OrbitingBeetle>(), 220, 0f, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, item, player);
        }
    }


    class OrbitingBeetle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 40;
            projectile.height = 34;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 8 * 60;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 2;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        float rotation = 0f;
        float currentDistance = 0f;
        public override void AI()
        {
            FindFrame(projectile.height);

            Player player = Main.player[projectile.owner];
            currentDistance = MathHelper.Lerp(currentDistance, player.Distance(Main.MouseWorld), 0.1f);
            if (currentDistance > 500)
            {
                currentDistance = 500;
            }
            projectile.spriteDirection = Math.Sign(Main.MouseWorld.X - player.Center.X);
            rotation += MathHelper.ToRadians(12f);
            projectile.position = player.Center + rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(180 * projectile.ai[0])) * currentDistance - projectile.Size / 2;
        }
    }
}

