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
using Microsoft.Diagnostics.Runtime.Utilities;

namespace Solutions.Day01
{
    public class SolverDay02 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;            
            for (int i = 0; i < lines.Length; i++)
            {
                var longs = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();

                if (isValid(longs))
                    result++;
            }
            
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                var longs = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();

                for(int x = -1; x < longs.Count;  x++)
                {
                    List<long> longs2 = longs.Select(x => x).ToList();
                    if(x >= 0)
                    {
                        longs2.RemoveAt(x);
                    }
                    if(isValid(longs2))
                    {
                        result++;
                        break;
                    }
                }
            }

            return result;
        }
        
        public bool isValid(List<long> longs)
        {
            if (longs[0] > longs[1])
            {
                for (int x = 0; x < longs.Count - 1; x++)
                {
                    if (longs[x] <= longs[x + 1] || Math.Abs(longs[x] - longs[x + 1]) > 3)
                    {
                        break;
                    }
                    if (x == longs.Count - 2)
                        return true;
                }
            }
            if (longs[0] < longs[1])
            {
                for (int x = 0; x < longs.Count - 1; x++)
                {
                    if (longs[x] >= longs[x + 1] || Math.Abs(longs[x] - longs[x + 1]) > 3)
                    {
                        break;
                    }
                    if (x == longs.Count - 2)
                        return true;
                }
            }
            return false;
        }
    }
}
