// See https://aka.ms/new-console-template for more information
using DenseBinaryEncoding.Data;
using DenseBinaryEncoding.Encoding;
using System.Collections;

IEncoder? enc = EncoderFactory.CreateEncoder(typeof(JournalRandomizationSettings));
if (enc == null)
{
    return;
}

if (args.Length == 0)
{
    JournalRandomizationSettings gs = new();
    //gs.StartLocationSettings.SetStartLocation("King's Pass");

    BitArray bits = enc.GetBits(gs);
    byte[] data = new byte[bits.Length / 8 + 1];
    bits.CopyTo(data, 0);
    Console.WriteLine(Convert.ToBase64String(data));
}
else
{
    byte[] data = Convert.FromBase64String(args[0]);
    BitArray bits = new(data);
    JournalRandomizationSettings gs = (JournalRandomizationSettings)enc.GetValue(bits, 0);
    Console.WriteLine(gs.ToString());
}
