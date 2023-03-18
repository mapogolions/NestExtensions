#### NestPit


How to read using single slice

```c#
await using var reader = await client.PointInTimeReader("index", size: 10_000, slice: 1);
var slice = reader.Slices.Single();
var documents = await slice.Documents();
```

How to read using N slices

```c#
await using var reader = await client.PointInTimeReader("index", size: 10_000, slices: 4);
var slices = reader.Slices;
await Task.WhenAll(slices.Select(HandleSlice));
```
