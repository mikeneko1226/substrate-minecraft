﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Substrate.Entities
{
    using Substrate.NBT;

    public class EntityMinecartChest : EntityMinecart, IItemContainer
    {
        public static readonly SchemaNodeCompound MinecartChestSchema = MinecartSchema.MergeInto(new SchemaNodeCompound("")
        {
            new SchemaNodeList("Items", TagType.TAG_COMPOUND, ItemCollection.InventorySchema),
        });

        private static int _CAPACITY = 27;

        private ItemCollection _items;

        public EntityMinecartChest ()
            : base()
        {
            _items = new ItemCollection(_CAPACITY);
        }

        public EntityMinecartChest (Entity e)
            : base(e)
        {
            EntityMinecartChest e2 = e as EntityMinecartChest;
            if (e2 != null) {
                _items = e2._items.Copy();
            }
        }
            
        #region IItemContainer Members

        public ItemCollection  Items
        {
            get { return _items; }
        }

        #endregion


        #region INBTObject<Entity> Members

        public override Entity LoadTree (TagNode tree)
        {
            TagNodeCompound ctree = tree as TagNodeCompound;
            if (ctree == null || base.LoadTree(tree) == null) {
                return null;
            }

            TagNodeList items = ctree["Items"].ToTagList();
            _items = _items.LoadTree(items);

            return this;
        }

        public override TagNode BuildTree ()
        {
            TagNodeCompound tree = base.BuildTree() as TagNodeCompound;
            tree["Items"] = _items.BuildTree();

            return tree;
        }

        public override bool ValidateTree (TagNode tree)
        {
            return new NBTVerifier(tree, MinecartChestSchema).Verify();
        }

        #endregion


        #region ICopyable<Entity> Members

        public override Entity Copy ()
        {
            return new EntityMinecartChest(this);
        }

        #endregion
    }
}
