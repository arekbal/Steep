using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Linq;

using static LangExt;

namespace Steep.Bench
{
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  public class loop_unrolling
  {
    int[] data;
    List<int> list_data;
    int[] sm_data;
    int[] md_data;

    public loop_unrolling()
    {
      data = Enumerable.Range(0, 9999).ToArray();
      list_data = new List<int>(data);
      sm_data = Enumerable.Range(0, 7).ToArray();
      md_data = Enumerable.Range(0, 12).ToArray();
    }

    int ProcessItem(int x)
    {
      x++;
      return x;
    }

    void ProcItem(int x)
    {
      x++;
      x++;
      x--;
    }

    [Benchmark, BenchmarkCategory("big")]
    public void plain_foreach()
    {
      int x = 0;
      foreach (var i in data)
        x = ProcessItem(i);
    }

    [Benchmark, BenchmarkCategory("big")]
    public void plain_for()
    {
      int x = 0;

      for (var i = 0; i < data.Length; i++)
        x = ProcessItem(data[i]);
    }

    [Benchmark, BenchmarkCategory("big")]
    public void unrolled_by_8_big_data_plain_for()
    {
      UnrollBy8(data);
    }

    [Benchmark, BenchmarkCategory("mid")]
    public void unrolled_by_8_mid_data_plain_for()
    {
      UnrollBy8(md_data);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("small")]
    public void unrolled_by_8_sm_data_plain_for()
    {
      UnrollBy8(sm_data);
    }

    [Benchmark, BenchmarkCategory("big")]
    public void unrolled_by_4_big_data_plain_for()
    {
      UnrollBy4(data);
    }

    [Benchmark, BenchmarkCategory("mid")]
    public void unrolled_by_4_mid_data_plain_for()
    {
      UnrollBy4(md_data);
    }

    [Benchmark, BenchmarkCategory("small")]
    public void unrolled_by_4_sm_data_plain_for()
    {
      UnrollBy4(sm_data);
    }

    // TODO: What is that?
    // [Benchmark]
    // [BenchmarkCategory("big", "list")]
    // public void unrolled_by_8_list_ForEach()
    // {
    //   list_data.ForEach(ProcItem);
    // }

    [Benchmark]
    [BenchmarkCategory("big", "list")]
    public void unrolled_by_8_list_for()
    {
      var length = list_data.Count / 8;
      for (var i = 0; i < length; i += 8)
      {
        ProcItem(list_data[i]);
        ProcItem(list_data[i + 1]);
        ProcItem(list_data[i + 2]);
        ProcItem(list_data[i + 3]);
        ProcItem(list_data[i + 4]);
        ProcItem(list_data[i + 5]);
        ProcItem(list_data[i + 6]);
        ProcItem(list_data[i + 7]);
      }

      length = list_data.Count % 8;
      for (var i = 0; i < length; i += 8)
        ProcItem(list_data[i]);
    }

    void UnrollBy4(int[] data)
    {
      int x = 0;

      var length = data.Length / 4;
      for (var i = 0; i < length; i += 4)
      {
        x = ProcessItem(data[i]);
        x = ProcessItem(data[i + 1]);
        x = ProcessItem(data[i + 2]);
        x = ProcessItem(data[i + 3]);
      }

      length = data.Length % 4;
      for (var i = 0; i < length; i += 4)
        x = ProcessItem(data[i]);
    }

    void UnrollBy8(int[] data)
    {
      int x = 0;

      var length = data.Length / 8;
      for (var i = 0; i < length; i += 8)
      {
        x = ProcessItem(data[i]);
        x = ProcessItem(data[i + 1]);
        x = ProcessItem(data[i + 2]);
        x = ProcessItem(data[i + 3]);
        x = ProcessItem(data[i + 4]);
        x = ProcessItem(data[i + 5]);
        x = ProcessItem(data[i + 6]);
        x = ProcessItem(data[i + 7]);
      }

      length = data.Length % 8;
      for (var i = 0; i < length; i += 8)
        x = ProcessItem(data[i]);
    }
  }
}
