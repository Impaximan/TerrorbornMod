using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using Terraria.DataStructures;

namespace TerrorbornMod.Items
{
    class TerrorTonic : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("While this item is favorited in your inventory, dark energy will be stored in the tonic, up to a maximum of 5" +
                "\nUse the item to consume all stored dark energy, granting you 15% terror per energy instead of 10%" +
                "\nUsing this will also decrease your damage by 15% for 15 seconds");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 6));
        }

        public override void SetDefaults()
        {
            Item.expert = true;
            Item.width = 30;
            Item.height = 192 / 6;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.UseSound = SoundID.Item4;
            Item.noUseGraphic = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            tooltips.Insert(tooltips.FindIndex(x => x.Name == "Expert" && x.Mod == "Terraria"), new TooltipLine(Mod, "TerrorTonic", modPlayer.DarkEnergyStored + "/5 stored currently (" + (modPlayer.DarkEnergyStored * 15).ToString() + "%)"));
            tooltips.FirstOrDefault(x => x.Name == "TerrorTonic" && x.Mod == "TerrorbornMod").OverrideColor = Color.FromNonPremultiplied(108, 150, 143, 255);
        }

        public override void UpdateInventory(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (Item.favorited)
            {
                modPlayer.TerrorTonic = true;
            }
            itemFrame = modPlayer.DarkEnergyStored;
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.DarkEnergyStored == 0)
            {
                return false;
            }
            modPlayer.GainTerror(15f * modPlayer.DarkEnergyStored, false, false, false);
            modPlayer.DarkEnergyStored = 0;
            player.AddBuff(ModContent.BuffType<Buffs.Debuffs.BrainDead>(), 60 * 15);
            return base.CanUseItem(player);
        }

        float rotationTime = 0f;
        int itemFrame = 0;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            rotationTime++;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            frame = new Rectangle(0, Item.height * itemFrame, Item.width, Item.height);
            if (modPlayer.DarkEnergyStored == 5)
            {
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), position + Item.Size / 2 * scale, frame, drawColor, MathHelper.ToRadians((float)Math.Sin(rotationTime / 10) * 15f), Item.Size / 2, scale, SpriteEffects.None, 0f);
                return false;
            }
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), position + Item.Size / 2 * scale, frame, drawColor, 0f, Item.Size / 2, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            rotationTime++;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            Rectangle frame = new Rectangle(0, Item.height * itemFrame, Item.width, Item.height);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), Item.Center - Main.screenPosition, frame, lightColor, 0f, Item.Size / 2, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}