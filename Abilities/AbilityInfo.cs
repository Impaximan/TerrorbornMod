﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Abilities
{
    class AbilityInfo
    {
        public virtual Texture2D texture()
        {
            return (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Abilities/ShriekOfHorror_Icon");
        }

        public virtual int typeInt()
        {
            return 69;
        }

        public virtual bool canUse(Player player)
        {
            return true;
        }

        public virtual string Name()
        {
            return "pootis";
        }

        public virtual string Description()
        {
            return "Summons pootis on use";
        }

        public virtual bool HeldDown()
        {
            return false;
        }

        public virtual float Cost()
        {
            return 25f;
        }

        public virtual void OnUse(Player player)
        {

        }
    }
}
