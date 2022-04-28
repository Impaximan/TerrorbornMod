using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class TidalConcoction : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetBurstJumpString((int)(60 * 1.5f)) +
                "\nCreates azure geysers below you upon activation");
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.AzuriteOre>(), 15)
                .AddIngredient(ModContent.ItemType<Materials.CrackedShell>(), 2)
                .AddTile(TileID.Bottles)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.damage = 60;
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TBUtils.Accessories.UpdateBurstJump((int)(60 * 1.5f), 60 * 1, Item, player, new Vector2(20, -15), Color.Azure, SoundID.Item14);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.BurstJumpTime > 0)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 88)];
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            if (modPlayer.JustBurstJumped)
            {
                for (int i = 0; i < Main.rand.Next(2, 5); i++)
                {
                    Vector2 position = player.Center;
                    if (i != 1)
                    {
                        position.X += Main.rand.Next(-200, 200);
                    }
                    while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                        {
        new Conditions.IsSolid()
                        }), out _))
                    {
                        position.Y++;
                    }
                    int proj = Projectile.NewProjectile(player.GetSource_Accessory(Item), position, new Vector2(0, -20), ModContent.ProjectileType<Items.Equipable.Armor.TideFireFriendly>(), Item.damage, 0f, player.whoAmI);
                    Main.projectile[proj].DamageType = DamageClass.Default;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item88, player.Center);
            }
        }
    }
}

