using System;
using System.Collections.Generic;

namespace DigitalLogicLab.Logic
{
    public enum CircuitType
    {
        AND = 0,
        OR = 1,
        NOT = 2,
        NAND = 3,
        NOR = 4,
        MUX = 5,
        DEMUX = 6
    }

    public struct TruthTableRow
    {
        public int[] Inputs;
        public int[] Outputs;

        public TruthTableRow(int[] inputs, int[] outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }
    }

    public class TruthTable
    {
        public CircuitType Circuit { get; set; }
        public string[] InputNames { get; set; }
        public string[] OutputNames { get; set; }
        public List<TruthTableRow> Rows { get; set; }

        public TruthTable(CircuitType circuit, string[] inputNames, string[] outputNames)
        {
            Circuit = circuit;
            InputNames = inputNames;
            OutputNames = outputNames;
            Rows = new List<TruthTableRow>();
        }
    }

    /// <summary>
    /// Pure static C# logic evaluation engine without any UnityEngine dependencies.
    /// Strictly unit-testable.
    /// </summary>
    public static class LogicEngine
    {
        public static string GetCircuitName(CircuitType type)
        {
            switch (type)
            {
                case CircuitType.AND: return "AND Gate";
                case CircuitType.OR: return "OR Gate";
                case CircuitType.NOT: return "NOT Gate (Inverter)";
                case CircuitType.NAND: return "NAND Gate (Universal)";
                case CircuitType.NOR: return "NOR Gate (Universal)";
                case CircuitType.MUX: return "2:1 Multiplexer (MUX)";
                case CircuitType.DEMUX: return "1:2 Demultiplexer (DEMUX)";
                default: return type.ToString();
            }
        }

        public static string GetBooleanExpression(CircuitType type)
        {
            switch (type)
            {
                case CircuitType.AND: return "Y = A · B";
                case CircuitType.OR: return "Y = A + B";
                case CircuitType.NOT: return "Y = ¬A";
                case CircuitType.NAND: return "Y = ¬(A · B)";
                case CircuitType.NOR: return "Y = ¬(A + B)";
                case CircuitType.MUX: return "Y = ¬S · I0 + S · I1";
                case CircuitType.DEMUX: return "Y0 = D · ¬S,  Y1 = D · S";
                default: return "";
            }
        }

        public static string[] GetInputLabels(CircuitType type)
        {
            switch (type)
            {
                case CircuitType.AND:
                case CircuitType.OR:
                case CircuitType.NAND:
                case CircuitType.NOR:
                    return new string[] { "A", "B" };
                case CircuitType.NOT:
                    return new string[] { "A" };
                case CircuitType.MUX:
                    return new string[] { "I0", "I1", "S" };
                case CircuitType.DEMUX:
                    return new string[] { "D", "S" };
                default:
                    return Array.Empty<string>();
            }
        }

        public static string[] GetOutputLabels(CircuitType type)
        {
            switch (type)
            {
                case CircuitType.DEMUX:
                    return new string[] { "Y0", "Y1" };
                default:
                    return new string[] { "Y" };
            }
        }

