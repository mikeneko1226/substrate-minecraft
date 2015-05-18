# Introduction #

substrate (n.): An underlying layer.  A surface to which a substance adheres.

**Substrate** is an SDK for reading, writing, and manipulating data in Minecraft worlds.  Like its name implies, Substrate is designed to be the underlying foundation of third party Minecraft tools and applications that need to access world data.  Substrate is written in C# 3.0 and is compatible with .NET Framework 2.0 or higher, as well as Mono for Linux.  Substrate should be compatible with all CLI languages that meet the minimum framework requirements.

Although Substrate is nominally NBT (Named-Binary-Tag)-oriented, it is designed to be flexible and adaptable to future changes in the world data specifications, as well as being able to easily interoperate with related data formats.  The components that make up Minecraft worlds, such as blocks, chunks, regions, players, entities, and level definitions, are all abstracted from each other and then composed to create the actual data models.  For example, these abstractions allow Substrate to easily support Alpha (pre-region) and Beta (post-region) map formats simultaneously without affecting the underlying block and chunk abstractions.

# Important Classes #

The following list briefly describes some of the more important classes that would be used in a typical tool for processing Beta maps, although many other classes and interfaces exist for composing other data models.

  * **Block** - A location-independent storage unit for a single block.
  * **BlockRef** - A reference to a block in a container object, such as a chunk or a global manager.  Unlike Blocks, BlockRefs expose location and lighting information.  Changes to BlockRefs are automatically reflected in the container from which the BlockRef originated.
  * **Chunk** - A standalone representation of a complete Alpha/Beta chunk.  A chunk exposes access to underlying blocks and entities.
  * **ChunkRef** - Like BlockRef, a ChunkRef refers to a chunk stored in a container object, such as a Region or global manager.  When a ChunkRef is modified, a cache manager (usually the container object) is notified so that a chunk will not be discarded until it has been saved, even if all references to the ChunkRef or its underlying Chunk are lost in the application.
  * **Region** - In Beta 1.3+ maps, a Region is a collection of 32x32 chunks.  On these maps, Regions are the backing store of map data.  A region is an instance of a ChunkContainer.
  * **BlockManager** - A BlockManager (as with all Manager objects) presents access to all blocks in a global view.  Blocks can be manipulated through a BlockManager without needing to worry about managing the underlying chunks or their boundaries.  The BlockManager works with a ChunkManager to handle all of the bookkeeping.
  * **ChunkManager** - The ChunkManager presents a global view of chunks without needing to know whether they're stored individually on disk or collected into region files.  A ChunkManager is another instance of a ChunkContainer.
  * **RegionManager** - RegionManagers provide the main interface for interacting with an enumerating region files.
  * **BlockContainer** - A BlockContainer is an interface that provides a common set of rules for working with Blocks.  Several flavors of BlockContainer interfaces exist to deal with differences in available Block data (e.g., a Block has no lighting data, which has no meaning outside the context of its container, whereas a BlockRef does).
  * **ChunkContainer** - A ChunkContainer is an interface that provides a common set of rules for working with sets of Chunks.  A ChunkContainer is also charged with maintaining an internal cache of recently used chunks, which may transparently be released or recreated as necessary to preserve memory, while simultaneously ensuring that any modified Chunks (via ChunkRefs) are not discarded before they are saved.
  * **TileEntity** - TileEntities are properties of some blocks like chests and furnaces.  Substrate strongly enforces TileEntity consistency via the Block and Chunk interfaces that expose access to them.  Each unique TileEntity is defined as its own class, and new classes may be registered with Substrate at any time.
  * **Entity** - Entities are the free-floating objects like cows and arrows in the world, and are stored with the chunks they exist inside of.  Like TileEntities, all Entities expose unique data and have their own classes defined.  New Entity definitions can be registered with Substrate.
  * **Player** - Players are special entities representing players in the world.
  * **PlayerManager** - The PlayerManager allows access to all player data on an SMP world.
  * **Level** - The Level class exposes basic data stored in level.dat, such as the random seed or the default spawn location.
  * **World** - A top-level class for gaining access to the other managers.
  * **NBT.`*`** - The NBT namespace provides access to the raw NBT parser, tag types, and a separate schema-based validation mechanism.  The NBT interfaces can be used if any of the existing Substrate APIs are insufficient for a particular task or dealing with unique nonstandard data.


# Example #

The following example demonstrates replacing one block type with another globally in a world.

```
        public void Replace (int oldid, int newid)
        {
            NBTWorld world = new BetaWorld("path/to/world");

            foreach (ChunkRef chunk in world.ChunkManager) {
                // Process Chunk
                for (int y = 0; y <= 127; y++) {
                    for (int x = 0; x <= 15; x++) {
                        for (int z = 0; z <= 15; z++) {
                            // Attempt to replace block
                            int oldBlock = chunk.GetBlockID(x , y, z);
                            if (oldBlock == oldid) {
                                chunk.SetBlockID(x, y, z, newid);
                                chunk.SetBlockData(x, y, z, 0);
                                // TileEntity consistency is implicitly maintained
                            }
                        }
                    }
                }

                // Save after each chunk so we can release unneeded chunks back to the system
                world.ChunkManager.Save();
            }
        }
```