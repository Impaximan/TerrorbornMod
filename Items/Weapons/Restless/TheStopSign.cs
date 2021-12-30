using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class TheStopSign : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/TheStopSign";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 4);
            recipe.AddIngredient(ItemID.TitaniumBar, 15);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 15);
            recipe.AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 4);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 4);
            recipe2.AddIngredient(ItemID.AdamantiteBar, 15);
            recipe2.AddRecipeGroup(RecipeGroupID.IronBar, 15);
            recipe2.AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 4);
            recipe2.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override string defaultTooltip()
        {
            return "It's a stop sign...";
        }

        public override string altTooltip()
        {
            return "The coolest weapon" +
                "\nBeats the target's soul right out of there body on hit dealing massive damage to them and other nearby enemies";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 160;
            item.melee = true;
            item.width = 128;
            item.height = 128;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.DD2_MonkStaffSwing;
            item.shoot = ModContent.ProjectileType<TheStopSoul>();
            item.shootSpeed = 20;
            item.crit = 7;
            item.autoReuse = true;
            item.noMelee = false;
            item.noUseGraphic = false;
            modItem.restlessTerrorDrain = 6f;
            modItem.restlessChargeUpUses = 5;
        }

        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                Main.PlaySound(SoundID.Item37, target.Center);
                TerrorbornMod.ScreenShake(10f);
                Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<TheStopSoul>(), 1250, 0f, player.whoAmI);
            }
        }
    }

    class TheStopSoul : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 20;
        const int defaultSize = 1000;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
            projectile.width = defaultSize;
            projectile.height = defaultSize;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, currentSize, Color.Pink);
            return false;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}
