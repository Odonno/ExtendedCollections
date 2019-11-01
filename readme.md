# Extended Collections for .NET

A set of types that extends C# collection types.

### Features

<details>
<summary>LimitedQueue</summary>
<br>

A `Queue` with a maximum number of items inside.

```cs
var queue = new LimitedQueue<Entity>(5); // Max of 5 items in the queue

queue.Enqueue(entity); // Enqueue an item

var (success, item) = queue.TryDequeue(); // Try to dequeue an item

queue.Enqueued += (sender, e) => {}; // Listen to enqueued items
queue.Dequeued += (sender, e) => {}; // Listen to dequeued items
```

</details>

<details>
<summary>LimitedStack</summary>
<br>

A `Stack` with a maximum number of items inside.

```cs
var stack = new LimitedStack<Entity>(5); // Max of 5 items in the stack

stack.Push(entity); // Push an item

var (success, item) = stack.TryPop(); // Try to pop an item

stack.Pushed += (sender, e) => {}; // Listen to pushed items
stack.Popped += (sender, e) => {}; // Listen to popped items
```

</details>

<details>
<summary>EntityList</summary>
<br>

A `List` to store unique items based on a key. A mix between a `List<TEntity>` and a `Dictionary<TKey, TEntity>`.

```cs
var list = new EntityList<int, Entity>(e => e.Id);

list.Upsert(entity); // Upsert a new item
// Add if the key does not exist in the list
// Update otherwise

var entity = list.Find(1); // Find item by key

list.Remove(1); // Remove using the key
```

</details>