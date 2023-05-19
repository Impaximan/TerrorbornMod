using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerrorbornMod.WeaponPossession
{
    class PossessedItem : GlobalItem
    {
        public override void PostReforge(Item item)
        {
            possessType = PossessType.None;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (possessType == PossessType.Might)
            {
                damage *= 1.15f;
            }

            if (possessType == PossessType.Flight)
            {
                damage *= 0.85f;
            }

            if (possessType == PossessType.Night)
            {
                damage *= 0.85f;
            }

            if (possessType == PossessType.Fright)
            {
                damage *= 0.9f;
            }

            if (possessType == PossessType.Sight)
            {
                damage *= 0.85f;
            }

            if (possessType == PossessType.Plight)
            {
                damage *= 1.5f;
            }
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            if (possessType == PossessType.Might)
            {
                knockback *= 1.5f;
            }
            base.ModifyWeaponKnockback(item, player, ref knockback);
        }

        public override void HoldItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (possessType == PossessType.Might)
            {
                player.GetAttackSpeed(DamageClass.Generic) *= 0.85f;
            }
            if (possessType == PossessType.Plight)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
            }
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("possessType", possessType);
        }

        public int possessType = PossessType.None;

        public override void LoadData(Item item, TagCompound tag)
        {
            possessType = tag.GetInt("possessType");
        }

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            //modItem(itemClone).possessType = modItem(item).possessType;
            return base.Clone(item, itemClone);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (possessType == PossessType.Light && hit.Crit)
            {
                Projectile.NewProjectile(target.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<Lightsplosion>(), hit.Damage / 2, 0, player.whoAmI);
                TerrorbornSystem.ScreenShake(5f);
                SoundExtensions.PlaySoundOld(SoundID.Item68, target.Center);
            }

            if (possessType == PossessType.Night && hit.Crit)
            {
                player.HealEffect(1);
                player.statLife++;
            }
        }

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

            TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.Mod == "Terraria");
            tooltips.Insert(1, new TooltipLine(Mod, "SoulType", "Possessed by Souls of " + PossessType.ToString(possessType) + " [i/s1:" + PossessType.ToItemType(possessType) + "]"));
            tooltips.FirstOrDefault(x => x.Name == "SoulType" && x.Mod == "TerrorbornMod").OverrideColor = PossessType.ToColor(possessType);

            string bonus = "";
            string drawback = "";
            if (possessType == PossessType.Plight)
            {
                drawback = "Holding this inflicts you with the 'Midnight Flames' debuff";
                bonus = "+50% damage";
            }
            if (possessType == PossessType.Might)
            {
                bonus = "Projectiles move 2x as fast" +
                    "\n+50% knockback" +
                    "\n+15% damage";
                drawback = "-15% use speed";
            }
            if (possessType == PossessType.Flight)
            {
                bonus = "Projectiles can travel through walls";
                drawback = "-15% damage" +
                    "\n-25% velocity";
            }
            if (possessType == PossessType.Sight)
            {
                bonus = "Projectiles will home into enemies";
                drawback = "-15% damage" +
                    "\n-35% velocity";
            }
            if (possessType == PossessType.Fright)
            {
                bonus = "Steals terror from enemies" +
                    "\n-10% damage";
            }
            if (possessType == PossessType.Light)
            {
                bonus = "Critical hits cause an explosion of light";
            }
            if (possessType == PossessType.Night)
            {
                bonus = "Lifesteals on critical hits";
                drawback = "-15% damage";
            }

            tooltips.Add(new TooltipLine(Mod, "SoulBonus", bonus));
            tooltips.FirstOrDefault(x => x.Name == "SoulBonus" && x.Mod == "TerrorbornMod").OverrideColor = Color.Lerp(PossessType.ToColor(possessType), Color.White, 0.25f);

            if (drawback != "")
            {
                tooltips.Add(new TooltipLine(Mod, "SoulDrawback", drawback));
                tooltips.FirstOrDefault(x => x.Name == "SoulDrawback" && x.Mod == "TerrorbornMod").OverrideColor = Color.Lerp(PossessType.ToColor(possessType), Color.Black, 0.25f);
            }
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (possessType == PossessType.None || item.accessory || item.damage <= 0 || item.consumable)
            {
                return;
            }

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/WeaponPossession/Icons/" + PossessType.ToString(possessType));
            spriteBatch.Draw(texture, position + new Vector2(frame.Width, frame.Height) * Main.UIScale * scale, null, Color.White, 0f, texture.Size(), 0.75f * Main.UIScale, SpriteEffects.None, 0f);
        }
    }
}
