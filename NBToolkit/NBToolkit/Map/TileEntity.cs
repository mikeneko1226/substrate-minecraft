﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NBToolkit.Map
{
    using NBT;
    using Utility;

    public class TileEntity : ICopyable<TileEntity>
    {
        private NBT_Compound _tree;

        public NBT_Compound Root
        {
            get { return _tree; }
        }

        public string ID
        {
            get { return _tree["id"].ToNBTString(); }
        }

        public int X
        {
            get { return _tree["x"].ToNBTInt(); }
            set { _tree["x"] = new NBT_Int(value); }
        }

        public int Y
        {
            get { return _tree["y"].ToNBTInt(); }
            set { _tree["y"] = new NBT_Int(value); }
        }

        public int Z
        {
            get { return _tree["z"].ToNBTInt(); }
            set { _tree["z"] = new NBT_Int(value); }
        }

        public TileEntity (string id)
        {
            _tree = new NBT_Compound();
            _tree["id"] = new NBT_String(id);
            _tree["x"] = new NBT_Int();
            _tree["y"] = new NBT_Int();
            _tree["z"] = new NBT_Int();
        }

        public TileEntity (NBT_Compound tree)
        {
            _tree = tree;
        }

        public TileEntity (NBTSchemaNode schema)
        {
            _tree = schema.BuildDefaultTree() as NBT_Compound;
        }

        public bool Verify ()
        {
            NBTVerifier v = new NBTVerifier(Root, BaseSchema);
            return v.Verify();
        }

        public bool Verify (NBTSchemaNode schema)
        {
            NBTVerifier v = new NBTVerifier(Root, schema);
            return v.Verify();
        }

        public bool LocatedAt (int x, int y, int z)
        {
            return _tree["x"].ToNBTInt().Data == x &&
                _tree["y"].ToNBTInt().Data == y &&
                _tree["z"].ToNBTInt().Data == z;
        }

        #region Predefined Schemas

        public static readonly NBTCompoundNode InventorySchema = new NBTCompoundNode("")
        {
            new NBTScalerNode("id", NBT_Type.TAG_SHORT),
            new NBTScalerNode("Damage", NBT_Type.TAG_SHORT),
            new NBTScalerNode("Count", NBT_Type.TAG_BYTE),
            new NBTScalerNode("Slot", NBT_Type.TAG_BYTE),
        };

        public static readonly NBTCompoundNode BaseSchema = new NBTCompoundNode("")
        {
            new NBTScalerNode("id", NBT_Type.TAG_STRING),
            new NBTScalerNode("x", NBT_Type.TAG_INT),
            new NBTScalerNode("y", NBT_Type.TAG_INT),
            new NBTScalerNode("z", NBT_Type.TAG_INT),
        };

        public static readonly NBTCompoundNode FurnaceSchema = BaseSchema.MergeInto(new NBTCompoundNode("")
        {
            new NBTStringNode("id", "Furnace"),
            new NBTScalerNode("BurnTime", NBT_Type.TAG_SHORT),
            new NBTScalerNode("CookTime", NBT_Type.TAG_SHORT),
            new NBTListNode("Items", NBT_Type.TAG_COMPOUND, InventorySchema),
        });

        public static readonly NBTCompoundNode SignSchema = BaseSchema.MergeInto(new NBTCompoundNode("")
        {
            new NBTStringNode("id", "Sign"),
            new NBTScalerNode("Text1", NBT_Type.TAG_STRING),
            new NBTScalerNode("Text2", NBT_Type.TAG_STRING),
            new NBTScalerNode("Text3", NBT_Type.TAG_STRING),
            new NBTScalerNode("Text4", NBT_Type.TAG_STRING),
        });

        public static readonly NBTCompoundNode MobSpawnerSchema = BaseSchema.MergeInto(new NBTCompoundNode("")
        {
            new NBTStringNode("id", "MobSpawner"),
            new NBTScalerNode("EntityId", NBT_Type.TAG_STRING),
            new NBTScalerNode("Delay", NBT_Type.TAG_SHORT),
        });

        public static readonly NBTCompoundNode ChestSchema = BaseSchema.MergeInto(new NBTCompoundNode("")
        {
            new NBTStringNode("id", "Chest"),
            new NBTListNode("Items", NBT_Type.TAG_COMPOUND, InventorySchema),
        });

        public static readonly NBTCompoundNode MusicSchema = BaseSchema.MergeInto(new NBTCompoundNode("")
        {
            new NBTStringNode("id", "Music"),
            new NBTScalerNode("note", NBT_Type.TAG_BYTE),
        });

        public static readonly NBTCompoundNode TrapSchema = BaseSchema.MergeInto(new NBTCompoundNode("")
        {
            new NBTStringNode("id", "Trap"),
            new NBTListNode("Items", NBT_Type.TAG_COMPOUND, InventorySchema),
        });

        #endregion

        #region ICopyable<TileEntity> Members

        public TileEntity Copy ()
        {
            return new TileEntity(_tree.Copy() as NBT_Compound);
        }

        #endregion
    }
}
