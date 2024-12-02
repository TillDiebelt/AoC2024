using System;
using System.IO;
using System.Linq;
using TillSharp.Math.Parser;
using TillSharp.Math.Functions;
using TillSharp.Math.Vectors;
using TillSharp.Math.Array;
using TillSharp.Math.ArrayExtender;
using TillSharp.Math;
using TillSharp.Extenders.Collections;
using TillSharp.Extenders.String;
using TillSharp.Extenders.Numerical;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Collections;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.Diagnostics.Runtime;
using BenchmarkDotNet.Toolchains.CoreRun;
using TillSharp.Base;
using System.Text.RegularExpressions;
using Microsoft.Diagnostics.Runtime.DacInterface;
using AOCLib;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Linq.Expressions;

namespace Solutions.Day01
{
    public class SolverDay01 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;
            List<long> left = new List<long>();
            List<long> right = new List<long>();
            for (int i = 0; i < lines.Length; i++)
            {
                var longs = lines[i].Split("   ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                left.Add(longs[0]);
                right.Add(longs[1]);
            }
            left.Sort();
            right.Sort();

            for(int i = 0; i < left.Count; i++)
            {
                result += Math.Abs(left[i] - right[i]);
            }
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;
            List<long> left = new List<long>();
            List<long> right = new List<long>();
            for (int i = 0; i < lines.Length; i++)
            {
                var longs = lines[i].Split("   ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                left.Add(longs[0]);
                right.Add(longs[1]);
            }
            left.Sort();
            right.Sort();

            for (int i = 0; i < left.Count; i++)
            {
                result += left[i] * right.Where(x => x == left[i]).Count();
            }
            return result;
        }
    }
}
