# WBTrees
[![license](https://img.shields.io/github/license/sakapon/WBTrees.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/WBTrees.svg)](https://www.nuget.org/packages/WBTrees/)
[![NuGet](https://img.shields.io/nuget/dt/WBTrees.svg)](https://www.nuget.org/packages/WBTrees/)

Provides a basic implementation of weight-balanced binary trees.

The WBTrees library contains classes as follows:
- A list by a weight-balanced binary tree, with all `O(log n)` basic operations
  - `WBList<T>`
- A set and a map by weight-balanced binary search trees, which can be accessed by index in `O(log n)` time
  - `WBSet<T>`
  - `WBMultiSet<T>`
  - `WBMap<TKey, TValue>`
  - `WBMultiMap<TKey, TValue>`

All these trees are constructed from `Node<T>` objects.

See [Wiki](https://github.com/sakapon/WBTrees/wiki) for more information.  
This library is written in C#.
You are welcome to port this to other languages.

## Features

### A List by a Weight-Balanced Binary Tree
Provides the `WBList<T>` class as a list with all `O(log n)` basic operations.  
You can also use a `WBList<T>` as a (high-grade) double-ended queue (deque).

The following table compares time complexities of [`System.Collections.Generic.List<T>`](https://docs.microsoft.com/dotnet/api/system.collections.generic.list-1) and `WBList<T>`:
| Operation | `List<T>` | `WBList<T>` |
|:--|:-:|:-:|
| Get by Index | `O(1)` | `O(log n)` |
| Set by Index | `O(1)` | `O(log n)` |
| Remove by Index | `O(n)` | `O(log n)` |
| Insert by Index | `O(n)` | `O(log n)` |
| Prepend | `O(n)` | `O(log n)` |
| Add | `O(1)` | `O(log n)` |
| Get All | `O(n)` | `O(n)` |

### A Set and a Map by Weight-Balanced Binary Search Trees
Provides the `WBSet<T>`, `WBMultiSet<T>`, `WBMap<TKey, TValue>` and `WBMultiMap<TKey, TValue>` classes, which can be accessed by index in `O(log n)` time. All these classes are derived from the `WBTreeBase<T>` class.  
You can also use a `WBMultiSet<T>` or a `WBMultiMap<TKey, TValue>` as a priority queue with stable sorting or a double-ended priority queue.

The following table compares time complexities of [`System.Collections.Generic.SortedSet<T>`](https://docs.microsoft.com/dotnet/api/system.collections.generic.sortedset-1) and `WBSet<T>`:
| Operation | `SortedSet<T>` | `WBSet<T>` |
|:--|:-:|:-:|
| Get by Item | `O(log n)` | `O(log n)` |
| Remove by Item | `O(log n)` | `O(log n)` |
| Add | `O(log n)` | `O(log n)` |
| Get by Index | `O(n)` | `O(log n)` |
| Remove by Index | `O(n)` | `O(log n)` |
| Get Index by Item | `O(n)` | `O(log n)` |
| Get All | `O(n)` | `O(n)` |

## Algorithm
Both `WBList<T>` and `WBTreeBase<T>` are weight-balanced binary trees (not necessarily searchable by items).
The `Node<T>` class contains the `Count` property that contributes to both self-balancing and fast access by index (order).

## Target Frameworks
- .NET 5
- .NET Standard 2.0
  - [.NET Core 2.0, .NET Framework 4.6.1, etc.](https://docs.microsoft.com/dotnet/standard/net-standard)

## Setup
The WBTrees library is published to [NuGet Gallery](https://www.nuget.org/packages/WBTrees/).
Install the NuGet package via Visual Studio, etc.

You can also [download a single source file here](downloads) for competitive programming, etc.

## Usage
See [Usage on Wiki](https://github.com/sakapon/WBTrees/wiki/Usage) for coding.

## Release Notes
- **v1.0.4** The first release.
