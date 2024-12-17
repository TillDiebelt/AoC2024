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

namespace Solutions
{
    public class SolverDay17 : ISolver
    {                
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;
            List<long> registers = new();
            List<long> program = new();
         
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    i++;
                    program = lines[i].GetNumbers();
                }
                else
                {
                    var nums = lines[i].GetNumbers();
                    registers.Add(nums[0]);
                }
            }

            List<long> sol = new();
            for (long pointer = 0; pointer < program.Count; pointer += 2)
            {
                long p = program[(int)pointer];
                long combo = program[(int)pointer+1];
                switch (combo)
                {
                    case 4:
                        combo = registers[0];
                        break;
                    case 5:
                        combo = registers[1];
                        break;
                    case 6:
                        combo = registers[2];
                        break;
                    case 7:
                        throw new Exception("!");
                }
                switch (p)
                {
                    case 0:
                        long foo0 = (long)Math.Truncate(registers[0] / Math.Pow(2, combo));
                        registers[0] = foo0;
                        break;
                    case 1:
                        long foo1 = registers[1] ^ program[(int)pointer + 1];
                        registers[1] = foo1;
                        break;
                    case 2:
                        long foo2 = combo % 8;
                        registers[1] = foo2;
                        break;
                    case 3:
                        if (registers[0] != 0)
                            pointer = program[(int)pointer + 1] - 2;
                        break;
                    case 4:
                        long foo4 = registers[1] ^ registers[2];
                        registers[1] = foo4;
                        break;
                    case 5:
                        long foo5 = combo % 8;
                        sol.Add(foo5);
                        break;
                    case 6:
                        long foo6 = (long)Math.Truncate(registers[0] / Math.Pow(2, combo));
                        registers[1] = foo6;
                        break;
                    case 7:
                        long foo7 = (long)Math.Truncate(registers[0] / Math.Pow(2, combo));
                        registers[2] = foo7;
                        break;
                }
            }

            foreach(var s in sol)
            {
                Console.Write(s + ",");
            }
            Console.WriteLine();

            return result;
        }
      
        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;
            List<long> program = new();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    i++;
                    program = lines[i].GetNumbers();
                }
                else
                {
                }
            }
            
            try
            {
                result = getInitialA(ref program, 0).Min();
            }
            catch
            { }

            return result;
        }

        private static Dictionary<(int, int), long> dyn = new Dictionary<(int, int), long>();
        private List<long> getInitialA(ref List<long> program, int pointer)
        {
            if (pointer == program.Count())
                return new List<long>(){0};
            var prev = getInitialA(ref program, pointer + 1);
            List<long> As = new List<long>();
            foreach(var p in prev)
            {
                long a = p << 3;
                for(long i = 0; i < 8; i++)
                {
                    if (getOut(a + i) == program[pointer])
                    {
                        As.Add(a + i);
                    }
                }
            }
            return As;
        }

        //only works for my input
        private long getOut(long A)
        {
            long B = 0;
            long C = 0;

            B = A & 7;
            B = B ^ 1;
            C = A >> (int)B;
            B = B ^ C;
            B = B ^ 4;
            return (B & 7);
        }

    }
}