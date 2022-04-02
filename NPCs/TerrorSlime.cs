using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class TerrorSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BlueSlime);
            NPC.width = 32;
            NPC.height = 26;
            NPC.damage = 13;
            NPC.defense = 2;
            NPC.lifeMax = 35;
            NPC.value *= 2;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.color = Color.White;
            animationType = NPCID.BlueSlime;
            if (Main.hardMode)
            {
                NPC.defense = 5;
                NPC.lifeMax = 160;
                NPC.damage = 34;
            }
            if (NPC.downedPlantBoss)
            {
                NPC.defense = 8;
                NPC.lifeMax = 190;
                NPC.damage = 38;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(NPC.getRect(), ItemID.Gel, Main.rand.Next(1, 3));
            Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.DarkEnergy>());

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (modPlayer.DeimosteelCharm)
            {
                if (Main.rand.NextFloat() <= 0.7f)
                {
                    Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
            else
            {
                if (Main.rand.NextFloat() <= 0.35f)
                {
                    Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
        }
        //int frame = 0;
        //public override void FindFrame(int frameHeight)
        //{
        //    if (NPC.velocity.Y == 0)
        //    {
        //        NPC.frameCounter--;
        //        if (NPC.frameCounter <= 0)
        //        {
        //            frame++;
        //            NPC.frameCounter = 30;
        //        }
        //        if (frame >= 2)
        //        {
        //            frame = 0;
        //        }
        //    }
        //    else
        //    {
        //        frame = 1;
        //    }
        //    NPC.frame.Y = frame * frameHeight;
        //}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!TerrorbornSystem.obtainedShriekOfHorror)
            {
                return 0f;
            }
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary)
            {
                return 0f;
            }
            if (NPC.downedMoonlord)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDaySlime.Chance * 0.5f;
        }
    }
}

