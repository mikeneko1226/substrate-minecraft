﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Substrate.Entities
{
    using Substrate.NBT;

    public class EntityChicken : EntityMob
    {
        public static readonly SchemaNodeCompound ChickenSchema = MobSchema.MergeInto(new SchemaNodeCompound("")
        {
            new SchemaNodeString("id", "Chicken"),
        });

        public EntityChicken ()
            : base("Chicken")
        {
        }

        public EntityChicken (Entity e)
            : base(e)
        {
        }


        #region INBTObject<Entity> Members

        public override bool ValidateTree (TagNode tree)
        {
            return new NBTVerifier(tree, ChickenSchema).Verify();
        }

        #endregion


        #region ICopyable<Entity> Members

        public override Entity Copy ()
        {
            return new EntityChicken(this);
        }

        #endregion
    }
}
