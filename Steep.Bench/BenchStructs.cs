using System.Runtime.InteropServices;

namespace Steep.Bench
{
   [StructLayout(LayoutKind.Auto, Pack=1)]
  public struct Bytes16
  {
    public byte _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15;
  }

  [StructLayout(LayoutKind.Auto, Pack=1)]
  public struct Bytes32
  {
    public byte _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15;
    public byte x_0, x_1, x_2, x_3, x_4, x_5, x_6, x_7, x_8, x_9, x_10, x_11, x_12, x_13, x_14, x_15;
  }

  [StructLayout(LayoutKind.Auto, Pack=1)]
  public struct Bytes64
  {
    public byte _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15;
    public byte x_0, x_1, x_2, x_3, x_4, x_5, x_6, x_7, x_8, x_9, x_10, x_11, x_12, x_13, x_14, x_15;

    public byte y_0, y_1, y_2, y_3, y_4, y_5, y_6, y_7, y_8, y_9, y_10, y_11, y_12, y_13, y_14, y_15;
    public byte z_0, z_1, z_2, z_3, z_4, z_5, z_6, z_7, z_8, z_9, z_10, z_11, z_12, z_13, z_14, z_15;
  }

  public struct Value16
  {
    int _0;
    int _1;
    int _2;
    public int Value;
  }

   public struct Value32
  {
    long _0;
    long _1;
    long _2;
    public long Value;
  }

  public struct Value64
  {
    long _0;
    long _1;
    long _2;
    public long Value;
  }

  public struct Value16f
  {
    float _0;
    float _1;
    float _2;
    public float Value;
  }

  public struct Value128
  {
    long _0;
    long _1;
    long _2;
    long _3;

    long _4;
    long _5;
    long _6;
    long _7;

    long _8;
    long _9;
    long _10;
    long _11;

    long _12;
    long _13;
    long _14;
    public long Value;
  }

  public struct Value256
  {
    long _0;
    long _1;
    long _2;
    long _3;

    long _4;
    long _5;
    long _6;
    long _7;

    long _8;
    long _9;
    long _10;
    long _11;

    long _12;
    long _13;
    long _14;
    long _15;

    public long _16;
    public long _17;
    public long _18;
    public long _19;

    public long _20;
    public long _21;
    public long _22;
    public long _23;

    public long _24;
    public long _25;
    public long _26;
    public long _27;

    public long _28;
    public long _29;
    public long _30;
    public long Value;
  }
}
