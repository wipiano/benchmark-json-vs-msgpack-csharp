using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using MessagePack;
using Microsoft.CodeAnalysis.Diagnostics;
using Utf8Json;

namespace MessagePackVsJson
{
    class Program
    {
        static void Main(string[] args)
        {
            var bench = new SampleBenchmark();
            bench.Setup();

            Console.WriteLine($"json:                     {bench.Utf8JsonWithType().Length:000,000,000} bytes");
            Console.WriteLine($"json + gzip:              {bench.Utf8JsonWithTypeGz().Length:000,000,000} bytes");
            Console.WriteLine($"msgpack:                  {bench.MessagePackWithType().Length:000,000,000} bytes");
            Console.WriteLine($"msgpack + lz4:            {bench.MessagePackWithTypeLz4().Length:000,000,000} bytes");
            Console.WriteLine($"msgpack (typeless):       {bench.MessagePackTypeless().Length:000,000,000} bytes");
            Console.WriteLine($"msgpack (typeless) + lz4: {bench.MessagePackTypelessLz4().Length:000,000,000} bytes");

            var config = new ManualConfig();
            config.Add(Job.ShortRun);
            config.Add(MarkdownExporter.GitHub);
            config.Add(MemoryDiagnoser.Default);
            config.Add(ConsoleLogger.Default);
            config.Add(DefaultColumnProviders.Instance);
            config.Add(ThreadingDiagnoser.Default);

            BenchmarkRunner.Run<SampleBenchmark>(config);
        }
    }

    public class SampleBenchmark
    {
        private Class1[] _testData;

        [GlobalSetup]
        public void Setup()
        {
            _testData = EnumerateTestData().ToArray();
        }

        [Benchmark(Baseline = true)]
        public byte[] Utf8JsonWithType()
        {
            return JsonSerializer.Serialize(_testData);
        }

        [Benchmark]
        public byte[] Utf8JsonWithTypeGz()
        {
            using (var memory = new MemoryStream())
            using (var gz = new GZipStream(memory, CompressionLevel.Optimal))
            {
                var json = JsonSerializer.Serialize(_testData);
                gz.Write(json);

                gz.Flush();
                return memory.ToArray();
            }
        }

        [Benchmark]
        public byte[] Utf8JsonTypeless()
        {
            return JsonSerializer.NonGeneric.Serialize(_testData);
        }

        [Benchmark]
        public byte[] Utf8JsonTypelessGz()
        {
            using (var memory = new MemoryStream())
            using (var gz = new GZipStream(memory, CompressionLevel.Optimal))
            {
                var json = JsonSerializer.NonGeneric.Serialize(_testData);
                gz.Write(json);

                gz.Flush();
                return memory.ToArray();
            }
        }

        [Benchmark]
        public byte[] MessagePackWithType()
        {
            return MessagePackSerializer.Serialize(_testData);
        }

        [Benchmark]
        public byte[] MessagePackWithTypeLz4()
        {
            return LZ4MessagePackSerializer.Serialize(_testData);
        }

        [Benchmark]
        public byte[] MessagePackTypeless()
        {
            return MessagePackSerializer.Typeless.Serialize(_testData);
        }

        [Benchmark]
        public byte[] MessagePackTypelessLz4()
        {
            return LZ4MessagePackSerializer.Typeless.Serialize(_testData);
        }

        private static IEnumerable<Class1> EnumerateTestData()
        {
            var rand = new Random();

            for (var i = 0; i < 100; i++)
            {
                yield return new Class1
                             {
                                 Date = DateTime.Now,
                                 Int = rand.Next(),
                                 Double = rand.NextDouble(),
                                 Name = Guid.NewGuid().ToString(),
                                 Class2s = EnumerateClass2().ToArray()
                             };
            }

            IEnumerable<Class2> EnumerateClass2()
            {
                for (var i = 0; i < 100; i++)
                {
                    yield return new Class2
                                 {
                                     Date = DateTime.Now,
                                     Int = rand.Next(),
                                     Double = rand.NextDouble(),
                                     Name = Guid.NewGuid().ToString(),
                                     Class3s = EnumerateClass3().ToArray()
                                 };
                }
            }

            IEnumerable<Class3> EnumerateClass3()
            {
                for (var i = 0; i < 100; i++)
                {
                    yield return new Class3
                                 {
                                     Date = DateTime.Now,
                                     Int = rand.Next(),
                                     Double = rand.NextDouble(),
                                     Name = Guid.NewGuid().ToString(),
                                 };
                }
            }
        }
    }

    [MessagePackObject(true)]
    public class Class1
    {
        public DateTime Date { get; set; }

        public int Int { get; set; }

        public double Double { get; set; }

        public string Name { get; set; }

        public Class2[] Class2s { get; set; }
    }

    [MessagePackObject(true)]
    public class Class2
    {
        public DateTime Date { get; set; }

        public int Int { get; set; }

        public double Double { get; set; }

        public string Name { get; set; }

        public Class3[] Class3s { get; set; }
    }

    [MessagePackObject(true)]
    public class Class3
    {
        public DateTime Date { get; set; }

        public int Int { get; set; }

        public double Double { get; set; }

        public string Name { get; set; }
    }
}