        /// <summary>
        /// Evaluates logic function according to circuit type and inputs.
        /// </summary>
        public static int[] Evaluate(CircuitType type, int[] inputs)
        {
            if (inputs == null)
            {
                throw new ArgumentNullException(nameof(inputs));
            }

            switch (type)
            {
                case CircuitType.AND:
                {
                    int a = inputs.Length > 0 ? inputs[0] : 0;
                    int b = inputs.Length > 1 ? inputs[1] : 0;
                    int y = (a == 1 && b == 1) ? 1 : 0;
                    return new int[] { y };
                }

                case CircuitType.OR:
                {
                    int a = inputs.Length > 0 ? inputs[0] : 0;
                    int b = inputs.Length > 1 ? inputs[1] : 0;
                    int y = (a == 1 || b == 1) ? 1 : 0;
                    return new int[] { y };
                }

                case CircuitType.NOT:
                {
                    int a = inputs.Length > 0 ? inputs[0] : 0;
                    int y = (a == 0) ? 1 : 0;
                    return new int[] { y };
                }

                case CircuitType.NAND:
                {
                    int a = inputs.Length > 0 ? inputs[0] : 0;
                    int b = inputs.Length > 1 ? inputs[1] : 0;
                    int y = !(a == 1 && b == 1) ? 1 : 0;
                    return new int[] { y };
                }

                case CircuitType.NOR:
                {
                    int a = inputs.Length > 0 ? inputs[0] : 0;
                    int b = inputs.Length > 1 ? inputs[1] : 0;
                    int y = !(a == 1 || b == 1) ? 1 : 0;
                    return new int[] { y };
                }

                case CircuitType.MUX:
                {
                    // Inputs: [I0, I1, S]
                    int i0 = inputs.Length > 0 ? inputs[0] : 0;
                    int i1 = inputs.Length > 1 ? inputs[1] : 0;
                    int s = inputs.Length > 2 ? inputs[2] : 0;
                    int y = (s == 1) ? i1 : i0;
                    return new int[] { y };
                }

                case CircuitType.DEMUX:
                {
                    // Inputs: [D, S] -> Outputs: [Y0, Y1]
                    // DEMUX: Y0 = D && !S, Y1 = D && S
                    int d = inputs.Length > 0 ? inputs[0] : 0;
                    int s = inputs.Length > 1 ? inputs[1] : 0;
                    int y0 = (d == 1 && s == 0) ? 1 : 0;
                    int y1 = (d == 1 && s == 1) ? 1 : 0;
                    return new int[] { y0, y1 };
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported circuit type: {type}");
            }
        }

        /// <summary>
        /// Generates full truth table dynamically by calling Evaluate.
        /// Never hardcodes output values.
        /// </summary>
        public static TruthTable GetTruthTable(CircuitType type)
        {
            string[] inLabels = GetInputLabels(type);
            string[] outLabels = GetOutputLabels(type);
            TruthTable table = new TruthTable(type, inLabels, outLabels);

            int numInputs = inLabels.Length;
            int totalCombinations = 1 << numInputs; // 2^numInputs

            for (int i = 0; i < totalCombinations; i++)
            {
                int[] rowInputs = new int[numInputs];
                for (int bit = 0; bit < numInputs; bit++)
                {
                    // standard MSB-first ordering: (i >> (numInputs - 1 - bit)) & 1
                    rowInputs[bit] = (i >> (numInputs - 1 - bit)) & 1;
                }

                int[] rowOutputs = Evaluate(type, rowInputs);
                table.Rows.Add(new TruthTableRow(rowInputs, rowOutputs));
            }

            return table;
        }

        public static string GetExplanation(CircuitType type)
        {
            switch (type)
            {
                case CircuitType.AND:
                    return "The AND gate gives an output of 1 ONLY when all of its inputs are 1. If any input is 0, the output remains 0.";
                case CircuitType.OR:
                    return "The OR gate gives an output of 1 if AT LEAST ONE input is 1. It only outputs 0 when all inputs are 0.";
                case CircuitType.NOT:
                    return "The NOT gate (inverter) produces the exact opposite of its single input: 0 becomes 1, and 1 becomes 0.";
                case CircuitType.NAND:
                    return "NAND is the inverse of AND. It outputs 0 only when all inputs are 1. It is a universal gate capable of building any logic circuit.";
                case CircuitType.NOR:
                    return "NOR is the inverse of OR. It outputs 1 only when all inputs are 0. Like NAND, it is a universal gate.";
                case CircuitType.MUX:
                    return "A 2:1 Multiplexer (data selector) chooses between data lines I0 and I1 based on select line S. When S=0, Y=I0. When S=1, Y=I1.";
                case CircuitType.DEMUX:
                    return "A 1:2 Demultiplexer takes a single data input D and routes it to either Y0 (when S=0) or Y1 (when S=1). When D=0, both outputs are 0. Both outputs are NEVER 1 at the same time.";
                default:
                    return "";
            }
        }

        public static string GetPracticalApplication(CircuitType type)
        {
            switch (type)
            {
                case CircuitType.AND:
                    return "Safety interlock switches (e.g. machine only turns on if door is closed AND start button is pressed).";
                case CircuitType.OR:
                    return "Alarm systems (e.g. fire alarm triggers if smoke sensor OR manual pull station is activated).";
                case CircuitType.NOT:
                    return "Low-light night sensor (inverting a daylight photocell output to turn lights on at dark).";
                case CircuitType.NAND:
                    return "NAND Flash memory chips found in modern SSDs, USB drives, and smartphones.";
                case CircuitType.NOR:
                    return "NOR Flash memory for fast bootloader/firmware execution in microcontrollers.";
                case CircuitType.MUX:
                    return "Telecommunications and audio routing (combining multiple audio/data channels across a shared transmission cable).";
                case CircuitType.DEMUX:
                    return "Memory address decoders and serial-to-parallel data distribution circuits.";
                default:
                    return "";
            }
        }
    }
}
