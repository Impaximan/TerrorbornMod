using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;

namespace TerrorbornMod.WeaponPossession
{
    class PossessedItem : GlobalItem
    {
        public override void PostReforge(Item item)
        {
            possessType = PossessType.None;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            if (possessType == PossessType.Might)
            {
                mult *= 1.15f;
            }

            if (possessType == PossessType.Flight)
            {
                mult *= 0.85f;
            }

            if (possessType == PossessType.Sight)
            {
                mult *= 0.85f;
            }
        }

        public override void GetWeaponKnockback(Item item, Player player, ref float knockback)
        {
            base.GetWeaponKnockback(item, player, ref knockback);
            if (possessType == PossessType.Might)
            {
                knockback *= 1.5f;
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (possessType == PossessType.Might)
            {
                modPlayer.allUseSpeed *= 0.85f;
            }
        }

        public override bool NeedsSaving(Item item)
        {
            return true;
        }

        public int possessType = PossessType.None;

        public override TagCompound Save(Item item)
        {
            return new TagCompound
            {
                { "possessType", possessType }
            };
        }

        public override void Load(Item item, TagCompound tag)
        {
            possessType = tag.GetInt("possessType");
        }

        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public static PossessedItem modItem(Item item)
        {
            return item.GetGlobalItem<PossessedItem>();
        }

        //public override GlobalItem Clone(Item item, Item itemClone)
        //{
        //    PossessedItem clone = (PossessedItem)base.Clone(item, itemClone);
        //    clone.possessType = possessType;
        //    return clone;
        //}

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (possessType == PossessType.None || item.accessory || item.damage <= 0 || item.consumable)
            {
                return;
            }

            TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.mod == "Terraria");
            tooltips.Insert(1, new TooltipLine(mod, "SoulType", "Possessed by Souls of " + PossessType.ToString(possessType) + " [i/s1:" + PossessType.ToItemType(possessType) + "]"));
            tooltips.FirstOrDefault(x => x.Name == "SoulType" && x.mod == "TerrorbornMod").overrideColor = PossessType.ToColor(possessType);

            string bonus = "";
            string drawback = "";
            if (possessType == PossessType.Plight)
            {
                bonus = "+Projectiles have a 15% chance to create a weaker clone of themselves on hit";
                drawback = "+Placeholder";
            }
            if (possessType == PossessType.Might)
            {
                bonus = "+Projectiles move 2x as fast" +
                    "\n+50% knockback" +
                    "\n+15% damage";
                drawback = "-15% use speed";
            }
            if (possessType == PossessType.Flight)
            {
                bonus = "+Projectiles can travel through walls";
                drawback = "-15% damage" +
                    "\n-25% velocity";
            }
            if (possessType == PossessType.Sight)
            {
                bonus = "+Projectiles will home into enemies";
                drawback = "-15% damage" +
                    "\n-35% velocity";
            }
            tooltips.Add(new TooltipLine(mod, "SoulBonus", bonus));
            tooltips.FirstOrDefault(x => x.Name == "SoulBonus" && x.mod == "TerrorbornMod").overrideColor = Color.Lerp(PossessType.ToColor(possessType), Color.White, 0.25f);

            if (drawback != "")
            {
                tooltips.Add(new TooltipLine(mod, "SoulDrawback", drawback));
                tooltips.FirstOrDefault(x => x.Name == "SoulDrawback" && x.mod == "TerrorbornMod").overrideColor = Color.Lerp(PossessType.ToColor(possessType), Color.Black, 0.25f);
            }
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (possessType == PossessType.None || item.accessory || item.damage <= 0 || item.consumable)
            {
                return;
            }

            Texture2D texture = ModContent.GetTexture("TerrorbornMod/WeaponPossession/Icons/" + PossessType.ToString(possessType));
            spriteBatch.Draw(texture, position + new Vector2(frame.Width, frame.Height) * Main.UIScale * scale, null, Color.White, 0f, texture.Size(), 0.75f * Main.UIScale, SpriteEffects.None, 0f);
        }
    }
}
