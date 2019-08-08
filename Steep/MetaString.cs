using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Steep
{
  public static class MetaString
  {
    public static MetaString3 Create(char c0, char c1, char c2)
    {
      return new MetaString3 { C0 = c0, C1 = c1, C2 = c2 };
    }
  }
  public struct MetaString2
  {
    public static readonly MetaString2 Empty = default;

    internal char C0;
    internal char C1;

    public int CalcLength()
    {
      if (C0 == '\0')
        return 0;
      if (C1 == '\0')
        return 1;

      return 2;
    }

    public static MetaString2 Create(char c0, char c1)
    {
      return new MetaString2 { C0 = c0, C1 = c1 };
    }

    public static MetaString2 Create(char c)
    {
      return new MetaString2 { C0 = c, C1 = '\0' };
    }

    public static MetaString2 Create()
    {
      return new MetaString2 { C0 = '\0', C1 = '\0' };
    }

    public int IndexOf(char c)
    {
      if (C0 == c)
        return 0;

      if (C0 != '\0' && C1 == c)
        return 1;

      return -2;
    }

    public bool Contains(char c)
      => IndexOf(c) > -1;

    public bool Contains(string s)
    {
      if (s == null || s.Length > 2)
        return false;

      if (s.Length == 0)
      {
        return true;
      }

      if (s.Length == 1)
      {
        return IndexOf(s[0]) > -1;
      }

      // s.Length == 2 for sure now
      return C0 == s[0] && C1 == s[1];
    }

    public override string ToString() => new string(new char[] { C0, C1 });

    public override bool Equals(object obj)
    {
      if (!(obj is MetaString2))
        return false;

      var @string = (MetaString2)obj;
      return C0 == @string.C0 &&
             C1 == @string.C1;
    }

    public override int GetHashCode()
    {
      var hashCode = 224864335;
      hashCode = hashCode * -1521134295 + C0.GetHashCode();
      hashCode = hashCode * -1521134295 + C1.GetHashCode();
      return hashCode;
    }

    public static MetaString3 operator +(MetaString2 a, char c)
    {
      return MetaString3.Create(a, c);
    }

    public char this[int key]
    {
      get
      {
        if (key == 0)
          return C0;
        if (key == 1)
          return C1;

        throw new IndexOutOfRangeException();
      }
      set
      {
        if (key == 0)
          C0 = value;

        if (key == 1)
          C1 = value;

        throw new IndexOutOfRangeException();
      }
    }

    public static bool operator ==(MetaString2 a, string s)
    {
      return !(a != s);
    }

    public static bool operator !=(MetaString2 a, string s)
    {
      if (s.Length != a.CalcLength())
        return true;

      for (var i = 0; i < s.Length; i++)
      {
        if (s[i] != a[i])
          return true;
      }
      return false;
    }
  }

  public struct MetaString3
  {
    public static readonly MetaString3 Empty = default;

    internal char C0;
    internal char C1;
    internal char C2;

    public int CalcLength()
    {
      if (C0 == '\0')
        return 0;

      if (C1 == '\0')
        return 1;

      if (C2 == '\0')
        return 2;

      return 3;
    }

    public static MetaString3 Create(char c)
    {
      return new MetaString3 { C0 = c, C1 = '\0', C2 = '\0' };
    }

    public static MetaString3 Repeat(char c)
    {
      return new MetaString3 { C0 = c, C1 = c, C2 = c };
    }

    public static MetaString3 Create(MetaString2 s)
    {
      if (s.C0 == '\0')
      {
        return new MetaString3 { C0 = '\0', C1 = '\0', C2 = '\0' };

      }
      if (s.C1 == '\0')
      {
        return new MetaString3 { C0 = s.C0, C1 = '\0', C2 = '\0' };
      }

      return new MetaString3 { C0 = s.C0, C1 = s.C1, C2 = '\0' };
    }

    public static MetaString3 Create(MetaString2 s, char c)
    {
      if (s.C0 == '\0')
      {
        return new MetaString3 { C0 = c, C1 = '\0', C2 = '\0' };

      }
      if (s.C1 == '\0')
      {
        return new MetaString3 { C0 = s.C0, C1 = c, C2 = '\0' };
      }

      return new MetaString3 { C0 = s.C0, C1 = s.C1, C2 = c };
    }

    public static MetaString3 Create(char c, MetaString2 s)
    {
      if (c == '\0')
      {
        return new MetaString3 { C0 = s.C0, C1 = s.C1, C2 = '\0' };
      }

      return new MetaString3 { C0 = c, C1 = s.C0, C2 = s.C1 };
    }

    public override string ToString() => new string(new char[] { C0, C1, C2 });

    public override bool Equals(object obj)
    {
      if (!(obj is MetaString3))
        return false;

      var @string = (MetaString3)obj;
      return C0 == @string.C0 &&
             C1 == @string.C1 &&
             C2 == @string.C2;
    }

    public override int GetHashCode()
    {
      var hashCode = 939317479;
      hashCode = hashCode * -1521134295 + C0.GetHashCode();
      hashCode = hashCode * -1521134295 + C1.GetHashCode();
      hashCode = hashCode * -1521134295 + C2.GetHashCode();
      return hashCode;
    }

    public char this[int key]
    {
      get
      {
        if (key == 0)
          return C0;

        if (key == 1)
          return C1;

        if (key == 2)
          return C2;

        throw new IndexOutOfRangeException();
      }
      set
      {
        if (key == 0)
          C0 = value;

        if (key == 1)
          C1 = value;

        if (key == 2)
          C2 = value;

        throw new IndexOutOfRangeException();
      }
    }

    public static bool operator ==(MetaString3 a, string s)
    {
      return !(a != s);
    }

    public static bool operator !=(MetaString3 a, string s)
    {
      if (s.Length != a.CalcLength())
        return true;

      for (var i = 0; i < s.Length; i++)
      {
        if (s[i] != a[i])
          return true;
      }
      return false;
    }
  }

  public ref struct AsUpperString
  {
    internal string s;

    public char this[int index]
    {
      get
      {
        return char.ToUpper(s[0]);
      }
    }

    public int IndexOf(char c)
    {
      if (char.IsLetter(c) && char.IsLower(c))
        return -1;

      return s.IndexOf(c);
    }

    public int IndexOf(string s)
    {
      if (StringContainsLowerLetters(s))
        return -1;

      return s.IndexOf(s);
    }

    public bool Contains(char c)
    => IndexOf(c) > -1;

    public bool Contains(string s)
      => IndexOf(s) > -1;

    private static bool StringContainsLowerLetters(string s)
    {
      for (var i = 0; i < s.Length; i++)
      {
        var c = s[i];
        if (char.IsLetter(c) && char.IsLower(c))
          return false;
      }

      return true;
    }

    public override string ToString() => s.ToUpper();
  }

  public ref struct AsLowerString
  {
    internal string s;

    public char this[int index]
    {
      get
      {
        return char.ToLower(s[0]);
      }
    }

    public int IndexOf(char c)
    {
      if (char.IsLetter(c) && (!char.IsLower(c)))
        return -1;

      return s.IndexOf(c);
    }

    public int IndexOf(string s)
    {
      if (StringContainsUpperLetters(s))
        return -1;

      return s.IndexOf(s);
    }

    public bool Contains(char c)
    => IndexOf(c) > -1;

    public bool Contains(string s)
      => IndexOf(s) > -1;

    private static bool StringContainsUpperLetters(string s)
    {
      for (var i = 0; i < s.Length; i++)
      {
        var c = s[i];
        if (char.IsLetter(c) && char.IsUpper(c))
          return false;
      }

      return true;
    }

    public override string ToString() => s.ToLower();
  }

  public ref struct Substring
  {
    string _s;
    int _startIndex;
    int _length;

    public int IndexOf(char c)
    {
      return _s.IndexOf(c, _startIndex);
    }

    public int LastIndexOf(char c)
    {
      return _s.LastIndexOf(c, _length - 1, _length - _startIndex);
    }

    public int IndexOf(string s)
    {
      return _s.IndexOf(s, _startIndex);
    }

    public int LastIndexOf(string s)
    {
      return s.LastIndexOf(s, _length - 1, _length - _startIndex);
    }

    public Substring Slice(int startIndex, int length)
    {
      return new Substring { _s = _s, _startIndex = _startIndex + startIndex, _length = length };
    }

    public override string ToString() => _s.Substring(_startIndex, _length);
  }

  public struct MetaStringUtil
  {
    public static void HelloWorld()
    {
      var s0 = MetaString2.Create('A', 'b');
      var s1 = s0 + 'c';
      if (s1 == "abc" || s0.Contains("hel"))
      {
      }
    }

    public static void StackString4Demo()
    {
      4.ToStr4(out Str4 s);

    }

    //public override string ToString()
    //{
    //}

    //public static Str Create(string s)
    //{
    //unsafe
    //{
    //  _
    //  Unsafe.AsPointer(ref s);
    //}

    //ref readonly var p = ref const_string.GetPinnableReference();

    //unsafe
    //{
    //  var bytesSpan = new ReadOnlySpan<byte>(Unsafe.AsPointer(ref p), const_string.Length * 2);
    //}

    //_bytes.Alloc(p,)

    //return new Str { };
    //}

    //public static Str Create(ReadOnlySpan<char> const_string)
    //{
    //  ref readonly var p = ref const_string.GetPinnableReference();

    //  unsafe
    //  {
    //    var bytesSpan = new ReadOnlySpan<byte>(Unsafe.AsPointer(ref p), const_string.Length * 2);
    //  }

    //  //_bytes.Alloc(p,)

    //  return new Str { };
    //}
  }
}
