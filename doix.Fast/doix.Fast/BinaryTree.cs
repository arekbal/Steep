using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace doix.Fast
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct Node<TValue>
    where TValue : struct
  {
    public int ParentIndex;// easier/faster removes, balancing
    public int LeftIndex;
    public int RightIndex;
    public int Key;
    public TValue Value;
  }

  public class BinaryTree<TValue> : IDisposable
    where TValue : struct
  {
    UnmanagedBuffer<Node<TValue>> _nodes;
    int _length;

    static BinaryTree()
    {
      UnmanagedBuffer<Node<TValue>>.SetSizeOfTValueType(4 * sizeof(int) + Marshal.SizeOf<TValue>());
    }

    public BinaryTree(int capacity = 8)
    {
      _nodes.Alloc(capacity);
      _nodes.ZeroMemory();
    }

    public ref TValue Add(int key) // TODO: Implement balancing
    {
      ref var root = ref _nodes.ItemRefAt(0);
      if (_length == 0)
      {
        root.Key = key;
        _length++;
        return ref root.Value;
      }

      var index = FindClosestIndex(key);
      ref var node = ref _nodes.ItemRefAt(index);
      if (node.Key == key)
        throw new ArgumentException($"key '{key}' already exists in tree");

      var newIndex = AddNode();
      ref var newNode = ref _nodes.ItemRefAt(newIndex);
      newNode.ParentIndex = index;
      newNode.Key = key;

      if (key < node.Key)
        node.LeftIndex = newIndex;
      else
        node.RightIndex = newIndex;

      BalanceTree();

      return ref newNode.Value;
    }

    void BalanceTree() => throw new NotImplementedException();

    public bool Remove(int key)
    {
      if (_length == 0)
        return false;

      var index = FindIndex(key);
      if (index.HasValue)
      {
        ref var node = ref _nodes.ItemRefAt(index.Value);

        // TODO: implement with balancing
        // ...
        // ...
        BalanceTree();
      }

      return false;     
    }

    Span<Node<TValue>> Nodes => _nodes.AsSpan(_length);

    public IEnumerable<KeyValuePair<int, TValue>> KeyValues
    {
      get
      {
        if (_length == 0)
          yield break;

        for(var i = 0; i < Nodes.Length; i++)
          yield return new KeyValuePair<int, TValue>(Nodes[i].Key, Nodes[i].Value);
      }
    }

    public IEnumerable<KeyValuePair<int, TValue>> KeyValuesAsc
    {
      get
      {
        if (_length == 0)
          yield break;
        
        foreach(var x in VisitAsc(0))
          yield return x;
      }
    }

    public IEnumerable<KeyValuePair<int, TValue>> KeyValuesDesc
    {
      get
      {
        if (_length == 0)
          yield break;

        foreach (var x in VisitDesc(0))
          yield return x;
      }
    }

    IEnumerable<KeyValuePair<int, TValue>> VisitAsc(int nodeIndex)
    {
      var leftIndex = _nodes.ItemRefAt(nodeIndex).LeftIndex;
      if (leftIndex > 0)
        foreach (var x in VisitAsc(leftIndex))
          yield return x; 

      yield return new KeyValuePair<int, TValue>(_nodes.ItemRefAt(nodeIndex).Key, _nodes.ItemRefAt(nodeIndex).Value);

      var rightIndex = _nodes.ItemRefAt(nodeIndex).RightIndex;

      if (rightIndex > 0)
        foreach (var x in VisitAsc(rightIndex))
          yield return x;
    }

    IEnumerable<KeyValuePair<int, TValue>> VisitDesc(int nodeIndex)
    {
      var rightIndex = _nodes.ItemRefAt(nodeIndex).RightIndex;
      if (rightIndex > 0)
        foreach (var x in VisitDesc(rightIndex))
          yield return x;      

      yield return new KeyValuePair<int, TValue>(_nodes.ItemRefAt(nodeIndex).Key, _nodes.ItemRefAt(nodeIndex).Value);
      
      var leftIndex = _nodes.ItemRefAt(nodeIndex).LeftIndex;
      if (leftIndex > 0)
        foreach (var x in VisitDesc(leftIndex))
          yield return x;
    }

    public bool HasKey(int key)
      => FindIndex(key).HasValue;

    int? FindIndex(int key)
    {
      var closestIndex = FindClosestIndex(key);
      if (_nodes.ItemRefAt(closestIndex).Key == key)
        return closestIndex;

      return default;
    }

    int FindClosestIndex(int key)
    {
      if (_length == 0)
        return -1;

      ref var node = ref _nodes.ItemRefAt(0);

      int nodeIndex = 0;

      while (true)
      {
        if (node.Key == key)
          return nodeIndex;

        if (key < node.Key)
        {
          if (node.LeftIndex == 0)
            return nodeIndex;

          nodeIndex = node.LeftIndex;
          node = ref _nodes.ItemRefAt(node.LeftIndex);
        }
        else
        {
          if (node.RightIndex == 0)
            return nodeIndex;

          nodeIndex = node.RightIndex;
          node = ref _nodes.ItemRefAt(node.RightIndex);
        }
      }
    }

    int AddNode()
    {
      // TODO: Resize...
      _length++;
      return _length - 1;
    }

    bool _isDisposed;

    protected virtual void Dispose(bool disposing)
    {
      if (!_isDisposed)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
        }

        _nodes.Free();

        _isDisposed = true;
      }
    }
   
    ~BinaryTree()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(false);
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      GC.SuppressFinalize(this);
      Dispose(true);
    }
  }
}
