using System;

namespace DesibelGainCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Gain: ");
                string? inputstring = Console.ReadLine();
                if (Int32.TryParse(inputstring, out int input) is false)
                {
                    Console.WriteLine("Not A Number '{0}'", inputstring);
                    Console.WriteLine("Press 'Enter' to Continue...");
                    Console.ReadLine();
                    continue;
                }

                Console.Write("Gain Unit (dBW, dBm, dBf, dBV, dBµV): ");
                string? referenceUnit = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(referenceUnit))
                {
                    Console.WriteLine("Gain unit not given.");
                    Console.WriteLine("Press 'Enter' to Continue...");
                    Console.ReadLine();
                    continue;
                }

                int whole10 = input / 10;

                int rem10 = input % 10;
                int whole3 = rem10 / 3;

                int rem3 = rem10 % 3;

                double multiplier = Math.Pow(10, whole10) * Math.Pow(2, whole3);
                switch (rem3)
                {
                    // 10 dB = 10 multiplier
                    // + 3 dB = multiplier*2
                    // - 3 dB = multiplier/2
                    case -2:
                        // Add -2 dB

                        // -2 dB = 1 dB - 3 dB => 1.25/2 = 0.675 multiplier.
                        multiplier *= 0.675;
                        break;
                    case -1:
                        // Add -1 dB

                        // -1 dB = 2dB - 3 dB
                        multiplier *= Math.Pow(1.25, 2) / 2;
                        break;
                    case 0:
                        // Do Nothing.
                        break;
                    case 1:
                        // Add 1 dB.

                        // 4 dB = 10 dB - 3 dB - 3 dB => 10/2/2 = 2,5 multiplier.
                        // 1 dB = 4 dB - 3 dB => 2,5/2 = 1.25 multiplier.
                        multiplier *= 1.25;
                        break;
                    case 2:
                        // Add 2 dB.
                        // 2 dB = 1 dB + 1 dB => 1.25^2 = 1.1625 multiplier.
                        multiplier *= Math.Pow(1.25, 2);
                        break;
                    default:
                        throw new InvalidOperationException("x mod 3 returned unexpected value.");
                }
                double referenceUnitValue;
                string outUnit;
                string referenceUnitIdentifier = referenceUnit.Length >= 3 ? referenceUnit.Substring(2) : referenceUnit;
                switch (referenceUnitIdentifier)
                {
                    case "W":
                        referenceUnitValue = 1;
                        outUnit = "W";
                        break;
                    case "m":
                        referenceUnitValue = Math.Pow(10, -3);
                        outUnit = "mW";
                        break;
                    case "f":
                        referenceUnitValue = Math.Pow(10, -15);
                        outUnit = "fW";
                        break;
                    case "V":
                        referenceUnitValue = 1;
                        outUnit = "V";
                        break;
                    case "µV":
                        referenceUnitValue = Math.Pow(10, -6);
                        outUnit = "µV";
                        break;
                    default:
                        Console.WriteLine("Invalid Reference value type '{0}'", referenceUnit);
                        continue;
                }
                double outPower = referenceUnitValue * multiplier;

                Console.WriteLine();
                Console.WriteLine("Multiplier: {0}", multiplier);
                Console.WriteLine("Output Power: {0} {1}", outPower, outUnit);
                Console.WriteLine();
                Console.WriteLine("Press 'Ctrl+C' to exit.");
                Console.WriteLine();
            }
        }
    }
}
