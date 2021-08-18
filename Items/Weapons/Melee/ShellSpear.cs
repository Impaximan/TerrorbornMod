using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class ShellSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rapidly hits enemies");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 8);
            recipe.AddIngredient(ItemID.IronBar, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 48;
            item.height = 50;
            item.damage = 8;
            item.melee = true;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.useTime = 10;
            item.useAnimation = 10;
            item.shootSpeed = 6;
            item.knockBack = 1;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ShellSpearProjectile");
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }
    class ShellSpearProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 5;
        }

        public float movementFactor
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {

            Player projOwner = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    projectile.netUpdate = true;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3)
                {
                    movementFactor -= 2.4f;
                }
                else
                {
                    movementFactor += 2.1f;
                }
            }
            projectile.position += projectile.velocity * movementFactor;
            if (projOwner.itemAnimation == 0)
            {
                projectile.Kill();
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (projOwner.direction == 1)
            {
                projectile.spriteDirection = -1;
            }
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }
}

