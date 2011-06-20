﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Substrate.Entities
{
    using Substrate.NBT;

    public class EntitySpider : EntityMob
    {
        public static readonly SchemaNodeCompound SpiderSchema = MobSchema.MergeInto(new SchemaNodeCompound("")
        {
            new SchemaNodeString("id", "Spider"),
        });

        public EntitySpider ()
            : base("Spider")
        {
        }

        public EntitySpider (Entity e)
            : base(e)
        {
        }


        #region INBTObject<Entity> Members

        public override bool ValidateTree (TagNode tree)
        {
            return new NBTVerifier(tree, SpiderSchema).Verify();
        }

        #endregion


        #region ICopyable<Entity> Members

        public override Entity Copy ()
        {
            return new EntitySpider(this);
        }

        #endregion
    }
}
