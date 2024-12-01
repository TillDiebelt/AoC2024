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

namespace Solutions.Day00
{
    public class SolverDay00 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            //Parse
            // 0,3 -> 1,4
            // 2,1 -> 4,3
            //var rocklines = lines.Select(x => x.Split("->").ToList().Select(y => (Int32.Parse(y.Trim().Split(',')[0]), Int32.Parse(y.Trim().Split(',')[1]))).ToList());

            //1,3,5,1,2,455,6
            //var longs = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x));

            //1
            //3
            var sum = lines.Select(y => Convert.ToInt64(y)).Map(x => x).Reduce((x, y) => x + y);


            //Solve
            long result = sum;
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Parse
            // 0,3 -> 1,4
            // 2,1 -> 4,3
            //var rocklines = lines.Select(x => x.Split("->").ToList().Select(y => (Int32.Parse(y.Trim().Split(',')[0]), Int32.Parse(y.Trim().Split(',')[1]))).ToList());

            //1,3,5,1,2,455,6
            //var longs = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x));

            //1
            //3
            var sum = lines.Select(y => Convert.ToInt64(y)).Map(x => x).Reduce((x, y) => x + y);


            //Solve
            long result = Utils.GaussSum(10, 5);
            return result;
        }
    }
}
