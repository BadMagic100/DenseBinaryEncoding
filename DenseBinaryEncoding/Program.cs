﻿// See https://aka.ms/new-console-template for more information
using DenseBinaryEncoding.Data;
using DenseBinaryEncoding.Data.RandoSettings;
using DenseBinaryEncoding.Encoding;
using System.Text.Json;

IEncoder e1 = EncoderFactory.CreateEncoder(typeof(GenerationSettings))!;
IEncoder e2 = EncoderFactory.CreateEncoder(typeof(JournalRandomizationSettings))!;
IEncoder e3 = EncoderFactory.CreateEncoder(typeof(NoEncoderListTest))!;
IEncoder e4 = EncoderFactory.CreateEncoder(typeof(DictionaryTest))!;

if (args.Length == 0)
{
    GenerationSettings gs = new();
    gs.StartLocationSettings.SetStartLocation("King's Pass");

    JournalRandomizationSettings js = new();
    js.Costs.IndividualCostWeights.AddRange(new[] { 0.1, 2, -8.5 });
    js.Pools.RandomizedEnemies = new[] { "Menderbug", "Pure Vessel", "Void Idol" };

    NoEncoderListTest lt = new();
    lt.foos.Add(new ConcreteFoo("Bar"));
    lt.foos.Add(new ConcreteFoo("Baz"));
    lt.myNum1 = null;
    lt.myNum2 = 42;

    DictionaryTest dt = new();
    dt.d1.Add("k1", "v1");
    dt.d1.Add("k2", "v2");
    dt.d2.Add(new ConcreteFoo("Bar"), "v1");
    dt.d2.Add(new ConcreteFoo("Baz"), "v2");
    dt.d3.Add("k1", new ConcreteFoo("Bar"));
    dt.d3.Add("k2", new ConcreteFoo("Baz"));

    Console.WriteLine(e1.GetBits(gs).ToBase64String());
    Console.WriteLine();
    Console.WriteLine(e2.GetBits(js).ToBase64String());
    Console.WriteLine();
    Console.WriteLine(e3.GetBits(lt).ToBase64String());
    Console.WriteLine();
    Console.WriteLine(e4.GetBits(dt).ToBase64String());
    Console.WriteLine();
}
else
{
    GenerationSettings? gs = e1.GetValue(args[0].Base64ToBitArray()) as GenerationSettings;
    JournalRandomizationSettings? js = e2.GetValue(args[1].Base64ToBitArray()) as JournalRandomizationSettings;
    NoEncoderListTest? lt = e3.GetValue(args[2].Base64ToBitArray()) as NoEncoderListTest;
    DictionaryTest? dt = e4.GetValue(args[3].Base64ToBitArray()) as DictionaryTest;

    JsonSerializerOptions options = new()
    {
        IncludeFields = true,
        WriteIndented = true,
    };
    Console.WriteLine(JsonSerializer.Serialize(gs, options));
    Console.WriteLine();
    Console.WriteLine(JsonSerializer.Serialize(js, options));
    Console.WriteLine();
    Console.WriteLine(JsonSerializer.Serialize(lt, options));
    Console.WriteLine();
    Console.WriteLine(JsonSerializer.Serialize(dt, options));
    Console.WriteLine();
}
