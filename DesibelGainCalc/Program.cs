using System;

while (true)
{
    int input;
    do
    {
        Console.Write("Gain: ");
        string? inputString = Console.ReadLine();
        if (Int32.TryParse(inputString, out input))
        {
            break;
        }

        if (Console.IsInputRedirected && inputString is null)
        {
            throw new InvalidOperationException("Input has no lines available.");
        }

        Console.WriteLine($"Not A Number '{inputString}'");
    } while (true);

    string? referenceUnit;
    do
    {
        Console.Write("Gain Unit (dBW, dBm, dBf, dBV, dBµV): ");
        referenceUnit = Console.ReadLine();

        if (referenceUnit is "dBW" or "dBm" or "dBf" or "dBV" or "dBµV")
        {
            break;
        }

        if (Console.IsInputRedirected && referenceUnit is null)
        {
            throw new InvalidOperationException("Input has no lines available.");
        }

        Console.WriteLine("Gain unit not given.");
    } while (true);

    (int whole10, int rem10) = Math.DivRem(input, 10);
    (int whole3, int rem3) = Math.DivRem(rem10, 3);

    double multiplier = Math.Pow(10, whole10) * Math.Pow(2, whole3);

    multiplier *= rem3 switch
    {
        -2 => 0.625,    // Add -2 dB = 1 dB - 3 dB => 1.25/2 = 0.625.
        -1 => 0.78125,  // Add -1 dB = 2dB - 3 dB
        0 => 1,         // Do Nothing.
        1 => 1.25,      // Add 1 dB = 10 dB - 3 dB - 3 dB - 3 dB => 10/2/2/2 = 1.25
        2 => 1.5625,    // Add 2 dB = 1 dB + 1 dB => 1.25^2 = 1.5625.
        _ => throw new InvalidOperationException("x mod 3 returned unexpected value.")
    };

    (double referenceUnitValue, string outUnit) = referenceUnit[2..] switch
    {
        "W" => (1, "W"),
        "m" => (1.0E-3, "mW"),
        "f" => (1.0E-15, "fW"),
        "V" => (1, "V"),
        "µV" => (1.0E-6, "µV"),
        _ => throw new InvalidOperationException("Shouldn't be possible.")
    };

    double outPower = referenceUnitValue * multiplier;

    Console.WriteLine();
    Console.WriteLine($"Multiplier: {multiplier}");
    Console.WriteLine($"Output Power: {outPower} {outUnit}");
    Console.WriteLine();
    Console.WriteLine("Press 'Ctrl+C' to exit.");
    Console.WriteLine();
}
