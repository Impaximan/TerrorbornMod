using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
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
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 10;
            Item.knockBack = knockback;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            TerrorbornItem.modItem(Item).parryShield = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeetleHusk, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.MediumPurple;
            if (modPlayer.JustParried)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<OrbitingBeetle>(), 220, 0f, player.whoAmI);
                int proj = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<OrbitingBeetle>(), 220, 0f, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, Item, player);
        }
    }


    class OrbitingBeetle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 40;
            Projectile.height = 34;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 8 * 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 2;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        float rotation = 0f;
        float currentDistance = 0f;
        public override void AI()
        {
            FindFrame(Projectile.height);

            Player player = Main.player[Projectile.owner];
            currentDistance = MathHelper.Lerp(currentDistance, player.Distance(Main.MouseWorld), 0.1f);
            if (currentDistance > 500)
            {
                currentDistance = 500;
            }
            Projectile.spriteDirection = Math.Sign(Main.MouseWorld.X - player.Center.X);
            rotation += MathHelper.ToRadians(12f);
            Projectile.position = player.Center + rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(180 * Projectile.ai[0])) * currentDistance - Projectile.Size / 2;
        }
    }
}

