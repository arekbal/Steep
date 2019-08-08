namespace Steep
{
  public interface IStr
  {
    int Capacity { get; }
  }

  public struct Str4 : IStr
  {
    public char f00, f01, f02, f03;

    public int Capacity => 4;

    public override string ToString() => this.StrToString();
  }

  public struct Str8 : IStr
  {
    public char f00, f01, f02, f03, f04, f05, f06, f07;

    public int Capacity => 8;

    public override string ToString() => this.StrToString();
  }

  public struct Str16 : IStr
  {
    public char f00, f01, f02, f03, f04, f05, f06, f07;
    public char f08, f09, f10, f11, f12, f13, f14, f15;

    public int Capacity => 16;

    public override string ToString() => this.StrToString();
  }

  public struct Str32 : IStr
  {
    public char f00, f01, f02, f03, f04, f05, f06, f07;
    public char f08, f09, f10, f11, f12, f13, f14, f15;
    public char f16, f17, f18, f19, f20, f21, f22, f23;
    public char f24, f25, f26, f27, f28, f29, f30, f31;

    public int Capacity => 32;

    public override string ToString() => this.StrToString();
  }

  public struct Str64 : IStr
  {
    public char f00, f01, f02, f03, f04, f05, f06, f07;
    public char f08, f09, f10, f11, f12, f13, f14, f15;
    public char f16, f17, f18, f19, f20, f21, f22, f23;
    public char f24, f25, f26, f27, f28, f29, f30, f31;

    public char f32, f33, f34, f35, f36, f37, f38, f39;
    public char f40, f41, f42, f43, f44, f45, f46, f47;
    public char f48, f49, f50, f51, f52, f53, f54, f55;
    public char f56, f57, f58, f59, f60, f61, f62, f63;

    public int Capacity => 64;

    public override string ToString() => this.StrToString();
  }

  public struct Str128 : IStr
  {
    public char f00, f01, f02, f03, f04, f05, f06, f07;
    public char f08, f09, f10, f11, f12, f13, f14, f15;
    public char f16, f17, f18, f19, f20, f21, f22, f23;
    public char f24, f25, f26, f27, f28, f29, f30, f31;

    public char f32, f33, f34, f35, f36, f37, f38, f39;
    public char f40, f41, f42, f43, f44, f45, f46, f47;
    public char f48, f49, f50, f51, f52, f53, f54, f55;
    public char f56, f57, f58, f59, f60, f61, f62, f63;

    public char f64, f65, f66, f67, f68, f69, f70, f71;
    public char f72, f73, f74, f75, f76, f77, f78, f79;
    public char f80, f81, f82, f83, f84, f85, f86, f87;
    public char f88, f89, f90, f91, f92, f93, f94, f95;

    public char f96, f97, f98, f99, f100, f101, f102, f103;
    public char f104, f105, f106, f107, f108, f109, f110, f111;
    public char f112, f113, f114, f115, f116, f117, f118, f119;
    public char f120, f121, f122, f123, f124, f125, f126, f127;

    public int Capacity => 128;

    public override string ToString() => this.StrToString();
  }
}
