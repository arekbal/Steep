
using System;

#if NOT_READY

namespace Steep
{
  [Flags]
  internal enum Bit : ulong
  {
    X00 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0001,
    X01 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0010,
    X02 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0100,
    X03 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_1000,
    X04 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0001_0000,
    X05 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0010_0000,
    X06 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0100_0000,
    X07 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_1000_0000,
    X08 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0001_0000_0000,
    X09 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0010_0000_0000,
    X10 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0100_0000_0000,
    X11 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_1000_0000_0000,
    X12 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0001_0000_0000_0000,
    X13 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0010_0000_0000_0000,
    X14 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0100_0000_0000_0000,
    X15 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__1000_0000_0000_0000,

    X16 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0001__0000_0000_0000_0000,
    X17 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0010__0000_0000_0000_0000,
    X18 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0100__0000_0000_0000_0000,
    X19 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_1000__0000_0000_0000_0000,
    X20 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0001_0000__0000_0000_0000_0000,
    X21 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0010_0000__0000_0000_0000_0000,
    X22 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0100_0000__0000_0000_0000_0000,
    X23 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_1000_0000__0000_0000_0000_0000,
    X24 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0001_0000_0000__0000_0000_0000_0000,
    X25 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0010_0000_0000__0000_0000_0000_0000,
    X26 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0100_0000_0000__0000_0000_0000_0000,
    X27 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_1000_0000_0000__0000_0000_0000_0000,
    X28 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0001_0000_0000_0000__0000_0000_0000_0000,
    X29 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0010_0000_0000_0000__0000_0000_0000_0000,
    X30 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0100_0000_0000_0000__0000_0000_0000_0000,
    X31 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___1000_0000_0000_0000__0000_0000_0000_0000,

    X32 = 0b_0000_0000_0000_0000__0000_0000_0000_0001___0000_0000_0000_0000__0000_0000_0000_0000,
    X33 = 0b_0000_0000_0000_0000__0000_0000_0000_0010___0000_0000_0000_0000__0000_0000_0000_0000,
    X34 = 0b_0000_0000_0000_0000__0000_0000_0000_0100___0000_0000_0000_0000__0000_0000_0000_0000,
    X35 = 0b_0000_0000_0000_0000__0000_0000_0000_1000___0000_0000_0000_0000__0000_0000_0000_0000,
    X36 = 0b_0000_0000_0000_0000__0000_0000_0001_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X37 = 0b_0000_0000_0000_0000__0000_0000_0010_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X38 = 0b_0000_0000_0000_0000__0000_0000_0100_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X39 = 0b_0000_0000_0000_0000__0000_0000_1000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X40 = 0b_0000_0000_0000_0000__0000_0001_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X41 = 0b_0000_0000_0000_0000__0000_0010_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X42 = 0b_0000_0000_0000_0000__0000_0100_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X43 = 0b_0000_0000_0000_0000__0000_1000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X44 = 0b_0000_0000_0000_0000__0001_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X45 = 0b_0000_0000_0000_0000__0010_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X46 = 0b_0000_0000_0000_0000__0100_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X47 = 0b_0000_0000_0000_0000__1000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,

    X48 = 0b_0000_0000_0000_0001__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X49 = 0b_0000_0000_0000_0010__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X50 = 0b_0000_0000_0000_0100__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X51 = 0b_0000_0000_0000_1000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X52 = 0b_0000_0000_0001_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X53 = 0b_0000_0000_0010_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X54 = 0b_0000_0000_0100_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X55 = 0b_0000_0000_1000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X56 = 0b_0000_0001_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X57 = 0b_0000_0010_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X58 = 0b_0000_0100_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X59 = 0b_0000_1000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X60 = 0b_0001_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X61 = 0b_0010_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X62 = 0b_0100_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
    X63 = 0b_1000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0000,
  }

  [Flags]
  internal enum Mask : ulong
  {
    X00 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0001,
    X01 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0011,
    X02 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_0111,
    X03 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0000_1111,
    X04 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0001_1111,
    X05 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0011_1111,
    X06 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_0111_1111,
    X07 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0000_1111_1111,
    X08 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0001_1111_1111,
    X09 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0011_1111_1111,
    X10 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_0111_1111_1111,
    X11 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0000_1111_1111_1111,
    X12 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0001_1111_1111_1111,
    X13 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0011_1111_1111_1111,
    X14 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__0111_1111_1111_1111,
    X15 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0000__1111_1111_1111_1111,

    X16 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0001__1111_1111_1111_1111,
    X17 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0011__1111_1111_1111_1111,
    X18 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_0111__1111_1111_1111_1111,
    X19 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0000_1111__1111_1111_1111_1111,
    X20 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0001_1111__1111_1111_1111_1111,
    X21 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0011_1111__1111_1111_1111_1111,
    X22 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_0111_1111__1111_1111_1111_1111,
    X23 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0000_1111_1111__1111_1111_1111_1111,
    X24 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0001_1111_1111__1111_1111_1111_1111,
    X25 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0011_1111_1111__1111_1111_1111_1111,
    X26 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_0111_1111_1111__1111_1111_1111_1111,
    X27 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0000_1111_1111_1111__1111_1111_1111_1111,
    X28 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0001_1111_1111_1111__1111_1111_1111_1111,
    X29 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0011_1111_1111_1111__1111_1111_1111_1111,
    X30 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___0111_1111_1111_1111__1111_1111_1111_1111,
    X31 = 0b_0000_0000_0000_0000__0000_0000_0000_0000___1111_1111_1111_1111__1111_1111_1111_1111,

    X32 = 0b_0000_0000_0000_0000__0000_0000_0000_0001___1111_1111_1111_1111__1111_1111_1111_1111,
    X33 = 0b_0000_0000_0000_0000__0000_0000_0000_0011___1111_1111_1111_1111__1111_1111_1111_1111,
    X34 = 0b_0000_0000_0000_0000__0000_0000_0000_0111___1111_1111_1111_1111__1111_1111_1111_1111,
    X35 = 0b_0000_0000_0000_0000__0000_0000_0000_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X36 = 0b_0000_0000_0000_0000__0000_0000_0001_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X37 = 0b_0000_0000_0000_0000__0000_0000_0011_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X38 = 0b_0000_0000_0000_0000__0000_0000_0111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X39 = 0b_0000_0000_0000_0000__0000_0000_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X40 = 0b_0000_0000_0000_0000__0000_0001_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X41 = 0b_0000_0000_0000_0000__0000_0011_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X42 = 0b_0000_0000_0000_0000__0000_0111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X43 = 0b_0000_0000_0000_0000__0000_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X44 = 0b_0000_0000_0000_0000__0001_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X45 = 0b_0000_0000_0000_0000__0011_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X46 = 0b_0000_0000_0000_0000__0111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X47 = 0b_0000_0000_0000_0000__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,

    X48 = 0b_0000_0000_0000_0001__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X49 = 0b_0000_0000_0000_0011__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X50 = 0b_0000_0000_0000_0111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X51 = 0b_0000_0000_0000_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X52 = 0b_0000_0000_0001_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X53 = 0b_0000_0000_0011_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X54 = 0b_0000_0000_0111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X55 = 0b_0000_0000_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X56 = 0b_0000_0001_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X57 = 0b_0000_0011_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X58 = 0b_0000_0111_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X59 = 0b_0000_1111_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X60 = 0b_0001_1111_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X61 = 0b_0011_1111_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X62 = 0b_0111_1111_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
    X63 = 0b_1111_1111_1111_1111__1111_1111_1111_1111___1111_1111_1111_1111__1111_1111_1111_1111,
  }
}
#endif
