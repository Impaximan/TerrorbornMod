using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Melee.Swords
{
    public class IcarusShred : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 10;
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 8)
                .AddIngredient(ItemID.CrimtaneBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 8)
                .AddIngredient(ItemID.DemoniteBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Teleports you to a nearby enemy when used" +
                "\nGives you some of your flight time back on hit");
        }

        public override void SetDefaults()
        {
            Item.damage = 72;
            Item.DamageType = DamageClass.Melee;
            Item.width = 70;
            Item.height = 66;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.MagicMissile;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float distance = 1000;
            NPC target = Main.npc[0];
            bool targetted = false;
            foreach (NPC NPC in Main.npc)
            {
                if (!NPC.friendly && NPC.active && NPC.Distance(player.Center) <= distance && NPC.CanBeChasedBy())
                {
                    target = NPC;
                    distance = NPC.Distance(player.Center);
                    targetted = true;
                }
            }

            if (targetted && distance > 200)
            {
                int direction = 1;
                if (player.Center.X < target.Center.X)
                {
                    direction = -1;
                }
                position = target.Center + new Vector2(target.width / 2 + 50, 0) * direction;

                if (Collision.CanHitLine(player.position, player.width, player.height, target.position + new Vector2(target.width / 2 + 50, 0) * direction, target.width, target.height))
                {
                    player.Teleport(position - player.Size / 2);
                    player.velocity = target.velocity;
                }
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.wingTime += (player.wingTimeMax - player.wingTime) / 5;
        }
    }
}