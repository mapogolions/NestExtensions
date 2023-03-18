### NestPit


```c#
await using var reader = await client.PointInTimeReader("index", size: 10_000, slices: 4);
var slices = reader.Slices;
await Task.WhenAll(slices.Select(HandleSlice));
```
