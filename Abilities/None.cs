﻿namespace TerrorbornMod.Abilities
{
    class None : AbilityInfo
    {
        public override int typeInt()
        {
            return 0;
        }

        public override float Cost()
        {
            return 0f;
        }

        public override string Name()
        {
            return "None";
        }

        public override string Description()
        {
            return "This ability has not yet been implemented.";
        }
    }
    class NotYetUnlocked : AbilityInfo
    {
        public override int typeInt()
        {
            return 0;
        }

        public override float Cost()
        {
            return 0f;
        }

        public override string Name()
        {
            return "Not Yet Unlocked";
        }

        public override string Description()
        {
            return "You haven't unlocked this ability yet.";
        }
    }
}
