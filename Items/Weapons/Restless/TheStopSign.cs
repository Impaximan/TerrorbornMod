using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

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
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 4)
                .AddIngredient(ItemID.TitaniumBar, 15)
                .AddRecipeGroup(RecipeGroupID.IronBar, 15)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 4)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 4)
                .AddIngredient(ItemID.AdamantiteBar, 15)
                .AddRecipeGroup(RecipeGroupID.IronBar, 15)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 4)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
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
            modItem.critDamageMult = 1.2f;
            Item.damage = 160;
            Item.DamageType = DamageClass.Melee;
            Item.width = 128;
            Item.height = 128;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.shoot = ModContent.ProjectileType<TheStopSoul>();
            Item.shootSpeed = 20;
            Item.crit = 7;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.noUseGraphic = false;
            modItem.restlessTerrorDrain = 6f;
            modItem.restlessChargeUpUses = 5;
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                SoundExtensions.PlaySoundOld(SoundID.Item37, target.Center);
                TerrorbornSystem.ScreenShake(10f);
                Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<TheStopSoul>(), 1250, 0f, player.whoAmI);
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
            Projectile.width = defaultSize;
            Projectile.height = defaultSize;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Utils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, currentSize, Color.Pink);
            return false;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}
