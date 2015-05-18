# Introduction #

Over time as I discover the limits of the CLR JIT, I will post some tips on how to use Substrate most efficiently.  When iterating over entire maps at the block level, small changes in your code can amount to a huge difference in runtimes.

# Tip 1 #

When iterating over all blocks in a ChunkRef, cache the AlphaBlockCollection into a local variable.

```cs

foreach (!ChunkRef chunk in chunkMan) {
!AlphaBlockCollection blocks = chunk.Blocks; // Cache the reference

for (int x = 0; x < blocks.XDim; x++) {
for (int z = 0; z < blocks.ZDim; z++) {
for (int y = 0; y < blocks.YDim; y++) {
int id = blocks.GetID(x, y, z);
// Do stuff...
}
}
}
}
```

The JIT will not effectively inline the chunk.Blocks lookup, so in a tight loop this could cut the runtime of your loop in half.  The blocks.XDim, YDim, etc., lookups will most likely be inlined and so should not pose any detriment to performance.