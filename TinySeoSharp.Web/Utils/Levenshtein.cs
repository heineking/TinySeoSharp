using System.Linq;

namespace TinySeoSharp.Web.Utils
{
  internal static class Levenshtein
  {
    public static int Score(string a, string b) {
      return Score(CreateArray(a), CreateArray(b));
    } 

    private static int Score(char[] a, char[] b) {
      var matrix = new int[b.Length, a.Length];

      for (var i = 0; i < b.Length; ++i) {
        for (var j = 0; j < a.Length; ++j) {
          var cost = a[j] == b[i] ? 0 : 1;
          matrix[i, j] = MinCost(matrix, i, j) + cost;
        }
      }
      return matrix[b.Length - 1, a.Length - 1];
    }

    private static int MinCost(int[,] matrix, int i, int j) {
      if (i == 0 && j == 0) {
        return 0;
      }
      var delete = i > 0 ? matrix[i - 1, j] : int.MaxValue;
      var insert = j > 0 ? matrix[i, j - 1] : int.MaxValue;
      var replace = j > 0 && i > 0 ? matrix[i - 1, j - 1] : int.MaxValue;
      return (new [] { delete, insert, replace }).Min();
    }

    private static char[] CreateArray(string a) {
      return a
        .ToCharArray()
        .Prepend('\0')
        .ToArray();
    }
  }
}