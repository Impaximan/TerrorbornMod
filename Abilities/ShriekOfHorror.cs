using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Abilities
{
    class ShriekOfHorror : TerrorAbility
    {
        public override string TexturePath => "Abilities/ShriekOfHorror_Icon";

        public override Vector2 lockedPosition()
        {
            return TerrorbornSystem.ShriekOfHorror;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(20, 24);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(12, 0);
        }

        public override void ActualAI()
        {
            Float(1.5f, 0.1f);
            UpdateObtainablity(30);
        }

        public override void ObtainAbility()
        {
            TerrorbornSystem.obtainedShriekOfHorror = true;
            Projectile.active = false;

            Vector2 terrorMasterPosition = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16);
            NPC.NewNPC(new EntitySource_WorldEvent(), (int)terrorMasterPosition.X, (int)terrorMasterPosition.Y, ModContent.NPCType<NPCs.TownNPCs.TerrorMaster>());
            Main.NewText("??? the Terror Master has arrived!", new Color(50, 125, 255));

            TerrorbornPlayer target = TerrorbornPlayer.modPlayer(Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)]);
            target.TriggerAbilityAnimation("Shriek of Horror", "Hold the 'Shriek of Horror' mod hotkey to unleash a scream that generates terror while close to enemies.", "Getting hit while doing so will cause you to take twice as much damage", 0, "Special abilities and items will consume terror", 800);
        }
    }
}
