## Box

#### tldr;
`class Box<T>` is meant to wrap `struct` so it could be treated like an `Object`.
_note_: it is `IDisposable` itself and will call `Dispose()` on underlying `struct`.

#### More
In *everything is an object* world, it is easy to forget that every `Object` in .Net is:
a) prefaced with header describing it taking pointer sized amount of extra bytes,
b) a handle to heap memory,
c) requires indirection everytime you access data contained in it,
d) essentially a *garbage* which have to be collected sooner or later,
e) could move during garbage collection so sometimes has to be *fixed*,
f) could be _null_,
g) has indeterminate lifetime*.

Alternative approach to objects are value types (structs, built-in numeric types).
Value types are more basic in their construction with following disadvantages compared to objects:
a) no inheritance, no trully virtual members,
b) takes lifetime of context in which it is used (stack based or - in case of a member - same as `Object`)*,
c) copy on assignment behavior,
d) forced default construction,
e) is being 'boxed' if used like `Object`.

Keep in mind, these lists are non-exhaustive.

**Steep** stands with opinion that `struct` - despite it's flaws in .Net and C# lang - should be considered a primary data structure instead of classes. Classes have it's rightful place in code but before one is used, `struct` should be a first pick. Because of this, most of **Steep** is written in structs, for structs.
In order to fulfil some of the scenarios where `Object` is necesarry, **Steep** provides `class Box<T>`.
It is meant to wrap a struct, so it could be used like `Object`.

#### Naming
.Net docs denote operation of *converting* value type into `Object` as boxing so naming is consistent here. **Rust** lang has similar type named `Box<T>`. Modern **C++** provides a `unique_ptr<T>`. Keep in mind that these two types are itself value types while in .Net classes are *managed* types. So `Steep.Box<T>` is meant to be a heap-based wrapper for value type while the mentioned two types are there to wrap heap memory. **Pony** lang has a `box` reference capability which is a totally different and unrelated construction.


*ignore static lifetime for the sake of simplicity.
