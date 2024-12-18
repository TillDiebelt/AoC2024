using TillSharp.Math.Array;
using AOCLib;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Filters;
using System.Collections.Generic;
using System.Net.Http.Headers;
using BenchmarkDotNet.Disassemblers;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.Diagnostics.Tracing.Parsers.FrameworkEventSource;
using System.Diagnostics;
using System.Numerics;
using System;
using static Solutions.SolverDay06;
using System.Collections;

namespace Solutions
{
    public class SolverDay18 : ISolver
    {                
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;
            
            int[,] map = new int[71,71];
            List<(int x, int y)> pairs = new List<(int, int)>();

            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();
                pairs.Add(((int)nums[0], (int)nums[1]));
            }

            int by = 0;
            foreach(var p in pairs)
            {
                by++;
                map[p.y, p.x] = 1;
                if (by == 1024)
                    break;
            }
            
            var path = Pathfinder<int[,], (int x, int y)>.FindPath(
            map,
            (a) => new List<(int, int)>() { (0,0) },
                (a, b) => (b.x == 70 && b.y == 70),
                (a, b) => Math.Abs(b.x - 70) + Math.Abs(b.y - 70),
                (a, b, c) => 1,
                (a, current) => {
                    var res = map.Neighbours(current.x, current.y);
                    res = res.Where(x => a[x.y, x.x] == 0).ToList();
                    return res;
                }
                );

            result = path.Count();

            return result;
        }
      
        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            int[,] map = new int[71, 71];
            List<(int x, int y)> pairs = new List<(int, int)>();

            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();
                pairs.Add(((int)nums[0], (int)nums[1]));
            }

            int by = 0;
            foreach (var p in pairs)
            {
                by++;
                map[p.y, p.x] = 1;
                if (by == 1024)
                    break;
            }

            for(int i = by; i < pairs.Count; i++)
            {
                var p = pairs[i];
                map[p.y, p.x] = 1;
                try
                {
                    var path = Pathfinder<int[,], (int x, int y)>.FindPath(
                    map,
                    (a) => new List<(int, int)>() { (0, 0) },
                        (a, b) => (b.x == 70 && b.y == 70),
                        (a, b) => Math.Abs(b.x - 70) + Math.Abs(b.y - 70),
                        (a, b, c) => 1,
                        (a, current) => {
                            var res = map.Neighbours(current.x, current.y);
                            res = res.Where(x => a[x.y, x.x] == 0).ToList();
                            return res;
                        }
                        );
                }
                catch
                {
                    Console.WriteLine(p);
                    break;
                }
            }

            return result;
        }
    }
}